using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data.Config;

public class BasketConfiguration : IEntityTypeConfiguration<Basket>
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Configure(EntityTypeBuilder<Basket> builder)
    {
        var navigation = builder.Metadata.FindNavigation(nameof(Basket.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

		_logger.LogInformation("Configure(builder)={Builder}): M2614-AUDIT: Change management operation performed", builder);
        builder.Property(b => b.BuyerId)
            .IsRequired()
            .HasMaxLength(256);
			_logger.LogInformation("Configure(builder)={Builder}): SOC2-AUDIT [CC8.1]: Change management operation performed", builder);
    }
}
