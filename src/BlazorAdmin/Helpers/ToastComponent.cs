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
		_logger.LogDebug(">>> Entering OnInitialized()");
        ToastService.OnShow += ShowToast;
        ToastService.OnHide += HideToast;
		_logger.LogDebug("<<< Exiting OnInitialized()");
    }
    private void ShowToast(string message, ToastLevel level)
    {
		_logger.LogDebug(">>> Entering ShowToast(message={Message},level={Level})", message, level);
        BuildToastSettings(level, message);
        IsVisible = true;
		_logger.LogDebug("ShowToast(message={Message},level={Level}): IsVisible → {IsVisible}", message, level, IsVisible);
        StateHasChanged();
		_logger.LogDebug("<<< Exiting ShowToast(message={Message},level={Level})", message, level);
    }
    private void HideToast()
    {
		_logger.LogDebug(">>> Entering HideToast()");
        IsVisible = false;
        StateHasChanged();
		_logger.LogDebug("HideToast(): IsVisible → {IsVisible}", IsVisible);
		_logger.LogDebug("<<< Exiting HideToast()");
    }
    private void BuildToastSettings(ToastLevel level, string message)
    {
		_logger.LogDebug(">>> Entering BuildToastSettings(level={Level},message={Message})", level, message);
        switch (level)
        {
            case ToastLevel.Info:
                BackgroundCssClass = "bg-info";
                IconCssClass = "info";
                Heading = "Info";
				_logger.LogDebug("BuildToastSettings(level={Level},message={Message}): BackgroundCssClass → {BackgroundCssClass}", level, message, BackgroundCssClass);
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
        Message = message;
		_logger.LogDebug("<<< Exiting BuildToastSettings(level={Level},message={Message})", level, message);
    }
    public void Dispose()
    {
		_logger.LogDebug(">>> Entering Dispose()");
        ToastService.OnShow -= ShowToast;
		_logger.LogDebug("<<< Exiting Dispose()");
    }
}
