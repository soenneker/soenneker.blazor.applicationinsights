using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private bool _initialized;

    public ApplicationInsightsInterop(IJSRuntime jSRuntime, ILogger<ApplicationInsightsInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    private async ValueTask EnsureInitialization(CancellationToken cancellationToken = default)
    {
        if (_initialized)
            return;

        _initialized = true;

        await _moduleImportUtil.Import("Soenneker.Blazor.ApplicationInsights/js/applicationinsights.js", cancellationToken);
        await _moduleImportUtil.WaitUntilLoaded("Soenneker.Blazor.ApplicationInsights/js/applicationinsights.js", cancellationToken);
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Application Insights...");

        await EnsureInitialization(cancellationToken);

        await _jsRuntime.InvokeVoidAsync("AppInsightsInterop.init", cancellationToken, connectionString);
    }
}