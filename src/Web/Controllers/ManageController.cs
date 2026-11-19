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
		_logger.LogDebug("ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder}): _userManager → {_userManager}", userManager, signInManager, logger, urlEncoder, _userManager);
        _signInManager = signInManager;
		_logger.LogDebug("ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder}): _signInManager → {_signInManager}", userManager, signInManager, logger, urlEncoder, _signInManager);
        _emailSender = emailSender;
		_logger.LogDebug("ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder}): _emailSender → <redacted>", userManager, signInManager, logger, urlEncoder);
        _logger = logger;
		_logger.LogError(">>> Entering ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder})", userManager, signInManager, logger, urlEncoder);
        _urlEncoder = urlEncoder;
		_logger.LogDebug("ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder}): _urlEncoder → {_urlEncoder}", userManager, signInManager, logger, urlEncoder, _urlEncoder);
		_logger.LogWarning("<<< Exiting ManageController(userManager={UserManager},signInManager={SignInManager},emailSender=<redacted>,logger={Logger},urlEncoder={UrlEncoder})", userManager, signInManager, logger, urlEncoder);
    }

    [TempData]
    public string? StatusMessage { get; set; }

    [HttpGet]
    public async Task<IActionResult> MyAccount()
    {
		_logger.LogError(">>> Entering MyAccount()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("MyAccount(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from MyAccount()");
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

		_logger.LogWarning("<<< Exiting MyAccount()");
		_logger.LogWarning("<<< Exiting MyAccount(model={Model})", model);
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
		_logger.LogDebug("MyAccount(model={Model}): StatusMessage → {StatusMessage}", model, StatusMessage);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
    {
		_logger.LogError(">>> Entering SendVerificationEmail(model={Model})", model);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("SendVerificationEmail(model={Model}): user → {User}", model, user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from SendVerificationEmail(model={Model})", model);
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		_logger.LogError("SendVerificationEmail(model={Model}): code → {Code}", model, code);
        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
		_logger.LogError("SendVerificationEmail(model={Model}): callbackUrl → {CallbackUrl}", model, callbackUrl);
        Guard.Against.Null(callbackUrl, nameof(callbackUrl));
        var email = user.Email;
		_logger.LogError("SendVerificationEmail(model={Model}): email → <redacted>", model);
        if (email == null)
        {
            throw new ApplicationException($"No email associated with user {user.UserName}'.");
        }

        await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

        StatusMessage = "Verification email sent. Please check your email.";
		_logger.LogError("SendVerificationEmail(model={Model}): StatusMessage → {StatusMessage}", model, StatusMessage);
		_logger.LogDebug("SendVerificationEmail(model={Model}): StatusMessage → {StatusMessage}", model, StatusMessage);
		_logger.LogWarning("<<< Exiting SendVerificationEmail(model={Model})", model);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
		_logger.LogError(">>> Entering ChangePassword()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("ChangePassword(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from ChangePassword()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
		_logger.LogError("ChangePassword(): hasPassword → <redacted>");
        if (!hasPassword)
        {
            return RedirectToAction(nameof(SetPassword));
        }

        var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogWarning("<<< Exiting ChangePassword()");
		_logger.LogWarning("<<< Exiting ChangePassword(model={Model})", model);
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

        return RedirectToAction(nameof(ChangePassword));
    }

    [HttpGet]
    public async Task<IActionResult> SetPassword()
    {
		_logger.LogError(">>> Entering SetPassword()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("SetPassword(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from SetPassword()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
		_logger.LogError("SetPassword(): hasPassword → <redacted>");

        if (hasPassword)
        {
            return RedirectToAction(nameof(ChangePassword));
        }

        var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogWarning("<<< Exiting SetPassword()");
		_logger.LogWarning("<<< Exiting SetPassword(model={Model})", model);
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

        return RedirectToAction(nameof(SetPassword));
    }

    [HttpGet]
    public async Task<IActionResult> ExternalLogins()
    {
		_logger.LogError(">>> Entering ExternalLogins()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("ExternalLogins(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from ExternalLogins()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new ExternalLoginsViewModel { CurrentLogins = await _userManager.GetLoginsAsync(user) };
		_logger.LogError("ExternalLogins(): model → {Model}", model);
        model.OtherLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
            .ToList();
        model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
        model.StatusMessage = StatusMessage;
		_logger.LogDebug("ExternalLogins(): model.StatusMessage → {Arg0}", model.StatusMessage);

		_logger.LogWarning("<<< Exiting ExternalLogins()");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LinkLogin(string provider)
    {
		_logger.LogError(">>> Entering LinkLogin(provider={Provider})", provider);
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Action(nameof(LinkLoginCallback));
		_logger.LogError("LinkLogin(provider={Provider}): redirectUrl → {RedirectUrl}", provider, redirectUrl);
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
		_logger.LogError("LinkLogin(provider={Provider}): properties → {Properties}", provider, properties);
		_logger.LogWarning("<<< Exiting LinkLogin(provider={Provider})", provider);
        return new ChallengeResult(provider, properties);
    }

    [HttpGet]
    public async Task<IActionResult> LinkLoginCallback()
    {
		_logger.LogError(">>> Entering LinkLoginCallback()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("LinkLoginCallback(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from LinkLoginCallback()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync(user.Id);
		_logger.LogError("LinkLoginCallback(): info → {Info}", info);
        if (info == null)
        {
            throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
        }

        var result = await _userManager.AddLoginAsync(user, info);
		_logger.LogError("LinkLoginCallback(): result → {Result}", result);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        StatusMessage = "The external login was added.";
		_logger.LogError("LinkLoginCallback(): StatusMessage → {StatusMessage}", StatusMessage);
		_logger.LogDebug("LinkLoginCallback(): StatusMessage → {StatusMessage}", StatusMessage);
		_logger.LogWarning("<<< Exiting LinkLoginCallback()");
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
    {
		_logger.LogError(">>> Entering RemoveLogin(model={Model})", model);
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("RemoveLogin(model={Model}): user → {User}", model, user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from RemoveLogin(model={Model})", model);
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _userManager.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
		_logger.LogError("RemoveLogin(model={Model}): result → {Result}", model, result);
        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        StatusMessage = "The external login was removed.";
		_logger.LogError("RemoveLogin(model={Model}): StatusMessage → {StatusMessage}", model, StatusMessage);
		_logger.LogDebug("RemoveLogin(model={Model}): StatusMessage → {StatusMessage}", model, StatusMessage);
		_logger.LogWarning("<<< Exiting RemoveLogin(model={Model})", model);
        return RedirectToAction(nameof(ExternalLogins));
    }

    [HttpGet]
    public async Task<IActionResult> TwoFactorAuthentication()
    {
		_logger.LogError(">>> Entering TwoFactorAuthentication()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("TwoFactorAuthentication(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from TwoFactorAuthentication()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new TwoFactorAuthenticationViewModel
        {
            HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
            Is2faEnabled = user.TwoFactorEnabled,
            RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
        };

		_logger.LogWarning("<<< Exiting TwoFactorAuthentication()");
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Disable2faWarning()
    {
		_logger.LogError(">>> Entering Disable2faWarning()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("Disable2faWarning(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from Disable2faWarning()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
        }

		_logger.LogWarning("<<< Exiting Disable2faWarning()");
        return View(nameof(Disable2fa));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Disable2fa()
    {
		_logger.LogError(">>> Entering Disable2fa()");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from Disable2fa()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
		_logger.LogError("Disable2fa(): disable2faResult → {Disable2faResult}", disable2faResult);
        if (!disable2faResult.Succeeded)
        {
            throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
        }

        _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
		_logger.LogWarning("<<< Exiting Disable2fa()");
        return RedirectToAction(nameof(TwoFactorAuthentication));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
		_logger.LogError(">>> Entering EnableAuthenticator()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("EnableAuthenticator(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from EnableAuthenticator()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new EnableAuthenticatorViewModel();
		_logger.LogError("EnableAuthenticator(): model → {Model}", model);
        await LoadSharedKeyAndQrCodeUriAsync(user, model);

		_logger.LogWarning("<<< Exiting EnableAuthenticator()");
		_logger.LogWarning("<<< Exiting EnableAuthenticator(model={Model})", model);
        return View(model);
    }

    [HttpGet]
    public IActionResult ShowRecoveryCodes()
    {
		_logger.LogError(">>> Entering ShowRecoveryCodes()");
        var recoveryCodes = (string[]?)TempData[RecoveryCodesKey];
		_logger.LogError("ShowRecoveryCodes(): recoveryCodes → {RecoveryCodes}", recoveryCodes);
        if (recoveryCodes == null)
        {
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
		_logger.LogWarning("<<< Exiting ShowRecoveryCodes()");
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

        return RedirectToAction(nameof(ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ResetAuthenticatorWarning()
    {
		_logger.LogError(">>> Entering ResetAuthenticatorWarning()");
        _logger.LogWarning("<<< Exiting ResetAuthenticatorWarning()");
        return View(nameof(ResetAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetAuthenticator()
    {
		_logger.LogError(">>> Entering ResetAuthenticator()");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from ResetAuthenticator()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

		_logger.LogWarning("<<< Exiting ResetAuthenticator()");
        return RedirectToAction(nameof(EnableAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GenerateRecoveryCodes()
    {
		_logger.LogError(">>> Entering GenerateRecoveryCodes()");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from GenerateRecoveryCodes()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
        }

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10) ?? new List<string>();
		_logger.LogError("GenerateRecoveryCodes(): recoveryCodes → {RecoveryCodes}", recoveryCodes);
        _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

        var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };
		_logger.LogError("GenerateRecoveryCodes(): model → {Model}", model);

		_logger.LogWarning("<<< Exiting GenerateRecoveryCodes()");
        return View(nameof(ShowRecoveryCodes), model);
    }

    [HttpGet]
    public async Task<IActionResult> GenerateRecoveryCodesWarning()
    {
		_logger.LogError(">>> Entering GenerateRecoveryCodesWarning()");
        var user = await _userManager.GetUserAsync(User);
		_logger.LogError("GenerateRecoveryCodesWarning(): user → {User}", user);
        if (user == null)
        {
			_logger.LogError("Throwing ApplicationException from GenerateRecoveryCodesWarning()");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!user.TwoFactorEnabled)
        {
            throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
        }

		_logger.LogWarning("<<< Exiting GenerateRecoveryCodesWarning()");
        return View(nameof(GenerateRecoveryCodesWarning));
    }

    private void AddErrors(IdentityResult result)
    {
		_logger.LogError(">>> Entering AddErrors(result={Result})", result);
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
		_logger.LogWarning("<<< Exiting AddErrors(result={Result})", result);
    }

    private string FormatKey(string unformattedKey)
    {
		_logger.LogError(">>> Entering FormatKey(unformattedKey={UnformattedKey})", unformattedKey);
        var result = new StringBuilder();
        int currentPosition = 0;
		_logger.LogError("FormatKey(unformattedKey={UnformattedKey}): result → {Result}", unformattedKey, result);
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
            currentPosition += 4;
        }
        if (currentPosition < unformattedKey.Length)
        {
            result.Append(unformattedKey.Substring(currentPosition));
        }

		_logger.LogWarning("<<< Exiting FormatKey(unformattedKey={UnformattedKey})", unformattedKey);
        return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
		_logger.LogError(">>> Entering GenerateQrCodeUri(email=<redacted>,unformattedKey={UnformattedKey})", unformattedKey);
        _logger.LogWarning("<<< Exiting GenerateQrCodeUri(email=<redacted>,unformattedKey={UnformattedKey})", unformattedKey);
        return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("eShopOnWeb"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
		_logger.LogError(">>> Entering LoadSharedKeyAndQrCodeUriAsync(user={User},model={Model})", user, model);
        var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
		_logger.LogError("LoadSharedKeyAndQrCodeUriAsync(user={User},model={Model}): unformattedKey → {UnformattedKey}", user, model, unformattedKey);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        model.SharedKey = FormatKey(unformattedKey!);
        model.AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);
		_logger.LogWarning("<<< Exiting LoadSharedKeyAndQrCodeUriAsync(user={User},model={Model})", user, model);
    }

}
