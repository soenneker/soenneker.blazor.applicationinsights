using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Blazor.ApplicationInsights.Logging;
using Soenneker.Blazor.Utils.ModuleImport.Abstract;
using Soenneker.Extensions.CancellationTokens;
using Soenneker.Utils.CancellationScopes;

namespace Soenneker.Blazor.ApplicationInsights;

public sealed class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly ApplicationInsightsLoggerProvider _loggerProvider;
    private IJSObjectReference? _moduleReference;
    private readonly ILogger<ApplicationInsightsInterop> _logger;
    private readonly IModuleImportUtil _moduleImportUtil;

    private const string _module = "./_content/Soenneker.Blazor.ApplicationInsights/applicationinsightsinterop.min.js";

    private readonly CancellationScope _cancellationScope = new();

    public ApplicationInsightsInterop(ILogger<ApplicationInsightsInterop> logger, IModuleImportUtil moduleImportUtil,
        ApplicationInsightsLoggerProvider loggerProvider)
    {
        _loggerProvider = loggerProvider;
        _logger = logger;
        _moduleImportUtil = moduleImportUtil;
    }

    public async ValueTask Init(string connectionString, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        _logger.LogDebug("Initializing Application Insights...");

        var linked = _cancellationScope.CancellationToken.Link(cancellationToken, out var source);

        using (source)
        {
            var module = await _moduleImportUtil.GetContentModuleReference(_module, linked);
            await module.InvokeVoidAsync("init", linked, connectionString);
            _moduleReference = module;
            // A singleton logger must never retain a Blazor Server circuit's scoped JS runtime.
            if (OperatingSystem.IsBrowser())
                _loggerProvider.Connect(module);
        }
    }

    public async ValueTask DisposeAsync()
    {
        _loggerProvider.Disconnect(_moduleReference);
        await _moduleImportUtil.DisposeContentModule(_module);
        await _cancellationScope.DisposeAsync();
    }
}
