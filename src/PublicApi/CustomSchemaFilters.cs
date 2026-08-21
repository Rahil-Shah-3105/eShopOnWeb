using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		_logger.LogDebug(">>> Entering Apply(schema={Schema},context={Context})", schema, context);
        var excludeProperties = new[] { "CorrelationId" };

		_logger.LogDebug("Apply(schema={Schema},context={Context}): excludeProperties → {ExcludeProperties}", schema, context, excludeProperties);
        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
                schema.Properties.Remove(prop);
				_logger.LogDebug("<<< Exiting Apply(schema={Schema},context={Context})", schema, context);
    }
}
