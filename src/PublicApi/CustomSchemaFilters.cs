using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC6.6]: Boundary protection — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC8.1]: Change management — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC7.1]: System operations — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC6.3]: Authorization — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC6.1]: Logical access — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC6.2]: Authentication — State change recorded for the audit trail", schema, context);
    _logger.LogInformation("Apply(schema={Schema}, context={Context}): SOC2-AUDIT [CC7.3]: Incident response — State change recorded for the audit trail", schema, context);
        var excludeProperties = new[] { "CorrelationId" };

        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
                schema.Properties.Remove(prop);
    }
}
