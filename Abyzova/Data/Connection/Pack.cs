// ReSharper disable InconsistentNaming

namespace Abyzova.Data.Connection;

public readonly record struct Pack(string Name, Pack.Preprocess[] Preprocesses)
{
    public static readonly Pack Triads_Ⅰ_Ⅳ_Ⅴ = new("Abyzova-032-049.xml", new[] { Preprocess.DoubleBass });
    public static readonly Pack Position_Ⅰ_Ⅳ_Ⅴ = new("Abyzova-038-061.xml", new[] { Preprocess.DoubleBass, Preprocess.Transposition });
    public static readonly Pack Position_Thirds_Ⅰ_Ⅳ_Ⅴ = new("Abyzova-055-102.xml", new[] { Preprocess.DoubleBass });

    public enum Preprocess
    {
        DoubleBass,
        Transposition
    }
}
