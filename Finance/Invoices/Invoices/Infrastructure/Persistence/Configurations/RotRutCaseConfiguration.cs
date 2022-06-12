﻿using YourBrand.Invoices.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YourBrand.Invoices.Infrastructure.Persistence.Configurations;

public class RotRutCaseConfiguration : IEntityTypeConfiguration<RotRutCase>
{
    public void Configure(EntityTypeBuilder<RotRutCase> builder)
    {
        builder.ToTable("RotRutCases");

        builder.OwnsOne(x => x.Rot);
        builder.OwnsOne(x => x.Rut);

        //builder.Ignore(e => e.DomainEvents);
    }
}