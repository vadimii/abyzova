using Abyzova.MusicXml.Preprocess;

namespace Abyzova.Data.Connection;

public readonly record struct Pack(string Name, Type[] Preprocess)
{
    public static readonly Pack Main = new Pack("Abyzova-032-049.xml", new[] { typeof(DoubleBass) });
}
