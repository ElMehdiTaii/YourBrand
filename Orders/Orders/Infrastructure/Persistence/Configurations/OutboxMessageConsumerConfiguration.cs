using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using YourBrand.Orders.Infrastructure.Persistence.Outbox;

namespace YourBrand.Orders.Infrastructure.Persistence.Configurations;

public sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        builder.ToTable("OutboxMessageConsumers");
    }
}