﻿using FluentValidation;

using MediatR;

using YourBrand.Sales.Domain.Events;
using YourBrand.Sales.Features.OrderManagement.Repositories;

namespace YourBrand.Sales.Features.OrderManagement.Orders.Commands;

public sealed record DeleteOrder(string Id) : IRequest<Result>
{
    public sealed class Validator : AbstractValidator<DeleteOrder>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }

    public sealed class Handler(IOrderRepository orderRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrder, Result>
    {
        public async Task<Result> Handle(DeleteOrder request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken);

            if (order is null)
            {
                return Errors.Orders.OrderNotFound;
            }

            orderRepository.Remove(order);

            order.AddDomainEvent(new OrderDeleted(order.OrderNo));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Results.Success;
        }
    }
}