﻿using MediatR;

namespace YourBrand.HumanResources.Domain.Common;

public abstract class DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}