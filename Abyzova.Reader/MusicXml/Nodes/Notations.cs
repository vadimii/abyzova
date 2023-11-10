using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Notations
{
    [XmlElement("articulations")]
    public required Articulations Articulations { get; init; }

    [XmlElement("other-notation")]
    public required string? OtherNotation { get; init; }

    public static readonly Notations Empty = new() { Articulations = default, OtherNotation = string.Empty };
}
