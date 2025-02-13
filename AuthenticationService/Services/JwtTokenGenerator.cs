using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services;


public class JwtTokenGenerator
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenGenerator(IConfiguration config, UserManager<ApplicationUser> userManager)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(user.Email)) throw new InvalidOperationException("User email is required");

        // Get user roles
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email)
        };

        // Add roles to claims
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        // Add any custom claims
        if (!string.IsNullOrEmpty(user.PhoneNumber))
        {
            claims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured")));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured"),
            audience: _config["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_config["Jwt:ExpirationMinutes"] ?? "60")),
            notBefore: DateTime.UtcNow,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

