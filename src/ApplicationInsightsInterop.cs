using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public sealed class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public ApplicationInsightsInterop(IJSRuntime jSRuntime, ILogger<ApplicationInsightsInterop> logger, IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncSingleton<object>(async (token, _) =>
        {
            await _resourceLoader.ImportModuleAndWaitUntilAvailable("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js", "AppInsightsInterop", 100, token).NoSync();

            return new object();
        });
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Application Insights...");

        await _scriptInitializer.Get(cancellationToken).NoSync();

        await _jsRuntime.InvokeVoidAsync("AppInsightsInterop.init", cancellationToken, connectionString);
    }

    public ValueTask DisposeAsync()
    {
        return _resourceLoader.DisposeModule("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js");
    }
}