using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Articulations
{
    [XmlElement("caesura")]
    public Caesura? Caesura { get; init; }
}
