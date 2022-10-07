﻿using MediatR;

namespace YourBrand.Accounting.Domain.Common;

public abstract class DomainEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
}