﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Exceptions;
using SmartValley.Ethereum;
using SmartValley.Messages.Commands;
using SmartValley.WebApi.Admin.Request;
using SmartValley.WebApi.Admin.Response;
using SmartValley.WebApi.Authentication;
using SmartValley.WebApi.Experts;
using SmartValley.WebApi.Extensions;
using SmartValley.WebApi.Users;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Admin
{
    [Route("api/admin")]
    [Authorize(Roles = nameof(RoleType.Admin))]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IExpertService _expertService;
        private readonly EthereumClient _ethereumClient;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly IMessageSession _messageSession;

        public AdminController(
            IExpertService expertService,
            IAdminService adminService,
            EthereumClient ethereumClient,
            IAuthenticationService authenticationService,
            IUserService userService,
            IMessageSession messageSession)
        {
            _adminService = adminService;
            _ethereumClient = ethereumClient;
            _expertService = expertService;
            _authenticationService = authenticationService;
            _userService = userService;
            _messageSession = messageSession;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdminRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _adminService.AddAsync(request.Address);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _adminService.GetAsync();
            return Ok(new CollectionResponse<AdminResponse>
                      {
                          Items = admins.Select(i => new AdminResponse {Address = i.Address, Email = i.Email}).ToArray()
                      });
        }

        [HttpGet("users")]
        public async Task<PartialCollectionResponse<UserResponse>> GetAllUsers(CollectionPageRequest request)
        {
            var users = await _userService.GetAsync(request.Offset, request.Count);

            return users.ToPartialCollectionResponse(UserResponse.Create);
        }

        [HttpPut("users")]
        public async Task<IActionResult> PutUserAsync([FromBody] AdminUserUpdateRequest request)
        {
            var user = await _userService.GetByAddressAsync(request.Address);
            if (user == null)
                throw new AppErrorException(ErrorCode.UserNotFound);

            await _userService.SetCanCreatePrivateProjectsAsync(user.Address, request.CanCreatePrivateProjects);

            return NoContent();
        }

        [HttpPut("experts/availability")]
        public async Task<IActionResult> SetExpertAvailabilityAsync([FromBody] AdminSetAvailabilityRequest request)
        {
            await _ethereumClient.WaitForConfirmationAsync(request.TransactionHash);
            await _expertService.SetAvailabilityAsync(request.Address, request.Value);
            return NoContent();
        }

        [HttpPut("experts/areas")]
        public async Task<IActionResult> UpdateExpertAreasAsync([FromBody] AdminExpertUpdateAreasRequest request)
        {
            var message = new UpdateExpertAreas
                          {
                              ExpertAddress = request.Address,
                              TransactionHash = request.TransactionHash,
                              UserId = User.GetUserId()
                          };

            await _messageSession.SendLocal(message);

            return NoContent();
        }

        [HttpPut("experts")]
        public async Task<IActionResult> UpdateExpertAsync([FromBody] AdminExpertUpdateRequest request)
        {
            await _userService.UpdateAsync(request.Address, request);
            await _expertService.SetInHouseAsync(request.Address, request.IsInHouse);
            await _authenticationService.ChangeEmailAsync(request.Address, request.Email);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string address, string transactionHash)
        {
            await _ethereumClient.WaitForConfirmationAsync(transactionHash);
            await _adminService.DeleteAsync(address);
            return NoContent();
        }
    }
}