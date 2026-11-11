using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.ApplicationInsights.Abstract;

/// <summary>
/// Initializes client-side Azure Application Insights in a Blazor application.
/// </summary>
public interface IApplicationInsightsInterop : IAsyncDisposable
{
    /// <summary>
    /// Loads the Application Insights browser SDK, captures JavaScript errors and unhandled promise rejections,
    /// and connects Error/Critical ILogger entries to exception telemetry in Blazor WebAssembly.
    /// Call once when JavaScript interop is available. Logs before initialization are not captured.
    /// Blazor Server logs are not forwarded by the browser logger provider.
    /// </summary>
    /// <param name="connectionString">The Application Insights connection string.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the init operation is complete.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is empty or whitespace.</exception>
    ValueTask Init(string connectionString, CancellationToken cancellationToken = default);
}
