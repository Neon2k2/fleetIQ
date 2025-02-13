using System;

namespace AuthenticationService.Application.DTOs;

public record CompanyDto
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string OwnerId { get; init; }
    public AddressDto Address { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }
    public List<UserDto> Users { get; init; } = new();
}
