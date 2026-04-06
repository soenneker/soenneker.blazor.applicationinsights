using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public sealed class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private const string _module = "/_content/Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.js";

    private readonly CancellationScope _cancellationScope = new();

    public ApplicationInsightsInterop(ILogger<ApplicationInsightsInterop> logger, IModuleImportUtil moduleImportUtil)
    {
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Initializing Application Insights...");

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            var module = await _moduleImportUtil.GetContentModuleReference(_module, linked);
            await module.InvokeVoidAsync("init", linked, connectionString);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await _moduleImportUtil.DisposeContentModule(_module);
        await _cancellationScope.DisposeAsync();
    }
}