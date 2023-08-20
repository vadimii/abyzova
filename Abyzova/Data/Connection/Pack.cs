using Abyzova.MusicXml.Preprocess;

// ReSharper disable InconsistentNaming

namespace Abyzova.Data.Connection;

public readonly record struct Pack(string Name, Type[] Preprocess)
{
    public static readonly Pack Triads_Ⅰ_Ⅳ_Ⅴ = new Pack("Abyzova-032-049.xml", new[] { typeof(DoubleBass) });
}
