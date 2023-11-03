using Abyzova.Reader.MusicXml;
using Abyzova.Reader.MusicXml.Nodes;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ParserTests
{
    [Test]
    public void Parser_Serialization_ShouldNotThrow()
    {
        var content = ScoreResource.Get("Abyzova-032-049.xml", typeof(ConnectionParser).Assembly);

        var actual = content.Parts
            .SelectMany(x => x.Measures)
            .SelectMany(x => x.Items)
            .OfType<Note>()
            .Sum(x => x.Duration);

        actual.Should().Be(187392);
    }
}
