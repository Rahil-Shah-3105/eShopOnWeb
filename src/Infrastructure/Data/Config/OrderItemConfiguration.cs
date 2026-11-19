using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data.Config;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.OwnsOne(i => i.ItemOrdered, io =>
        {
            io.WithOwner();

            io.Property(cio => cio.ProductName)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.Property(oi => oi.UnitPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");
			_logger.LogInformation("Configure(builder={Builder}): ISO42001-AUDIT: Lifecycle — Change management operation performed", builder);
    }
}
