using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Soenneker.Blazor.ApplicationInsights.Abstract;

namespace Soenneker.Blazor.ApplicationInsights;

///<inheritdoc cref="IApplicationInsightsInterop"/>
public class ApplicationInsightsInterop : IApplicationInsightsInterop
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ILogger<ApplicationInsightsInterop> _logger;

    public ApplicationInsightsInterop(IJSRuntime jSRuntime, ILogger<ApplicationInsightsInterop> logger)
    {
        _jsRuntime = jSRuntime;
        _logger = logger;
    }

    public async ValueTask Init(string connectionString)
    {
        _logger.LogDebug("Initializing Application Insights...");

        await _jsRuntime.InvokeVoidAsync("initClarity", connectionString);
    }
}