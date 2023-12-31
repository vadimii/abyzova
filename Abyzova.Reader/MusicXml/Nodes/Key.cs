﻿using System.Xml.Serialization;

namespace Abyzova.Reader.MusicXml.Nodes;

public readonly record struct Key
{
    [XmlElement("fifths")]
    public required int Fifths { get; init; }

    [XmlElement("mode")]
    public required Mode Mode { get; init; }
}
