using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public readonly record struct Articulations
{
    [XmlElement("caesura")]
    public Caesura? Caesura { get; init; }
}
