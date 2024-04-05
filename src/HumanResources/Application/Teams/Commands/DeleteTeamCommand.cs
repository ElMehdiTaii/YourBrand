﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.HumanResources.Application.Common.Interfaces;
using YourBrand.HumanResources.Domain.Exceptions;
using YourBrand.Identity;

namespace YourBrand.HumanResources.Application.Teams.Commands;

public record DeleteTeamCommand(string TeamId) : IRequest
{
    public class Handler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly IUserContext _currentPersonService;
        private readonly IApplicationDbContext _context;
        private readonly IEventPublisher _eventPublisher;

        public Handler(IUserContext currentPersonService, IApplicationDbContext context, IEventPublisher eventPublisher)
        {
            _currentPersonService = currentPersonService;
            _context = context;
            _eventPublisher = eventPublisher;
        }

        public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            var team = await _context.Teams
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.TeamId, cancellationToken);

            if (team is null)
            {
                throw new PersonNotFoundException(request.TeamId);
            }

            _context.Teams.Remove(team);

            await _context.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishEvent(new Contracts.TeamDeleted(team.Id, _currentPersonService.UserId));

        }
    }
}