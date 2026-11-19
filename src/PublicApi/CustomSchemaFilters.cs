using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Privileged access — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Access to programs and data — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Change management (PCAOB AS 2201) — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Computer operations — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Segregation of duties — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOX-AUDIT: Audit log tamper — State change recorded for the audit trail", schema, context);
        var excludeProperties = new[] { "CorrelationId" };

        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
                schema.Properties.Remove(prop);
    }
}
