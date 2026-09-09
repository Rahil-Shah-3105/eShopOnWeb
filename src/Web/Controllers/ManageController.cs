using System.Text;
using System.Text.Encodings.Web;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels.Manage;

namespace Microsoft.eShopWeb.Web.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[Authorize] // Controllers that mainly require Authorization still use Controller/View; other pages use Pages
[Route("[controller]/[action]")]
public class ManageController : Controller
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly IAppLogger<ManageController> _logger;
    private readonly UrlEncoder _urlEncoder;

    private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
    private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

    public ManageController(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      IEmailSender emailSender,
      IAppLogger<ManageController> logger,
      UrlEncoder urlEncoder)
    {
		_logger.LogInformation("ENTERING: ManageController() method");
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> MyAccount()
    {
		_logger.LogInformation("ENTERING: MyAccount() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new IndexViewModel
        {
            Username = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsEmailConfirmed = user.EmailConfirmed,
            StatusMessage = StatusMessage
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MyAccount(IndexViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var email = user.Email;
        if (model.Email != email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
            if (!setEmailResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
            }
        }

        var phoneNumber = user.PhoneNumber;
        if (model.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
            }
        }

        StatusMessage = "Your profile has been updated";
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
    {
		_logger.LogInformation("ENTERING: SendVerificationEmail() method");
        if (!ModelState.IsValid)
        {
			_logger.LogWarning("ENTERING: SendVerificationEmail() method");
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
        Guard.Against.Null(callbackUrl, nameof(callbackUrl));
        var email = user.Email;
        if (email == null)
        {
            throw new ApplicationException($"No email associated with user {user.UserName}'.");
        }

        await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

        StatusMessage = "Verification email sent. Please check your email.";
		_logger.LogWarning("SendVerificationEmail(model)={Model}): GDPR-AUDIT: Outbound notification recorded for the audit trail", model);
		_logger.LogWarning("SendVerificationEmail(model)={Model}): GDPR-AUDIT [Art.44]: Personal data transfer operation recorded", model);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
		_logger.LogInformation("ENTERING: ChangePassword() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
        {
            return RedirectToAction(nameof(SetPassword));
        }
		_logger.LogWarning("ChangePassword()=null): GDPR-AUDIT: Password reset operation performed for user {User}", user);

        var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogWarning("ChangePassword()=null): GDPR-AUDIT: State change recorded for the audit trail");
		_logger.LogInformation("ChangePassword()=null): GDPR-AUDIT [Art.5]: Personal data processing operation recorded {User}", user);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var changePasswordResult = await _userManager
            .ChangePasswordAsync(user, model.OldPassword!, model.NewPassword!);
        if (!changePasswordResult.Succeeded)
        {
            AddErrors(changePasswordResult);
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        _logger.LogInformation("User changed their password successfully.");
        StatusMessage = "Your password has been changed.";

		_logger.LogInformation("ChangePassword(model)={Model}): GDPR-AUDIT [Art.5]: Personal data processing operation recorded {User}", model, user);
        return RedirectToAction(nameof(ChangePassword));
    }

    [HttpGet]
    public async Task<IActionResult> SetPassword()
    {
		_logger.LogInformation("ENTERING: SetPassword() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);

        if (hasPassword)
        {
            return RedirectToAction(nameof(ChangePassword));
        }

		_logger.LogWarning("SetPassword()=null): GDPR-AUDIT: Password reset operation performed for user {User}", user);
        var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogWarning("SetPassword()=null): GDPR-AUDIT: State change recorded for the audit trail");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword!);
        if (!addPasswordResult.Succeeded)
        {
            AddErrors(addPasswordResult);
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        StatusMessage = "Your password has been set.";

		_logger.LogWarning("SetPassword(model)={Model}): GDPR-AUDIT: State change recorded for the audit trail", model);
        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLogins()
    {
		_logger.LogInformation("ENTERING: ExternalLogins() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
        model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();
        model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
        model.StatusMessage = StatusMessage;

		_logger.LogWarning("ExternalLogins()=null): GDPR-AUDIT: Password reset operation performed for user {User}", user);
		_logger.LogWarning("ExternalLogins()=null): GDPR-AUDIT [Art.44]: Personal data transfer operation recorded");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkLogin(string provider)
    {
		_logger.LogInformation("ENTERING: LinkLogin() method");
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Action(nameof(LinkLoginCallback));
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
        return new ChallengeResult(provider, properties);
    }

    [HttpGet]
    public async Task<IActionResult> LinkLoginCallback()
    {
		_logger.LogInformation("ENTERING: LinkLoginCallback() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
        if (info == null)
        {
            throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
        }

        var result = await _userManager.AddLoginAsync(user, info);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        StatusMessage = "The external login was added.";
		_logger.LogWarning("LinkLoginCallback()=null): GDPR-AUDIT [Art.44]: Personal data transfer to third party initiated {Result}", result);
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
    {
		_logger.LogInformation("ENTERING: RemoveLogin() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
		_logger.LogWarning("RemoveLogin(model)={Model}): GDPR-AUDIT: State change recorded for the audit trail {Result}", model, result);
		_logger.LogWarning("RemoveLogin(model)={Model}): GDPR-AUDIT [Art.44]: Personal data transfer to third party initiated {Result}", model, result);
		_logger.LogWarning("RemoveLogin(model)={Model}): GDPR-AUDIT [Art.17]: Data subject right exercised — erasure/portability request processed {User}", model, user);
        StatusMessage = "The external login was removed.";
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
		_logger.LogInformation("ENTERING: TwoFactorAuthentication() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new TwoFactorAuthenticationViewModel
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
            Is2faEnabled = user.TwoFactorEnabled,
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
        };

		_logger.LogInformation("TwoFactorAuthentication()=null): GDPR-AUDIT: Encryption operation completed");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Disable2faWarning()
    {
		_logger.LogInformation("ENTERING: Disable2faWarning() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
        }

		_logger.LogWarning("Disable2faWarning()=null): GDPR-AUDIT: State change recorded for the audit trail");
        return View(nameof(Disable2fa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disable2fa()
    {
		_logger.LogInformation("ENTERING: Disable2fa() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
        if (!disable2faResult.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
        }

        _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
		_logger.LogWarning("Disable2fa()=null): GDPR-AUDIT: State change recorded for the audit trail {Disable2faResult}", disable2faResult);
        return RedirectToAction(nameof(TwoFactorAuthentication));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
		_logger.LogInformation("ENTERING: EnableAuthenticator() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new EnableAuthenticatorViewModel();
        await LoadSharedKeyAndQrCodeUriAsync(user, model);

		_logger.LogWarning("EnableAuthenticator()=null): GDPR-AUDIT: State change recorded for the audit trail");
        return View(model);
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
		_logger.LogInformation("ENTERING: ShowRecoveryCodes() method");
        var recoveryCodes = (string[]?)TempData[RecoveryCodesKey];
        if (recoveryCodes == null)
        {
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
		_logger.LogInformation("ShowRecoveryCodes()=null): GDPR-AUDIT: Encryption operation completed");
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadSharedKeyAndQrCodeUriAsync(user, model);
            return View(model);
        }

        // Strip spaces and hypens
        string verificationCode = model.Code?.Replace(" ", string.Empty).Replace("-", string.Empty) ?? "";

        var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        if (!is2faTokenValid)
        {
            ModelState.AddModelError("Code", "Verification code is invalid.");
            await LoadSharedKeyAndQrCodeUriAsync(user, model);
			_logger.LogInformation("ENTERING: LoadSharedKeyAndQrCodeUriAsync() method");
			_logger.LogWarning("ENTERING: LoadSharedKeyAndQrCodeUriAsync() method");
			_logger.LogWarning("LoadSharedKeyAndQrCodeUriAsync(user,model)={User},{Model}: GDPR-AUDIT: Outbound notification recorded for the audit trail", user, model);
			_logger.LogWarning("LoadSharedKeyAndQrCodeUriAsync(user,model)={User},{Model}: GDPR-AUDIT [Art.44]: Personal data transfer operation recorded", user, model);
			_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user,model)={User},{Model}: GDPR-AUDIT [Art.30]: Personal data processing operation recorded {User1}", user, model, user);
            return View(model);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10) ?? new List<string>();
        TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

		_logger.LogWarning("EnableAuthenticator(model)={Model}): GDPR-AUDIT: State change recorded for the audit trail", model);
        return RedirectToAction(nameof(ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ResetAuthenticatorWarning()
    {
		_logger.LogInformation("ENTERING: ResetAuthenticatorWarning() method");
        return View(nameof(ResetAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetAuthenticator()
    {
		_logger.LogInformation("ENTERING: ResetAuthenticator() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

		_logger.LogWarning("ResetAuthenticator()=null): GDPR-AUDIT: State change recorded for the audit trail");
        return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
		_logger.LogInformation("ENTERING: GenerateRecoveryCodes() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10) ?? new List<string>();
        _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

        var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

		_logger.LogInformation("GenerateRecoveryCodes()=null): GDPR-AUDIT: Encryption operation completed");
        return View(nameof(ShowRecoveryCodes), model);
    }

    [HttpGet]
    public async Task<IActionResult> GenerateRecoveryCodesWarning()
    {
		_logger.LogInformation("ENTERING: GenerateRecoveryCodesWarning() method");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
        }

		_logger.LogInformation("GenerateRecoveryCodesWarning()=null): GDPR-AUDIT: Encryption operation completed");
        return View(nameof(GenerateRecoveryCodesWarning));
    }

    private void AddErrors(IdentityResult result)
    {
		_logger.LogInformation("ENTERING: AddErrors() method");
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
			_logger.LogWarning("AddErrors(result)={Result}): GDPR-AUDIT: State change recorded for the audit trail {Result1}", result, result);
        }
    }

    private string FormatKey(string unformattedKey)
    {
		_logger.LogInformation("ENTERING: FormatKey() method");
        var result = new StringBuilder();
        int currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition));
        }

        return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
		_logger.LogInformation("ENTERING: GenerateQrCodeUri() method");
        return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("eShopOnWeb"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
        var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        model.SharedKey = FormatKey(unformattedKey!);
        model.AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);
    }

}
