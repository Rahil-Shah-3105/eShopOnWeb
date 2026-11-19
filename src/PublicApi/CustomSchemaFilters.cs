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
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail", schema, context);

		_logger.LogInformation("Apply(schema={Schema}, context={Context}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail", schema, context);
			_logger.LogInformation("Apply(schema={Schema}, context={Context}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail", schema, context);
        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop)) {
                schema.Properties.Remove(prop);
            }
				_logger.LogInformation("Apply(schema={Schema}, context={Context}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", schema, context);
    }
}
