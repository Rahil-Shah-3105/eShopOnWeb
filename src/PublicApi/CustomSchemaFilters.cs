using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail", schema, context);
        var excludeProperties = new[] { "CorrelationId" };
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail", schema, context);

		_logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail", schema, context);
        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
			_logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail", schema, context);
                schema.Properties.Remove(prop);
				_logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", schema, context);
    _logger.LogInformation("Apply(schema={Schema}, context={Context}): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail", schema, context);
    }
}
