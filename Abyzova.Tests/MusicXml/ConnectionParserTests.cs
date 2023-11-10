using Abyzova.Reader;
using Abyzova.Reader.MusicXml;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ConnectionParserTests
{
    [Test]
    public void Parse_MainModule_ShouldCreatePairs()
    {
        var parser = new ConnectionParser();

        var actual = parser.Parse(Pack.MainTriad);

        actual.Should().HaveCount(82);
    }
}
