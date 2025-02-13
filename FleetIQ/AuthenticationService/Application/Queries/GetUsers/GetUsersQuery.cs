

using System.Collections.Generic;
using AuthenticationService.Application.Queries.Dtos;
using MediatR;

namespace AuthenticationService.Application.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<Result<List<UserDto>>>
    {
    }
}

