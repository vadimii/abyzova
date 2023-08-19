using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Notations
{
    [XmlElement("articulations")]
    public required Articulations Articulations { get; init; }
}
