using System;
using Microsoft.Extensions.Logging;

namespace Soenneker.Blazor.ApplicationInsights.Logging;

internal sealed class ApplicationInsightsLogger(ApplicationInsightsLoggerProvider provider, string category) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => provider.IsEnabled(logLevel);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (IsEnabled(logLevel))
            _ = provider.TrackError(category, eventId, formatter(state, exception), exception, logLevel);
    }
}
