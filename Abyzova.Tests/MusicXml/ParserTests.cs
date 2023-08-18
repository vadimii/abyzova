using System.Xml;
using System.Xml.Serialization;
using Abyzova.MusicXml;
using Abyzova.MusicXml.Nodes;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ParserTests
{
    private static readonly XmlReaderSettings XmlReaderSettings = new() { DtdProcessing = DtdProcessing.Parse };

    [Test]
    public void Parser_Serialization_ShouldNotThrow()
    {
        var assembly = typeof(Parser).Assembly;
        var path = $"{assembly.GetName().Name}.Data.Abyzova-032-049.xml";
        using var stream = assembly.GetManifestResourceStream(path)!;
        using var reader = XmlReader.Create(stream, XmlReaderSettings);
        var serializer = new XmlSerializer(typeof(Score));
        var content = (Score)serializer.Deserialize(reader)!;

        var actual = content.Parts
            .SelectMany(x => x.Measures)
            .SelectMany(x => x.Items)
            .OfType<Note>()
            .Sum(x => x.Duration);

        actual.Should().Be(152832);
    }
}
