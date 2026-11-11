using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Soenneker.Blazor.ApplicationInsights.Logging;

[ProviderAlias("BrowserApplicationInsights")]
public sealed class ApplicationInsightsLoggerProvider : ILoggerProvider
{
    private IJSObjectReference? _module;

    internal void Connect(IJSObjectReference module) => _module = module;

    internal void Disconnect(IJSObjectReference? module)
    {
        if (ReferenceEquals(_module, module))
            _module = null;
    }

    public ILogger CreateLogger(string categoryName) => new ApplicationInsightsLogger(this, categoryName);

    internal bool IsEnabled(LogLevel level) => _module is not null && level is LogLevel.Error or LogLevel.Critical;

    internal async ValueTask TrackError(string category, EventId eventId, string message, Exception? exception, LogLevel level)
    {
        IJSObjectReference? module = _module;
        if (module is null)
            return;

        try
        {
            object?[] args = [message, exception?.ToString(), exception?.GetType().FullName, level == LogLevel.Critical ? 4 : 3,
                category, eventId.Id, eventId.Name];

            if (module is IJSInProcessObjectReference synchronousModule)
                synchronousModule.InvokeVoid("trackError", args);
            else
                await module.InvokeVoidAsync("trackError", args);
        }
        catch (Exception)
        {
            // Logging must not crash the application or recursively log a telemetry failure.
        }
    }

    public void Dispose() => _module = null;
}
