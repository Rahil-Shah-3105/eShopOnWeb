using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        var excludeProperties = new[] { "CorrelationId" };
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): EUAIACT-AUDIT: Logging coverage — State change recorded for the audit trail", schema, context);

		_logger.LogInformation("Apply(schema={Schema}, context={Context}): EUAIACT-AUDIT: GPAI obligations — State change recorded for the audit trail", schema, context);
			_logger.LogInformation("Apply(schema={Schema}, context={Context}): EUAIACT-AUDIT [Art. 26(6)]: Deployer log retention — State change recorded for the audit trail", schema, context);
        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop)) {
                schema.Properties.Remove(prop);
            }
				_logger.LogInformation("Apply(schema={Schema}, context={Context}): EUAIACT-AUDIT [Art. 50]: Transparency — State change recorded for the audit trail", schema, context);
    }
}
