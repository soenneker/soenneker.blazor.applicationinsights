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
    ValueTask Init(string connectionString, CancellationToken cancellationToken = default);
}