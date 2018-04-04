﻿using System.Threading.Tasks;
using SmartValley.Domain.Core;
using SmartValley.Domain.Entities;
using SmartValley.Domain.Interfaces;

namespace SmartValley.WebApi.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<User> GetByAddressAsync(Address address)
            => _repository.GetByAddressAsync(address);

        public Task<User> GetByIdAsync(long id)
            => _repository.GetByIdAsync(id);

        public async Task UpdateAsync(Address address, string name, string about)
        {
            var user = await _repository.GetByAddressAsync(address);

            user.Name = name;
            user.About = about;

            await _repository.UpdateWholeAsync(user);
        }
    }
}