using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Part
{
    [XmlAttribute("id")]
    public required string Id { get; init; }

    [XmlElement("measure")]
    public required Measure[] Measures { get; init; }
}
