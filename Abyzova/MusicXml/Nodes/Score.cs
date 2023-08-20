using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

[XmlRoot("score-partwise")]
public readonly record struct Score
{
    [XmlElement("work")]
    public required Work Work { get; init; }

    [XmlElement("part")]
    public required Part[] Parts { get; init; }
}
