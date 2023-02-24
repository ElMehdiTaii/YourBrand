﻿using MediatR;

using Microsoft.EntityFrameworkCore;

using OrderPriceCalculator;

using YourBrand.Orders.Contracts;
using YourBrand.Orders.Infrastructure.Persistence;

namespace YourBrand.Orders.Application.Orders.Commands;

public record RemoveDiscountFromOrderCommand(int OrderNo, Guid DiscountId) : IRequest
{
    public class RemoveDiscountFromOrderCommandHandler : IRequestHandler<RemoveDiscountFromOrderCommand>
    {
        private readonly ILogger<RemoveDiscountFromOrderCommandHandler> _logger;
        private readonly OrdersContext context;
 
        public RemoveDiscountFromOrderCommandHandler(
            ILogger<RemoveDiscountFromOrderCommandHandler> logger,
            OrdersContext context)
        {
            _logger = logger;
            this.context = context;
        }

        public async Task Handle(RemoveDiscountFromOrderCommand request, CancellationToken cancellationToken)
        {
            var message = request;

            var order = await context.Orders
                .IncludeAll()
                .Where(c => c.OrderNo == message.OrderNo)
                .FirstOrDefaultAsync();

            if (order is null)
            {
                throw new Exception();
            }

            var discount = order.Discounts.FirstOrDefault(x => x.Id == message.DiscountId);

            if (discount is null)
            {
                throw new Exception();
            }

            order.Discounts.Remove(discount);

            context.OrderDiscounts.Remove(discount);

            order.Update();

            await context.SaveChangesAsync();

        }
    }
}