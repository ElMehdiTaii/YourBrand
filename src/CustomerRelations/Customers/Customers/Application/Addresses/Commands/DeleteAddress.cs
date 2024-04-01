﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Customers.Domain;

namespace YourBrand.Customers.Application.Commands;

public record DeleteAddress(string AddressId) : IRequest
{
    public class Handler : IRequestHandler<DeleteAddress>
    {
        private readonly ICustomersContext _context;

        public Handler(ICustomersContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteAddress request, CancellationToken cancellationToken)
        {
            var address = await _context.Addresses
                //.Include(i => i.Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.AddressId, cancellationToken);

            if (address is null)
            {
                throw new Exception();
            }

            _context.Addresses.Remove(address);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}