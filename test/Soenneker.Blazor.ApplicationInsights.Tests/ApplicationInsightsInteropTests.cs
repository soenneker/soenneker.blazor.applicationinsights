using System;
using System.Threading.Tasks;
using AwesomeAssertions;
using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.ApplicationInsights.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class ApplicationInsightsInteropTests : HostedUnitTest
{
    private readonly IApplicationInsightsInterop _util;

    public ApplicationInsightsInteropTests(Host host) : base(host)
    {
        _util = Resolve<IApplicationInsightsInterop>(true);
    }

    [Test]
    public async Task Init_rejects_a_blank_connection_string()
    {
        Func<Task> act = async () => await _util.Init("   ");

        await act.Should().ThrowAsync<ArgumentException>();
    }
}
