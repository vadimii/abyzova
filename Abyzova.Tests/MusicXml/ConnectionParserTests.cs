using Abyzova.Data.Connection;
using Abyzova.MusicXml;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ConnectionParserTests
{
    [Test]
    public void Parse_MainModule_ShouldCreatePairs()
    {
        var parser = new ConnectionParser();

        var actual = parser.Parse(new[] { Pack.Triads_Ⅰ_Ⅳ_Ⅴ });

        actual.Should().HaveCount(62);
    }
}
