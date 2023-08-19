using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Note
{
    [XmlElement("pitch")]
    public required Pitch Pitch { get; init; }

    [XmlElement("duration")]
    public required int Duration { get; init; }

    [XmlElement("notations")]
    public required Notations Notations { get; init; }

    [XmlElement("chord")]
    public Chord? Chord { get; init; }
}
