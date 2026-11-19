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
        if (!ModelState.IsValid)
        {
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
		_logger.LogInformation("SendVerificationEmail(model={Model}): FERPA-AUDIT: Inspect amend — Database transaction completed", model);

		_logger.LogInformation("SendVerificationEmail(model={Model}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Database transaction completed", model);
        StatusMessage = "Verification email sent. Please check your email.";
		_logger.LogInformation("SendVerificationEmail(model={Model}): FERPA-AUDIT [34 CFR 99.37]: Directory information — Outbound notification recorded for the audit trail", model);
		_logger.LogInformation("SendVerificationEmail(model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — Outbound notification recorded for the audit trail", model);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
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
		_logger.LogInformation("ChangePassword(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
		_logger.LogInformation("ChangePassword(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
		_logger.LogInformation("ChangePassword(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("ChangePassword(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");

        var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
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

		_logger.LogInformation("ChangePassword(model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", model);
        return RedirectToAction(nameof(ChangePassword));
    }

    [HttpGet]
    public async Task<IActionResult> SetPassword()
    {
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
		_logger.LogInformation("SetPassword(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
		_logger.LogInformation("SetPassword(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
		_logger.LogInformation("SetPassword(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("SetPassword(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");

        var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
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

		_logger.LogInformation("SetPassword(model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", model);
        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLogins()
    {
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

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkLogin(string provider)
    {
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
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
    {
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

		_logger.LogInformation("RemoveLogin(model={Model}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail {Result}", model, result);
        await _signInManager.SignInAsync(user, isPersistent: false);
		_logger.LogInformation("RemoveLogin(model={Model}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail {Result}", model, result);
        StatusMessage = "The external login was removed.";
		_logger.LogInformation("RemoveLogin(model={Model}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail {Result}", model, result);
		_logger.LogInformation("RemoveLogin(model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail {Result}", model, result);
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
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

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Disable2faWarning()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
        }

        return View(nameof(Disable2fa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disable2fa()
    {
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
        return RedirectToAction(nameof(TwoFactorAuthentication));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new EnableAuthenticatorViewModel();
		_logger.LogInformation("EnableAuthenticator(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
		_logger.LogInformation("EnableAuthenticator(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
		_logger.LogInformation("EnableAuthenticator(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("EnableAuthenticator(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");
        await LoadSharedKeyAndQrCodeUriAsync(user, model);

        return View(model);
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
        var recoveryCodes = (string[]?)TempData[RecoveryCodesKey];
        if (recoveryCodes == null)
        {
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
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
            return View(model);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);
        _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10) ?? new List<string>();
        TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

		_logger.LogInformation("EnableAuthenticator(model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail", model);
        return RedirectToAction(nameof(ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ResetAuthenticatorWarning()
    {
		_logger.LogInformation("ResetAuthenticatorWarning(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("ResetAuthenticatorWarning(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
    _logger.LogInformation("ResetAuthenticatorWarning(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
    _logger.LogInformation("ResetAuthenticatorWarning(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");
        return View(nameof(ResetAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
		_logger.LogInformation("ResetAuthenticator(): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail");
		_logger.LogInformation("ResetAuthenticator(): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail");
		_logger.LogInformation("ResetAuthenticator(): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail");
		_logger.LogInformation("ResetAuthenticator(): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail");
        _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

        return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
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

        return View(nameof(ShowRecoveryCodes), model);
    }

    [HttpGet]
    public async Task<IActionResult> GenerateRecoveryCodesWarning()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
        }

        return View(nameof(GenerateRecoveryCodesWarning));
    }

    private void AddErrors(IdentityResult result)
    {
		_logger.LogInformation("AddErrors(result={Result}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — State change recorded for the audit trail {Result1}", result, result);
        foreach (var error in result.Errors)
        {
			_logger.LogInformation("AddErrors(result={Result}): FERPA-AUDIT [34 CFR 99.37]: Directory information — State change recorded for the audit trail {Result1}", result, result);
            ModelState.AddModelError(string.Empty, error.Description);
			_logger.LogInformation("AddErrors(result={Result}): FERPA-AUDIT [34 CFR 99.30]: Consent — State change recorded for the audit trail {Result1}", result, result);
        }
		_logger.LogInformation("AddErrors(result={Result}): FERPA-AUDIT: Inspect amend — State change recorded for the audit trail {Result1}", result, result);
    }

    private string FormatKey(string unformattedKey)
    {
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
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): FERPA-AUDIT: Inspect amend — Record access recorded for the audit trail", user, model);

		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): FERPA-AUDIT [34 CFR 99.32]: Disclosure log — Record access recorded for the audit trail", user, model);
        model.SharedKey = FormatKey(unformattedKey!);
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): FERPA-AUDIT [34 CFR 99.37]: Directory information — Record access recorded for the audit trail", user, model);
        model.AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): FERPA-AUDIT [34 CFR 99.30]: Consent — Record access recorded for the audit trail", user, model);
    }

}
