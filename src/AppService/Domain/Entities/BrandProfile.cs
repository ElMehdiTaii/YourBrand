﻿using YourBrand.Domain.Common;
using YourBrand.Identity;
using YourBrand.Tenancy;

namespace YourBrand.Domain.Entities;

public class BrandProfile : AuditableEntity, ISoftDelete, IHasTenant
{
    protected BrandProfile()
    {

    }

    public BrandProfile(string name, string? description = null)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description;
    }

    public string Id { get; set; } = null!;

    public TenantId TenantId { get; set; } = null!;

    public OrganizationId? OrganizationId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public string? BackgroundColor { get; set; } = null!;

    public string? AppbarBackgroundColor { get; set; } = null!;

    public string? PrimaryColor { get; set; } = null!;

    public string? SecondaryColor { get; set; } = null!;

    public DateTime? Deleted { get; set; }
    public UserId? DeletedById { get; set; }
    public User? DeletedBy { get; set; }
}