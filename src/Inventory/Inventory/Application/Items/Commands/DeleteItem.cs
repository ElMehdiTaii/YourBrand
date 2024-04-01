﻿using MediatR;

using YourBrand.Inventory.Domain;

namespace YourBrand.Inventory.Application.Items.Commands;

public record DeleteItem(string ItemId) : IRequest
{
    public class Handler : IRequestHandler<DeleteItem>
    {
        private readonly IInventoryContext _context;

        public Handler(IInventoryContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteItem request, CancellationToken cancellationToken)
        {
            /*
            var invoice = await _context.Items
                //.Include(i => i.Addresses)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ItemId, cancellationToken);

            if(invoice is null)
            {
                throw new Exception();
            }

            _context.Items.Remove(invoice);

            await _context.SaveChangesAsync(cancellationToken);
            */

        }
    }
}