using System;

namespace AuthenticationService.Application.DTOs;

public record CreateUserDto
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Role { get; init; }
    public AddressDto Address { get; init; }
}
