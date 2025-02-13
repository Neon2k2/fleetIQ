using System;

namespace AuthenticationService.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserCommandResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ApplicationDbContext _context;

    public CreateUserCommandHandler(
        IIdentityService identityService,
        IEventPublisher eventPublisher,
        ApplicationDbContext context)
    {
        _identityService = identityService;
        _eventPublisher = eventPublisher;
        _context = context;
    }

    public async Task<Result<CreateUserCommandResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        // Validate role
        if (request.Role != UserRoles.Admin && request.Role != UserRoles.Driver)
        {
            return Result<CreateUserCommandResponse>.Failure(new[] { "Invalid role provided" });
        }

        // Verify company exists
        var company = await _context.Companies.FindAsync(new object[] { request.CompanyId }, cancellationToken);
        if (company == null)
        {
            return Result<CreateUserCommandResponse>.Failure(new[] { "Company not found" });
        }

        // Create user
        var userResult = await _identityService.CreateUserAsync(new CreateUserDto
        {
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            Address = request.Address
        });

        if (!userResult.IsSuccess)
            return Result<CreateUserCommandResponse>.Failure(userResult.Errors);

        var user = userResult.Value;
        company.AddUser(user);

        await _context.SaveChangesAsync(cancellationToken);

        // Publish user created event
        await _eventPublisher.PublishAsync(new UserCreatedEvent
        {
            UserId = user.Id,
            CompanyId = company.Id,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow
        });

        return Result<CreateUserCommandResponse>.Success(new CreateUserCommandResponse
        {
            UserId = user.Id
        });
    }
}
