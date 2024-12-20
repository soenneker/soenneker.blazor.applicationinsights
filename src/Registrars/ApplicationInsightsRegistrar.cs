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
    public static void AddApplicationInsightsInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoader();
        services.TryAddScoped<IApplicationInsightsInterop, ApplicationInsightsInterop>();
    }
}