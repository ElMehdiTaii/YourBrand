﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using Skynet.Application.Common.Interfaces;
using Skynet.Domain.Exceptions;

namespace Skynet.Application.Users.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public GetUserQuery(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    { 
        readonly ICatalogContext _context;

        public GetUserQueryHandler(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                //.AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return null!;
            }

            return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName, user.SSN, user.Email, user.Created, user.Deleted);
        }
    }
}