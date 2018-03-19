﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartValley.WebApi.Applications.Requests;
using SmartValley.WebApi.Applications.Responses;
using SmartValley.WebApi.WebApi;

namespace SmartValley.WebApi.Applications
{
    [Route("api/applications")]
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationService _service;

        public ApplicationsController(IApplicationService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ApplicationRequest request)
        {
            await _service.CreateAsync(request);
            return NoContent();
        }

        [HttpGet("countries")]
        public async Task<CollectionResponse<CountryResponse>> GetCountriesAsync()
        {
            var countries = await _service.GetCountriesAsync();
            return new CollectionResponse<CountryResponse>
                   {
                       Items = countries.Select(c => new CountryResponse
                                                     {
                                                         Code = c.Code
                                                     }).ToArray()
                   };
        }

        [HttpGet("categories")]
        public async Task<CollectionResponse<CategoryResponse>> GetCategoriesAsync()
        {
            var categories = await _service.GetCategoriesAsync();
            return new CollectionResponse<CategoryResponse>
                   {
                       Items = categories.Select(c => new CategoryResponse
                                                      {
                                                          Id = c.Id
                                                      }).ToArray()
                   };
        }

        [HttpGet("stages")]
        public async Task<CollectionResponse<StageResponse>> GetStagesAsync()
        {
            var stages = await _service.GetStagesAsync();
            return new CollectionResponse<StageResponse>
                   {
                       Items = stages.Select(s => new StageResponse
                       {
                                                         Id = s.Id
                                                     }).ToArray()
                   };
        }

        [HttpGet("socialmedias")]
        public async Task<CollectionResponse<SocialMediaResponse>> GetSocialMediasAsync()
        {
            var stages = await _service.GetSocialMediasAsync();
            return new CollectionResponse<SocialMediaResponse>
                   {
                       Items = stages.Select(s => new SocialMediaResponse
                       {
                                                      NetworkId = s.Id
                                                  }).ToArray()
                   };
        }
    }
}