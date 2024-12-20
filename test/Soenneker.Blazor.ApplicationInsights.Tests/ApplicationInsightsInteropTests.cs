using Soenneker.Blazor.ApplicationInsights.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blazor.ApplicationInsights.Tests;

[Collection("Collection")]
public class ApplicationInsightsInteropTests : FixturedUnitTest
{
    private readonly IApplicationInsightsInterop _util;

    public ApplicationInsightsInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IApplicationInsightsInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
