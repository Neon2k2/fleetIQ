using System;

namespace AuthenticationService.Application.DTOs;

public record UserDto
{
    public string Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string CompanyId { get; init; }
    public string Role { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? LastLogin { get; init; }
}
