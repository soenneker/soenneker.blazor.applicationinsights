using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Asyncs.Initializers;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public sealed class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer _scriptInitializer;

    public ApplicationInsightsInterop(IJSRuntime jSRuntime, ILogger<ApplicationInsightsInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncInitializer(async token =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js", "AppInsightsInterop", 100, token);
        });
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Application Insights...");

        await _scriptInitializer.Init(cancellationToken);

        await _jsRuntime.InvokeVoidAsync("AppInsightsInterop.init", cancellationToken, connectionString);
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js");
        await _scriptInitializer.DisposeAsync();
    }
}
