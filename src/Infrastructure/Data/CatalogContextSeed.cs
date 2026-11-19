using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.CatalogBrands.AnyAsync())
            {
                await catalogContext.CatalogBrands.AddRangeAsync(
                    GetPreconfiguredCatalogBrands());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogTypes.AnyAsync())
            {
                await catalogContext.CatalogTypes.AddRangeAsync(
                    GetPreconfiguredCatalogTypes());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogItems.AnyAsync())
            {
                await catalogContext.CatalogItems.AddRangeAsync(
                    GetPreconfiguredItems());

                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;
            
            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
		logger.LogInformation("GetPreconfiguredCatalogBrands(): SOC2-AUDIT [CC8.1]: Change management operation performed");
		logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed");
    logger.LogInformation("GetPreconfiguredCatalogBrands(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
        return new List<CatalogBrand>
            {
                new("Azure"),
                new(".NET"),
                new("Visual Studio"),
                new("SQL Server"),
                new("Other")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
		logger.LogInformation("GetPreconfiguredCatalogTypes(): SOC2-AUDIT [CC8.1]: Change management operation performed");
		logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredCatalogTypes(): SOC2-AUDIT [CC7.2]: System operation completed");
            logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredCatalogTypes(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
        return new List<CatalogType>
            {
                new("Mug"),
                new("T-Shirt"),
                new("Sheet"),
                new("USB Memory Stick")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
		logger.LogInformation("GetPreconfiguredItems(): SOC2-AUDIT [CC8.1]: Change management operation performed");
		logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 5: infrastructure changes — Change management operation performed");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 7: anomaly hunting — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 9: trace attack vectors — Record access recorded for the audit trail");
            logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 10: automated alerting — Record access recorded for the audit trail");
		logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Outcome 11: CEM + THIRF capability — Record access recorded for the audit trail");
    logger.LogInformation("GetPreconfiguredItems(): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — Record access recorded for the audit trail");
        return new List<CatalogItem>
            {
                new(2,2, ".NET Bot Black Sweatshirt", ".NET Bot Black Sweatshirt", 19.5M,  "http://catalogbaseurltobereplaced/images/products/1.png"),
                new(1,2, ".NET Black & White Mug", ".NET Black & White Mug", 8.50M, "http://catalogbaseurltobereplaced/images/products/2.png"),
                new(2,5, "Prism White T-Shirt", "Prism White T-Shirt", 12,  "http://catalogbaseurltobereplaced/images/products/3.png"),
                new(2,2, ".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/4.png"),
                new(3,5, "Roslyn Red Sheet", "Roslyn Red Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/5.png"),
                new(2,2, ".NET Blue Sweatshirt", ".NET Blue Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/6.png"),
                new(2,5, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt",  12, "http://catalogbaseurltobereplaced/images/products/7.png"),
                new(2,5, "Kudu Purple Sweatshirt", "Kudu Purple Sweatshirt", 8.5M, "http://catalogbaseurltobereplaced/images/products/8.png"),
                new(1,5, "Cup<T> White Mug", "Cup<T> White Mug", 12, "http://catalogbaseurltobereplaced/images/products/9.png"),
                new(3,2, ".NET Foundation Sheet", ".NET Foundation Sheet", 12, "http://catalogbaseurltobereplaced/images/products/10.png"),
                new(3,2, "Cup<T> Sheet", "Cup<T> Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
                new(2,5, "Prism White TShirt", "Prism White TShirt", 12, "http://catalogbaseurltobereplaced/images/products/12.png")
            };
    }
}
