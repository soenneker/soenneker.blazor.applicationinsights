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
    /// Loads the Application Insights browser SDK and starts client-side telemetry. Call this once after the application has rendered and JavaScript interop is available.
    /// </summary>
    /// <param name="connectionString">The Application Insights connection string.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the init operation is complete.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString"/> is empty or whitespace.</exception>
    ValueTask Init(string connectionString, CancellationToken cancellationToken = default);
}
