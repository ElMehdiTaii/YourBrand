﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Identity;
using YourBrand.IdentityManagement.Application.Common.Interfaces;
using YourBrand.IdentityManagement.Domain.Entities;

using TenantCreated = YourBrand.IdentityManagement.Contracts.TenantCreated;

namespace YourBrand.IdentityManagement.Application.Tenants.Commands;

public record CreateTenantCommand(string Name, string? FriendlyName) : IRequest<TenantDto>
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, TenantDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserContext _currentTenantContext;
        private readonly IEventPublisher _eventPublisher;

        public CreateTenantCommandHandler(IApplicationDbContext context, IUserContext currentTenantContext, IEventPublisher eventPublisher)
        {
            _context = context;
            _currentTenantContext = currentTenantContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<TenantDto> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant(
                 request.Name,
                 request.FriendlyName ?? request.Name.ToLower().Replace(' ', '-'));

            _context.Tenants.Add(tenant);

            await _context.SaveChangesAsync(cancellationToken);

            tenant = await _context.Tenants
               .AsNoTracking()
               .AsSplitQuery()
               .FirstAsync(x => x.Id == tenant.Id, cancellationToken);

            await _eventPublisher.PublishEvent(new TenantCreated(tenant.Id, tenant.Name, _currentTenantContext.UserId));

            return tenant.ToDto();
        }
    }
}