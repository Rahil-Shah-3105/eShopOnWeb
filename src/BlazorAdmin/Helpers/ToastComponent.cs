using System;
using BlazorAdmin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace BlazorAdmin.Helpers;

public class ToastComponent : ComponentBase, IDisposable
{
	private static readonly Microsoft.Extensions.Logging.ILogger _logger = Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;
    [Inject]
    ToastService ToastService
    {
        get;
        set;
    }
    protected string Heading
    {
        get;
        set;
    }
    protected string Message
    {
        get;
        set;
    }
    protected bool IsVisible
    {
        get;
        set;
    }
    protected string BackgroundCssClass
    {
        get;
        set;
    }
    protected string IconCssClass
    {
        get;
        set;
    }
    protected override void OnInitialized()
    {
		_logger.LogError(">>> Entering OnInitialized()");
        ToastService.OnShow += ShowToast;
        ToastService.OnHide += HideToast;
		_logger.LogWarning("<<< Exiting OnInitialized()");
    }
    private void ShowToast(string message, ToastLevel level)
    {
		_logger.LogError(">>> Entering ShowToast(message={Message},level={Level})", message, level);
        BuildToastSettings(level, message);
        IsVisible = true;
		_logger.LogDebug("ShowToast(message={Message},level={Level}): IsVisible → {IsVisible}", message, level, IsVisible);
        StateHasChanged();
		_logger.LogWarning("<<< Exiting ShowToast(message={Message},level={Level})", message, level);
    }
    private void HideToast()
    {
		_logger.LogError(">>> Entering HideToast()");
        IsVisible = false;
		_logger.LogWarning("<<< Exiting HideToast()");
		_logger.LogDebug("HideToast(): IsVisible → {IsVisible}", IsVisible);
        StateHasChanged();
    }
    private void BuildToastSettings(ToastLevel level, string message)
    {
		_logger.LogError(">>> Entering BuildToastSettings(level={Level},message={Message})", level, message);
        switch (level)
        {
            case ToastLevel.Info:
                BackgroundCssClass = "bg-info";
				_logger.LogError("BuildToastSettings(level={Level},message={Message}): BackgroundCssClass → {BackgroundCssClass}", level, message, BackgroundCssClass);
				_logger.LogDebug("BuildToastSettings(level={Level},message={Message}): BackgroundCssClass → {BackgroundCssClass}", level, message, BackgroundCssClass);
                IconCssClass = "info";
                Heading = "Info";
				_logger.LogError("BuildToastSettings(level={Level},message={Message}): IconCssClass → {IconCssClass}", level, message, IconCssClass);
				_logger.LogError("BuildToastSettings(level={Level},message={Message}): Heading → {Heading}", level, message, Heading);
				_logger.LogDebug("BuildToastSettings(level={Level},message={Message}): IconCssClass → {IconCssClass}", level, message, IconCssClass);
                break;
            case ToastLevel.Success:
                BackgroundCssClass = "bg-success";
                IconCssClass = "check";
                Heading = "Success";
				_logger.LogDebug("BuildToastSettings(level={Level},message={Message}): Heading → {Heading}", level, message, Heading);
                break;
            case ToastLevel.Warning:
                BackgroundCssClass = "bg-warning";
                IconCssClass = "exclamation";
                Heading = "Warning";
                break;
            case ToastLevel.Error:
                BackgroundCssClass = "bg-danger";
                IconCssClass = "times";
                Heading = "Error";
                break;
        }
		_logger.LogWarning("<<< Exiting BuildToastSettings(level={Level},message={Message})", level, message);
        Message = message;
    }
    public void Dispose()
    {
		_logger.LogError(">>> Entering Dispose()");
        ToastService.OnShow -= ShowToast;
		_logger.LogWarning("<<< Exiting Dispose()");
    }
}
