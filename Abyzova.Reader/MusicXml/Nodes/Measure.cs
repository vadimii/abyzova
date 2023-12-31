﻿using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Measure
{
    [XmlAttribute("number")]
    public required int Number { get; init; }

    [XmlElement("attributes")]
    public Attributes? Attributes { get; init; }

    [XmlElement("note", Type=typeof(Note))]
    [XmlElement("direction", Type=typeof(Direction))]
    public required object[] Items { get; init; }
}
