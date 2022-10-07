using YourBrand.Warehouse.Application.Common.Models;
using YourBrand.Warehouse.Domain;
using YourBrand.Warehouse.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;
using YourBrand.Warehouse.Application.Common.Interfaces;

namespace YourBrand.Warehouse.Application.Items.Events;

public class ItemCreatedHandler : IDomainEventHandler<ItemCreated>
{
    private readonly IWarehouseContext _context;

    public ItemCreatedHandler(IWarehouseContext context)
    {
        _context = context;
    }

    public async Task Handle(ItemCreated notification, CancellationToken cancellationToken)
    {
        /*
        var person = await _context.Items
            .FirstOrDefaultAsync(i => i.Id == notification.ItemId);

        if(person is not null) 
        {
           
        }
        */
    }
}