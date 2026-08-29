[![](https://img.shields.io/nuget/v/Soenneker.Blazor.ApplicationInsights.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.ApplicationInsights/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.applicationinsights/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.applicationinsights/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.ApplicationInsights.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.ApplicationInsights/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.applicationinsights/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.applicationinsights/actions/workflows/codeql.yml)

# Soenneker.Blazor.ApplicationInsights

Blazor JavaScript interop for loading Azure Application Insights in the browser and starting client-side telemetry.

## Installation

```bash
dotnet add package Soenneker.Blazor.ApplicationInsights
```

## Registration

```csharp
using Soenneker.Blazor.ApplicationInsights.Registrars;

builder.Services.AddApplicationInsightsInteropAsScoped();
```

## Initialize after the first render

JavaScript interop is required, so initialize from `OnAfterRenderAsync` rather than during component initialization:

```razor
@using Soenneker.Blazor.ApplicationInsights.Abstract
@inject IApplicationInsightsInterop ApplicationInsights
@inject IConfiguration Configuration

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;

        await ApplicationInsights.Init(
            Configuration["ApplicationInsights:ConnectionString"]!);
    }
}
```

Call `Init` once per application session. The interop begins tracking page load, browser exceptions, and `fetch` dependencies.

The browser downloads the Application Insights SDK from `https://js.monitor.azure.com`. If the application uses a Content Security Policy, allow that script origin and the ingestion endpoint named by the connection string. Telemetry is sent from the user's browser, so review captured URLs, headers, and exception data before enabling it in production.

The scoped service owns its imported JavaScript module and releases it when the scope is disposed.
