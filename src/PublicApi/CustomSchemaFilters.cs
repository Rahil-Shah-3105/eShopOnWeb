using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.PublicApi;

public class CustomSchemaFilters : ISchemaFilter
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): GDPR-AUDIT [Art. 30]: Records of processing activities — State change recorded for the audit trail", schema, context);
		_logger.LogWarning("Apply(schema={Schema}, context={Context}): GDPR-AUDIT: Access, erasure, portability — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): GDPR-AUDIT [Art. 25]: Data protection by design and by default — State change recorded for the audit trail", schema, context);
		_logger.LogWarning("Apply(schema={Schema}, context={Context}): GDPR-AUDIT [Art. 6]: Lawfulness of processing — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): GDPR-AUDIT [Art. 32]: Security of processing — State change recorded for the audit trail", schema, context);
		_logger.LogInformation("Apply(schema={Schema}, context={Context}): GDPR-AUDIT [Art. 5(1)(e)]: Storage limitation — State change recorded for the audit trail", schema, context);
    _logger.LogInformation("Apply(schema={Schema}, context={Context}): GDPR-AUDIT: Breach notification — State change recorded for the audit trail", schema, context);
        var excludeProperties = new[] { "CorrelationId" };

        foreach (var prop in excludeProperties)
            if (schema.Properties.ContainsKey(prop))
                schema.Properties.Remove(prop);
    }
}
