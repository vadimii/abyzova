using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Notations
{
    [XmlElement("articulations")]
    public required Articulations Articulations { get; init; }
}
