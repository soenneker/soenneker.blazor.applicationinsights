using System;
using AwesomeAssertions;
using Microsoft.Extensions.Logging;
using Soenneker.Blazor.ApplicationInsights.Logging;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.ApplicationInsights.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ApplicationInsightsLoggerTests(Host host) : HostedUnitTest(host)
{
    [Test]
    public void Registered_provider_forwards_Blazor_errors_and_survives_telemetry_failure()
    {
        var provider = Resolve<ApplicationInsightsLoggerProvider>();
        var logger = Resolve<ILoggerFactory>().CreateLogger("Microsoft.AspNetCore.Components.WebAssembly.Rendering.WebAssemblyRenderer");
        using var module = new RecordingJsModule();
        logger.LogError("Before initialization");
        module.Calls.Should().BeEmpty();
        provider.Connect(module);
        try
        {
            logger.LogWarning("Warning");
            module.Calls.Should().BeEmpty();
            var exception = new InvalidOperationException("Render failed");
            logger.LogError(new EventId(100, "RenderFailure"), exception, "Unhandled exception rendering {Component}", "TestComponent");
            module.Calls.Should().ContainSingle();
            object?[] args = module.Calls[0]!;
            args[0].Should().Be("Unhandled exception rendering TestComponent");
            args[1].Should().Be(exception.ToString());
            args[2].Should().Be(typeof(InvalidOperationException).FullName);
            args[3].Should().Be(3);
            args[4].Should().Be("Microsoft.AspNetCore.Components.WebAssembly.Rendering.WebAssemblyRenderer");
            args[5].Should().Be(100);
            args[6].Should().Be("RenderFailure");

            logger.LogCritical("Fatal error without exception");
            module.Calls.Should().HaveCount(2);
            module.Calls[1]![3].Should().Be(4);
            module.ThrowOnInvoke = true;
            Action log = () => logger.LogError("Telemetry failure must not escape");
            log.Should().NotThrow();
        }
        finally
        {
            provider.Disconnect(module);
        }
        logger.LogError("After disposal");
        module.Calls.Should().HaveCount(2);
    }
}
