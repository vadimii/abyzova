using System.Xml;
using System.Xml.Serialization;
using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Exercises;

internal static class ScoreResource
{
    private static readonly XmlReaderSettings XmlReaderSettings = new() { DtdProcessing = DtdProcessing.Parse };

    public static Score Get(string name)
    {
        var assembly = typeof(ScoreResource).Assembly;
        var path = $"{assembly.GetName().Name}.Resources.{name}";
        using var stream = assembly.GetManifestResourceStream(path)!;
        using var reader = XmlReader.Create(stream, XmlReaderSettings);
        var serializer = new XmlSerializer(typeof(Score));

        return (Score)serializer.Deserialize(reader)!;
    }
}
