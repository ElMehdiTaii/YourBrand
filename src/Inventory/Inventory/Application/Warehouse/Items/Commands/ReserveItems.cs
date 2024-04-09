
using MediatR;

using Microsoft.EntityFrameworkCore;

using YourBrand.Inventory.Domain;

namespace YourBrand.Inventory.Application.Warehouses.Items.Commands;

public record ReserveWarehouseItems(string WarehouseId, string Id, int Quantity) : IRequest
{
    public class Handler(IInventoryContext context) : IRequestHandler<ReserveWarehouseItems>
    {
        public async Task Handle(ReserveWarehouseItems request, CancellationToken cancellationToken)
        {
            var item = await context.WarehouseItems.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item is null) throw new Exception();

            item.Reserve(request.Quantity);

            await context.SaveChangesAsync(cancellationToken);

        }
    }
}