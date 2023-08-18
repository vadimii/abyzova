using System.Xml.Serialization;

namespace Abyzova.MusicXml.Nodes;

public enum Mode
{
    [XmlEnum("major")]
    Major,

    [XmlEnum("minor")]
    Minor
}
