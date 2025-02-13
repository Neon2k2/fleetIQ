using System;
using AuthenticationService.Application.DTOs;

using AuthenticationService.Application.Common;

namespace AuthenticationService.Application.Commands.CreateCompany;

public record CreateCompanyCommand
{
    public string Name { get; init; }
    public string OwnerEmail { get; init; }
    public string OwnerPassword { get; init; }
    public string OwnerFirstName { get; init; }
    public string OwnerLastName { get; init; }
    public AddressDto Address { get; init; }
}

public record CreateCompanyCommandResponse
{
    public string CompanyId { get; init; }
    public string OwnerId { get; init; }
}