using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public enum Mode
{
    [XmlEnum("major")]
    Major,

    [XmlEnum("minor")]
    Minor
}
