using Microsoft.Extensions.DependencyInjection;
using Soenneker.Blazor.ApplicationInsights.Abstract;

namespace Soenneker.Blazor.ApplicationInsights.Extensions;

public static class ApplicationInsightsRegistrar
{
    /// <summary>
    /// Shorthand for <code>services.AddScoped</code>
    /// </summary>
    public static void AddApplicationInsights(this IServiceCollection services)
    {
        services.AddScoped<IApplicationInsightsInterop, ApplicationInsightsInterop>();
    }
}