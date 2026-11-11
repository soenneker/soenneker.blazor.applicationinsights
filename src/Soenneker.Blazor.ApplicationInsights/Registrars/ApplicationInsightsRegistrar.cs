using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Soenneker.Blazor.ApplicationInsights.Logging;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Registrars;

namespace Soenneker.Blazor.ApplicationInsights.Registrars;

/// <summary>
/// A Blazor interop library that sets up client-side Azure Application Insights
/// </summary>
public static class ApplicationInsightsRegistrar
{
    /// <summary>
    /// Registers scoped Application Insights interop and a browser logger provider. After Init, Error and Critical
    /// ILogger entries are sent to the browser SDK, including Blazor rendering and event-handler exceptions.
    /// Register after clearing logging providers.
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddApplicationInsightsInteropAsScoped(this IServiceCollection services)
    {
        services.AddModuleImportUtilAsScoped();
        services.TryAddSingleton<ApplicationInsightsLoggerProvider>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ApplicationInsightsLoggerProvider>(
            provider => provider.GetRequiredService<ApplicationInsightsLoggerProvider>()));
        services.TryAddScoped<IApplicationInsightsInterop, ApplicationInsightsInterop>();

        return services;
    }
}
