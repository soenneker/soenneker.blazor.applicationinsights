using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Asyncs.Initializers;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ResourceLoader.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public sealed class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IResourceLoader _resourceLoader;

    private readonly AsyncInitializer _scriptInitializer;

    private const string _module = "Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js";

    private readonly CancellationScope _cancellationScope = new();

    public ApplicationInsightsInterop(
        IJSRuntime jSRuntime,
        ILogger<ApplicationInsightsInterop> logger,
        IResourceLoader resourceLoader)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _resourceLoader = resourceLoader;

        _scriptInitializer = new AsyncInitializer(InitializeScript);
    }

    private ValueTask InitializeScript(CancellationToken token)
    {
        return _resourceLoader.ImportModuleAndWaitUntilAvailable(
            _module,
            "AppInsightsInterop",
            100,
            token);
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Application Insights...");

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            await _scriptInitializer.Init(linked);
            await _jsRuntime.InvokeVoidAsync("AppInsightsInterop.init", linked, connectionString);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _resourceLoader.DisposeModule(_module);
        await _scriptInitializer.DisposeAsync();
        await _cancellationScope.DisposeAsync();
    }
}
