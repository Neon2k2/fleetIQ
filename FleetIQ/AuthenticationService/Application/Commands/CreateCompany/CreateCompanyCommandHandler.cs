using System;
using MediatR;
using AuthenticationService.Application.Interfaces;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Domain.Enums;
using AuthenticationService.Domain.ValueObjects;
using AuthenticationService.Infrastructure.Persistence;
using AuthenticationService.Application.Common;
using AuthenticationService.Application.DTOs;

namespace AuthenticationService.Application.Commands.CreateCompany;

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result<CreateCompanyCommandResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IEventPublisher _eventPublisher;
    private readonly ApplicationDbContext _context;

    public CreateCompanyCommandHandler(
        IIdentityService identityService,
        IEventPublisher eventPublisher,
        ApplicationDbContext context)
    {
        _identityService = identityService;
        _eventPublisher = eventPublisher;
        _context = context;
    }

    public async Task<Result<CreateCompanyCommandResponse>> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        // Create owner
        var ownerResult = await _identityService.CreateUserAsync(new CreateUserDto
        {
            Email = request.OwnerEmail,
            Password = request.OwnerPassword,
            FirstName = request.OwnerFirstName,
            LastName = request.OwnerLastName,
            Role = UserRoles.Owner
        });

        if (!ownerResult.IsSuccess)
            return Result<CreateCompanyCommandResponse>.Failure(ownerResult.Errors);

        var owner = ownerResult.Value;

        // Create company
        var address = new Address(
            request.Address.Street,
            request.Address.City,
            request.Address.State,
            request.Address.Country,
            request.Address.PostalCode
        );

        var company = Company.Create(request.Name, owner, address);

        _context.Companies.Add(company);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish events
        await _eventPublisher.PublishAsync(new CompanyCreatedEvent
        {
            CompanyId = company.Id,
            Name = company.Name,
            OwnerId = owner.Id,
            CreatedAt = DateTime.UtcNow
        });

        return Result<CreateCompanyCommandResponse>.Success(new CreateCompanyCommandResponse
        {
            CompanyId = company.Id,
            OwnerId = owner.Id
        });
    }
}
