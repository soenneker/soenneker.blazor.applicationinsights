using System.Threading.Tasks;

namespace Soenneker.Blazor.ApplicationInsights.Abstract;

public interface IApplicationInsightsInterop 
{
    /// <summary>
    /// Calls the JS interop initialization code, and begins the connection to Application Insights. <para/>
    /// Should be called ASAP in the app, typically in App.razor within OnInitializedAsync
    /// </summary>
    ValueTask Init(string key);
}