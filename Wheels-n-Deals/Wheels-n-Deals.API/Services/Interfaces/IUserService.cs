﻿using Wheels_n_Deals.API.DataLayer.DTO;
using Wheels_n_Deals.API.DataLayer.Models;

namespace Wheels_n_Deals.API.Services.Interfaces;

public interface IUserService
{
    Task<Guid> RegisterUserAsync(RegisterDto dto);
    Task<string> LoginUserAsync(LoginDto dto);

    Task<List<User>> GetUsersAsync();
    Task<User> GetUserByIdAsync(Guid id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> DeleteUserAsync(Guid userId);
    Task<User> UpdateUserAsync(UpdateUserDto dto);
}
