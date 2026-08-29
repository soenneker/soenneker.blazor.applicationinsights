using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Blazor.ApplicationInsights.Abstract;

/// <summary>
/// A Blazor interop library that sets up client-side Azure Application Insights
/// </summary>
public interface IApplicationInsightsInterop : IAsyncDisposable
{
    /// <summary>
    /// Calls the JS interop initialization code, and begins the connection to Application Insights. <para/>
    /// Should be called ASAP in the app, typically in App.razor within OnInitializedAsync
    /// </summary>
    /// <param name="connectionString">Connection string used to open the backing service.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>A task that completes when the init operation is complete.</returns>
    ValueTask Init(string connectionString, CancellationToken cancellationToken = default);
}
