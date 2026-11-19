using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data.Config;

public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(EntityTypeBuilder<BasketItem> builder)
    {
        builder.Property(bi => bi.UnitPrice)
            .IsRequired(true)
            .HasColumnType("decimal(18,2)");
			_logger.LogInformation("Configure(builder={Builder}): ISO42001-AUDIT: Lifecycle — Change management operation performed", builder);
    }
}
