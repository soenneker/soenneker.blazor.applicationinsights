[![](https://img.shields.io/nuget/v/Soenneker.Blazor.ApplicationInsights.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.ApplicationInsights/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.applicationinsights/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.applicationinsights/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/Soenneker.Blazor.ApplicationInsights.svg?style=for-the-badge)](https://www.nuget.org/packages/Soenneker.Blazor.ApplicationInsights/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.ApplicationInsights
### A Blazor interop library that sets up client-side Azure [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview?tabs=net).

## Installation

```
dotnet add package Soenneker.Blazor.ApplicationInsights
```

## Usage

1. Insert the script in `wwwroot/index.html` at the bottom of your `<body>` but before the other scripts

```html
<script src="_content/Soenneker.Blazor.ApplicationInsights/applicationinsights.js"></script>
```

2. Register the interop within DI (`Program.cs`)

```csharp
public static async Task Main(string[] args)
{
    ...
    builder.Services.AddApplicationInsights();
}
```

3. Inject `IApplicationInsightsInterop` within your `App.Razor` file


```csharp
@using Soenneker.Blazor.ApplicationInsights.Abstract
@inject IApplicationInsightsInterop AppInsightsInterop
```


4. Initialize the interop in `OnInitializedAsync` within `App.Razor` using your Clarity project key

```csharp
protected override async Task OnInitializedAsync()
{
    await AppInsightsInterop.Init("your-connection-string-here");
    ...
}
```