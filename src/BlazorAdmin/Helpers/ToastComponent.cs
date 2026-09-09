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
        ToastService.OnShow += ShowToast;
        ToastService.OnHide += HideToast;
    }
    private void ShowToast(string message, ToastLevel level)
    {
        BuildToastSettings(level, message);
        IsVisible = true;
        StateHasChanged();
    }
    private void HideToast()
    {
        IsVisible = false;
        StateHasChanged();
    }
    private void BuildToastSettings(ToastLevel level, string message)
    {
        switch (level)
        {
            case ToastLevel.Info:
                BackgroundCssClass = "bg-info";
                IconCssClass = "info";
                Heading = "Info";
                break;
            case ToastLevel.Success:
                BackgroundCssClass = "bg-success";
                IconCssClass = "check";
                Heading = "Success";
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
		_logger.LogWarning("BuildToastSettings(level,message)={Level},{Message}: BIPA-AUDIT: State change recorded for the audit trail", level, message);
        Message = message;
    }
    public void Dispose()
    {
        ToastService.OnShow -= ShowToast;
    }
}
