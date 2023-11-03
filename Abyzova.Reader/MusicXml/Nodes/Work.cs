using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Work
{
    [XmlElement("work-title")]
    public required string Title { get; init; }
}
