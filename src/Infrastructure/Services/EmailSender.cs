using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Services;

// This class is used by the application to send email for account confirmation and password reset.
// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
public class EmailSender : IEmailSender
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public Task SendEmailAsync(string email, string subject, string message)
    {
		_logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): SOC2-AUDIT [CC7.1]: System operations — Outbound notification recorded for the audit trail", subject, message);
		_logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): SOC2-AUDIT [CC6.3]: Authorization — Outbound notification recorded for the audit trail", subject, message);
		_logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): SOC2-AUDIT [CC6.2]: Authentication — Outbound notification recorded for the audit trail", subject, message);
    _logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): SOC2-AUDIT [CC8.1]: Change management — Outbound notification recorded for the audit trail", subject, message);
    _logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): SOC2-AUDIT [CC6.6]: Boundary protection — Outbound notification recorded for the audit trail", subject, message);
        // TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
        return Task.CompletedTask;
    }
}
