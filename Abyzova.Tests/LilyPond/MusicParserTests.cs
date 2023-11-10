using Abyzova.Tests.MusicXml;
using MusicParser = Abyzova.Reader.LilyPond.MusicParser;
using XmlMusicParser = Abyzova.Reader.MusicXml.MusicParser;

namespace Abyzova.Tests.LilyPond;

public class MusicParserTests
{
    [Test]
    public void ParseTest()
    {
        var musicXml = ScoreResource.Get("Abyzova-055-102-01.xml");
        var parser = new XmlMusicParser();
        var match = parser.Parse(musicXml).First().Units.Select(x => x.Chord);
        var source = LilyPondResource.Get("Abyzova-055-102-01.ly");
        var target = new MusicParser();

        var actual = target.Parse(source).Units.Select(x => x.Chord);

        actual.Should().BeEquivalentTo(match);
    }

    [Test]
    public void ParsePickupBarTest()
    {
        var source = LilyPondResource.Get("Brigadny-035-068-03.ly");
        var target = new MusicParser();

        var actual = target.Parse(source).Units.Select(x => x.Chord);

        actual.Should().HaveCount(8);
    }

    [Test]
    public void ParseRestsTest()
    {
        var source = LilyPondResource.Get("Brigadny-035-068-12.ly");
        var target = new MusicParser();

        var actual = target.Parse(source).Units.Select(x => x.Chord);

        actual.Should().HaveCount(19);
    }
}
