using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct DirectionValue
{
    [XmlElement("words")]
    public string Words { get; init; }

    [XmlElement("rehearsal")]
    public string Rehearsal { get; init; }
}
