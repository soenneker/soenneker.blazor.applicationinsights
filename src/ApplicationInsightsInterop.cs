using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.ValueTask;
using Soenneker.Utils.AsyncSingleton;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private readonly AsyncSingleton<object> _scriptInitializer;

    public ApplicationInsightsInterop(IJSRuntime jSRuntime, ILogger<ApplicationInsightsInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;

        _scriptInitializer = new AsyncSingleton<object>(async objects =>
        {
            var cancellationToken = (CancellationToken) objects[0];

            await _moduleImportUtil.Import("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js", cancellationToken);
            await _moduleImportUtil.WaitUntilLoadedAndAvailable("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js", "AppInsightsInterop", 100, cancellationToken);

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
        return _moduleImportUtil.DisposeModule("Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js");
    }
}