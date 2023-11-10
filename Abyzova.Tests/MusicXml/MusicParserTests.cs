using Abyzova.Reader.MusicXml;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class MusicParserTests
{
    [Test]
    public void Parse_RegularData_ShouldPass()
    {
        var score = ScoreResource.Get("Abyzova-032-049.xml");
        var parser = new MusicParser();

        var actual = parser.Parse(score);

        actual.SelectMany(x => x.Units).Should().HaveCount(155);
    }
}
