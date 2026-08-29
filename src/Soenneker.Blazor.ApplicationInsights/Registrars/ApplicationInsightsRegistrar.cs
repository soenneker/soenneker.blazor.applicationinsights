using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Registrars;

namespace Soenneker.Blazor.ApplicationInsights.Registrars;

/// <summary>
/// A Blazor interop library that sets up client-side Azure Application Insights
/// </summary>
public static class ApplicationInsightsRegistrar
{
    /// <summary>
    /// Registers Application Insights Interop with a scoped lifetime.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddApplicationInsightsInteropAsScoped(this IServiceCollection services)
    {
        services.AddModuleImportUtilAsScoped();
        services.TryAddScoped<IApplicationInsightsInterop, ApplicationInsightsInterop>();

        return services;
    }
}
