using System;
using AuthenticationService.Application.Common;
using AuthenticationService.Application.DTOs;
using AuthenticationService.Domain.Entities;

namespace AuthenticationService.Application.Interfaces;

public interface IIdentityService
{
    Task<Result<ApplicationUser>> CreateUserAsync(CreateUserDto userDto);
    Task<Result<string>> AuthenticateAsync(string email, string password);
    Task<Result> AssignRoleAsync(string userId, string role);
    Task<Result> RemoveRoleAsync(string userId, string role);
    Task<Result<IList<string>>> GetUserRolesAsync(string userId);
}
