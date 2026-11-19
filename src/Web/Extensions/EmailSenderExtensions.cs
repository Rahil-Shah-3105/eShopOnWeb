using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Web.Services;

public static class EmailSenderExtensions
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
    {
		_logger.LogInformation("SendEmailConfirmationAsync(emailSender=<redacted>, email=<redacted>, link={Link}): COAI-AUDIT: Consumer notice — Outbound notification recorded for the audit trail", link);
        return emailSender.SendEmailAsync(email, "Confirm your email",
            $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
    }
}
