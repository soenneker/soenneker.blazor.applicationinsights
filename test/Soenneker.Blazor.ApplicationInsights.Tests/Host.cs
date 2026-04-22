using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soenneker.TestHosts.Unit;
using Soenneker.Utils.Test;
using Soenneker.Blazor.ApplicationInsights.Registrars;
using Soenneker.Blazor.MockJsRuntime.Registrars;

namespace Soenneker.Blazor.ApplicationInsights.Tests;

public class Host : UnitTestHost
{
    public override Task InitializeAsync()
    {
        SetupIoC(Services);

        Services.AddMockJsRuntimeAsScoped();

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: false);
        });

        var config = TestUtil.BuildConfig();
        services.AddSingleton(config);

        services.AddApplicationInsightsInteropAsScoped();
    }
}
