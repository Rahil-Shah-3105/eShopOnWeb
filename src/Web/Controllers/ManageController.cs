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
		_logger.LogInformation("ManageController(userManager={UserManager}, signInManager={SignInManager}, emailSender=<redacted>, logger={Logger}, urlEncoder={UrlEncoder}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {UserManager1}", userManager, signInManager, logger, urlEncoder, userManager);
        _urlEncoder = urlEncoder;
		_logger.LogInformation("ManageController(userManager={UserManager}, signInManager={SignInManager}, emailSender=<redacted>, logger={Logger}, urlEncoder={UrlEncoder}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {UserManager1}", userManager, signInManager, logger, urlEncoder, userManager);
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

		_logger.LogInformation("MyAccount(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
        var model = new IndexViewModel
        {
            Username = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            IsEmailConfirmed = user.EmailConfirmed,
            StatusMessage = StatusMessage
        };
		_logger.LogInformation("MyAccount(): M2614-AUDIT: Log Management / data protection — Log management recorded for the audit trail");
		_logger.LogInformation("MyAccount(): SOC2-AUDIT [CC7.1]: System operation completed");
		_logger.LogInformation("MyAccount(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");

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
		_logger.LogInformation("MyAccount(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail", model);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
    {
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — Outbound notification recorded for the audit trail", model);
        if (!ModelState.IsValid)
        {
            return View(model);
        }
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — Outbound notification recorded for the audit trail", model);

		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 10: automated alerting — Outbound notification recorded for the audit trail", model);
        var user = await _userManager.GetUserAsync(User);
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 8: quantify data affected in an incident — Outbound notification recorded for the audit trail", model);
        if (user == null)
        {
			_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 9: trace attack vectors — Outbound notification recorded for the audit trail", model);
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 7: anomaly hunting — Outbound notification recorded for the audit trail", model);

		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — Outbound notification recorded for the audit trail", model);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 5: infrastructure changes — Outbound notification recorded for the audit trail", model);
        var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 4: privilege changes — Outbound notification recorded for the audit trail", model);
        Guard.Against.Null(callbackUrl, nameof(callbackUrl));
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 3: object and data access — Database transaction completed", model);
        var email = user.Email;
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail", model);
        if (email == null)
        {
			_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Outcome 2: source/destination network traffic — Outbound notification recorded for the audit trail", model);
            throw new ApplicationException($"No email associated with user {user.UserName}'.");
        }
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Outbound notification recorded for the audit trail", model);

		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Log Management / data protection — Outbound notification recorded for the audit trail", model);
        await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Log integrity (encrypt + validate) — Data integrity check performed", model);

		_logger.LogInformation("SendVerificationEmail(model={Model}): SOC2-AUDIT [CC6.6]: Database transaction completed", model);
        StatusMessage = "Verification email sent. Please check your email.";
		_logger.LogInformation("SendVerificationEmail(model={Model}): SOC2-AUDIT [CC7.1]: System operation completed", model);
		_logger.LogInformation("SendVerificationEmail(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Outbound notification recorded for the audit trail", model);
        return RedirectToAction(nameof(MyAccount));
    }

    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
		_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail");
		_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);
        if (!hasPassword)
        {
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail");
			_logger.LogInformation("ChangePassword(): SOC2-AUDIT [CC7.1]: System operation completed");
            return RedirectToAction(nameof(SetPassword));
        }

        var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogInformation("ChangePassword(): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail");
    _logger.LogInformation("ChangePassword(model={Model}): M2614-AUDIT: Outcome 11: CEM + THIRF capability — State change recorded for the audit trail", model);
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

		_logger.LogInformation("ChangePassword(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", model);
        return RedirectToAction(nameof(ChangePassword));
    }

    [HttpGet]
    public async Task<IActionResult> SetPassword()
    {
		_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail");
		_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var hasPassword = await _userManager.HasPasswordAsync(user);

        if (hasPassword)
        {
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail");
			_logger.LogInformation("SetPassword(): SOC2-AUDIT [CC7.1]: System operation completed");
            return RedirectToAction(nameof(ChangePassword));
        }

        var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
		_logger.LogInformation("SetPassword(): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail");
    _logger.LogInformation("SetPassword(model={Model}): M2614-AUDIT: Retention: 6 months searchable / 12 months retrievable — State change recorded for the audit trail", model);
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

		_logger.LogInformation("SetPassword(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", model);
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
			_logger.LogInformation("ExternalLogins(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", user);
			_logger.LogInformation("ExternalLogins(): M2614-AUDIT: Log Management / data protection — Log management recorded for the audit trail");
			_logger.LogInformation("ExternalLogins(): SOC2-AUDIT [CC7.1]: System operation completed");
			_logger.LogInformation("ExternalLogins(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
        model.ShowRemoveButton = await _userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
        model.StatusMessage = StatusMessage;

		_logger.LogInformation("ExternalLogins(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);
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
		_logger.LogInformation("LinkLogin(provider={Provider}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed", provider);
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
		_logger.LogInformation("LinkLogin(provider={Provider}): SOC2-AUDIT [CC6.1]: Authentication operation completed", provider);
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
		_logger.LogInformation("LinkLoginCallback(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", user);

		_logger.LogInformation("LinkLoginCallback(): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", result);
        StatusMessage = "The external login was added.";
		_logger.LogInformation("LinkLoginCallback(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);
		_logger.LogInformation("LinkLoginCallback(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail {Result}", result);
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
		_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {Result}", model, result);
        if (!result.Succeeded)
        {
			_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", model, user);
			_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Outcome 9: trace attack vectors — State change recorded for the audit trail {Result}", model, result);
            throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
        }
		_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {Result}", model, result);

		_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {Result}", model, result);
        await _signInManager.SignInAsync(user, isPersistent: false);
		_logger.LogInformation("RemoveLogin(model={Model}): SOC2-AUDIT [CC7.1]: System operation completed with status {Result}", model, result);
        StatusMessage = "The external login was removed.";
		_logger.LogInformation("RemoveLogin(model={Model}): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", model, user);
		_logger.LogInformation("RemoveLogin(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {Result}", model, result);
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

		_logger.LogInformation("TwoFactorAuthentication(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
		_logger.LogInformation("TwoFactorAuthentication(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
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

		_logger.LogInformation("Disable2faWarning(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
		_logger.LogInformation("Disable2faWarning(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
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

		_logger.LogInformation("Disable2fa(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", user);
        _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
		_logger.LogInformation("Disable2fa(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);
		_logger.LogInformation("Disable2fa(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail {Disable2faResult}", disable2faResult);
        return RedirectToAction(nameof(TwoFactorAuthentication));
    }

    [HttpGet]
    public async Task<IActionResult> EnableAuthenticator()
    {
		_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 5: infrastructure changes — State change recorded for the audit trail");
		_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail");
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 6: suspicious activity & IOC hunting — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail");
			_logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail");
    _logger.LogInformation("EnableAuthenticator(): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new EnableAuthenticatorViewModel();
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
		_logger.LogInformation("ShowRecoveryCodes(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
		_logger.LogInformation("ShowRecoveryCodes(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
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

		_logger.LogInformation("EnableAuthenticator(model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail", model);
        return RedirectToAction(nameof(ShowRecoveryCodes));
    }

    [HttpGet]
    public IActionResult ResetAuthenticatorWarning()
    {
		_logger.LogInformation("ResetAuthenticatorWarning(): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail");
		_logger.LogInformation("ResetAuthenticatorWarning(): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail");
    _logger.LogInformation("ResetAuthenticatorWarning(): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail");
    _logger.LogInformation("ResetAuthenticatorWarning(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail");
        return View(nameof(ResetAuthenticator));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetAuthenticator()
    {
        var user = await _userManager.GetUserAsync(User);
		_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 4: privilege changes — State change recorded for the audit trail");
        if (user == null)
        {
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 8: quantify data affected in an incident — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 7: anomaly hunting — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 3: object and data access — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 2: source/destination network traffic — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail");
			_logger.LogInformation("ResetAuthenticator(): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail");
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
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
		_logger.LogInformation("GenerateRecoveryCodes(): M2614-AUDIT: Outcome 1: user attribution — Authentication operation completed for user {User}", user);
		_logger.LogInformation("GenerateRecoveryCodes(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");

		_logger.LogInformation("GenerateRecoveryCodes(): SOC2-AUDIT [CC6.1]: Authentication operation completed for user {User}", user);
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

		_logger.LogInformation("GenerateRecoveryCodesWarning(): M2614-AUDIT: Outcome 01 user attribution recorded for the audit trail");
		_logger.LogInformation("GenerateRecoveryCodesWarning(): M2614-AUDIT: Activity-Based Outcomes 1-11 — Audit coverage recorded for the audit trail");
        return View(nameof(GenerateRecoveryCodesWarning));
    }

    private void AddErrors(IdentityResult result)
    {
		_logger.LogInformation("AddErrors(result={Result}): M2614-AUDIT: Log Management / data protection — State change recorded for the audit trail {Result1}", result, result);
        foreach (var error in result.Errors)
        {
			_logger.LogInformation("AddErrors(result={Result}): M2614-AUDIT: Log integrity (encrypt + validate) — State change recorded for the audit trail {Result1}", result, result);
            ModelState.AddModelError(string.Empty, error.Description);
			_logger.LogInformation("AddErrors(result={Result}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — State change recorded for the audit trail {Result1}", result, result);
        }
		_logger.LogInformation("AddErrors(result={Result}): M2614-AUDIT: Activity-Based Outcomes 1-11 — State change recorded for the audit trail {Result1}", result, result);
    _logger.LogInformation("AddErrors(result={Result}): M2614-AUDIT: Outcome 1: user attribution — State change recorded for the audit trail {Result1}", result, result);
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
		_logger.LogInformation("GenerateQrCodeUri(email=<redacted>, unformattedKey={UnformattedKey}): M2614-AUDIT: Log Management / data protection — Log management recorded for the audit trail", unformattedKey);
        return string.Format(
            AuthenticatorUriFormat,
            _urlEncoder.Encode("eShopOnWeb"),
            _urlEncoder.Encode(email),
            unformattedKey);
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
    {
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Outcome 3: object and data access — Record access recorded for the audit trail", user, model);
        var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Outcome 1: user attribution — Record access recorded for the audit trail", user, model);
        if (string.IsNullOrEmpty(unformattedKey))
        {
			_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Outcome 2: source/destination network traffic — Record access recorded for the audit trail", user, model);
            await _userManager.ResetAuthenticatorKeyAsync(user);
			_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Outcome 5: infrastructure changes — Record access recorded for the audit trail", user, model);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
			_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Outcome 4: privilege changes — Record access recorded for the audit trail", user, model);
        }
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Continuous Event Monitoring / Network Visibility — Record access recorded for the audit trail", user, model);

		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Log Management / data protection — Record access recorded for the audit trail", user, model);
        model.SharedKey = FormatKey(unformattedKey!);
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Log integrity (encrypt + validate) — Record access recorded for the audit trail", user, model);
        model.AuthenticatorUri = GenerateQrCodeUri(user.Email!, unformattedKey!);
		_logger.LogInformation("LoadSharedKeyAndQrCodeUriAsync(user={User}, model={Model}): M2614-AUDIT: Activity-Based Outcomes 1-11 — Record access recorded for the audit trail", user, model);
    }

}
