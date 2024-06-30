using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.ApplicationInsights.Registrars;

/// <summary>
/// A Blazor interop library that sets up client-side Azure Application Insights
/// </summary>
public static class ApplicationInsightsRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.TryAddSingleton</code>
    /// </summary>
    public static void AddApplicationInsights(this IServiceCollection services)
    {
        services.AddResourceLoader();
        services.TryAddSingleton<IApplicationInsightsInterop, ApplicationInsightsInterop>();
    }
}