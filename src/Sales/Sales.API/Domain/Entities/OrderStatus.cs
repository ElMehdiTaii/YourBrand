﻿using YourBrand.Sales.Domain.ValueObjects;

using YourBrand.Tenancy;

namespace YourBrand.Sales.Domain.Entities;

public class OrderStatus : Entity<int>, IAuditable, IHasTenant
{
    protected OrderStatus()
    {
    }

    public OrderStatus(string name, string handle, string? description)
    {
        Name = name;
        Handle = handle;
        Description = description;
    }

    public TenantId TenantId { get; set; }

    public string Name { get; set; } = null!;

    public string Handle { get; set; } = null!;

    public string? Description { get; set; }

    public User? CreatedBy { get; set; }

    public UserId? CreatedById { get; set; }

    public DateTimeOffset Created { get; set; }

    public User? LastModifiedBy { get; set; }

    public UserId? LastModifiedById { get; set; }

    public DateTimeOffset? LastModified { get; set; }
}