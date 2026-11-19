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

        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
			_logger.LogInformation("Apply(schema={Schema}, context={Context}): NYCLL144-AUDIT: Candidate notice — State change recorded for the audit trail", schema, context);
                schema.Properties.Remove(prop);
				_logger.LogInformation("Apply(schema={Schema}, context={Context}): NYCLL144-AUDIT: Aedt audit — State change recorded for the audit trail", schema, context);
    }
}
