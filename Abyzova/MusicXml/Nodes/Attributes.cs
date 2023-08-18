using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Attributes
{
    [XmlElement("key")]
    public required Key Key { get; init; }
}
