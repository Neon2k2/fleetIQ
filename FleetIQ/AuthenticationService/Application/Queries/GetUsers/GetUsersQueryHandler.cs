using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AuthenticationService.Application.Queries.Dtos;
using AuthenticationService.Domain.Entities;
using AuthenticationService.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Application.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
    {
        private readonly ApplicationDbContext _context;

        public GetUsersQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Select(user => new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    Role = user.Roles.FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            return Result<List<UserDto>>.Success(users);
        }
    }
}
