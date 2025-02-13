using System;
using AuthenticationService.Application.Common;
using AuthenticationService.Application.DTOs;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Common.Helpers;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationService.Infrastructure.Identity;

public class IdentityService
{
    public interface IIdentityService
    {
        Task<Result<global::System.String>> AuthenticateAsync(global::System.String email, global::System.String password);
        Task<Result<ApplicationUser>> CreateUserAsync(CreateUserDto userDto);
    }

    public class IdentityService : IIdentityService

    , IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtHelper _jwtHelper;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
        }

        public async Task<Result<ApplicationUser>> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new ApplicationUser
            {
                UserName = userDto.Email,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Address = new Address(
                    userDto.Address.Street,
                    userDto.Address.City,
                    userDto.Address.State,
                    userDto.Address.Country,
                    userDto.Address.PostalCode
                )
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
                return Result<ApplicationUser>.Failure(result.Errors.Select(e => e.Description));

            if (!string.IsNullOrEmpty(userDto.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, userDto.Role);
                if (!roleResult.Succeeded)
                    return Result<ApplicationUser>.Failure(roleResult.Errors.Select(e => e.Description));
            }

            return Result<ApplicationUser>.Success(user);
        }

        public async Task<Result<string>> AuthenticateAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<string>.Failure(new[] { "Invalid credentials" });

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
                return Result<string>.Failure(new[] { "Invalid credentials" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtHelper.GenerateJwtToken(user, roles);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Result<string>.Success(token);
        }

        // Implement other interface methods...
    }
}
