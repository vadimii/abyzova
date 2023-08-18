using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Pitch
{
    [XmlElement("step")]
    public required Step Step { get; init; }

    [XmlElement("octave")]
    public required uint Octave { get; init; }
}
