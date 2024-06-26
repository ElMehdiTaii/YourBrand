
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Inventory.Domain;

namespace YourBrand.Inventory.Application.Warehouses.Items.Commands;

public record ReceiveWarehouseItems(string WarehouseId, string Id, int Quantity) : IRequest
{
    public class Handler(IInventoryContext context) : IRequestHandler<ReceiveWarehouseItems>
    {
        public async Task Handle(ReceiveWarehouseItems request, CancellationToken cancellationToken)
        {
            var item = await context.WarehouseItems.FirstOrDefaultAsync(i => i.WarehouseId == request.WarehouseId && i.ItemId == request.Id, cancellationToken);

            if (item is null) throw new Exception();

            item.Receive(request.Quantity);

            await context.SaveChangesAsync(cancellationToken);

        }
    }
}