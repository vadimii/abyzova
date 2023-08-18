using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Direction
{
    [XmlElement("direction-type")]
    public required DirectionValue Value { get; init; }
}
