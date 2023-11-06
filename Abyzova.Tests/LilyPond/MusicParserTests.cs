using Abyzova.Reader.LilyPond;
using Abyzova.Tests.MusicXml;
using FluentAssertions;
using XmlMusicParser = Abyzova.Reader.MusicXml.MusicParser;

namespace Abyzova.Tests.LilyPond;

public class MusicParserTests
{
    [Test]
    public void ParseTest()
    {
        var musicXml = ScoreResource.Get("Abyzova-055-102-1.xml");
        var parser = new XmlMusicParser();
        var match = parser.Parse(musicXml).First().Units.Select(x => x.Chord);
        var source = LilyPondResource.Get("Abyzova-055-102-1.ly");
        var target = new MusicParser();

        var actual = target.Parse(source).Units.Select(x => x.Chord);

        actual.Should().BeEquivalentTo(match);
    }
}
