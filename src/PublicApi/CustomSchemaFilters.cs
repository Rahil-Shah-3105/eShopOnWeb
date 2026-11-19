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

		_logger.LogInformation("Apply(schema={Schema}, context={Context}): ISO42001-AUDIT: Lifecycle — State change recorded for the audit trail", schema, context);
			_logger.LogInformation("Apply(schema={Schema}, context={Context}): ISO42001-AUDIT [A.7]: Data governance — State change recorded for the audit trail", schema, context);
        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop)) {
                schema.Properties.Remove(prop);
            }
				_logger.LogInformation("Apply(schema={Schema}, context={Context}): ISO42001-AUDIT: Ai governance — State change recorded for the audit trail", schema, context);
    }
}
