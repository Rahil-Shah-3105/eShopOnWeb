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
		_logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): DORA-AUDIT: Resilience testing — Outbound notification recorded for the audit trail", subject, message);
        // TODO: Wire this up to actual email sending logic via SendGrid, local SMTP, etc.
		_logger.LogInformation("SendEmailAsync(email=<redacted>, subject={Subject}, message={Message}): DORA-AUDIT [Art. 45]: Information sharing — Outbound notification recorded for the audit trail", subject, message);
        return Task.CompletedTask;
    }
}
