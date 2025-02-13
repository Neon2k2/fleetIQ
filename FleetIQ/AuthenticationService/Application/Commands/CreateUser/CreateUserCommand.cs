using System;

namespace AuthenticationService.Application.Commands.CreateUser;

public record CreateUserCommand : IRequest<Result<CreateUserCommandResponse>>
{
    public string CompanyId { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Role { get; init; }
    public AddressDto Address { get; init; }
}

public record CreateUserCommandResponse
{
    public string UserId { get; init; }
}
