﻿using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Attributes
{
    [XmlElement("key")]
    public required Key Key { get; init; }

    [XmlElement("divisions")]
    public required int Divisions { get; init; }
}
