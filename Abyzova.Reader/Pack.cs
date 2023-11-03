namespace Abyzova.Reader;

public readonly record struct Pack(string Name, Pack.Preprocess[] Preprocesses)
{
    public static readonly Pack MainTriad = new("Abyzova-032-049.xml", new[] { Preprocess.DoubleBass });
    public static readonly Pack MainTriadRepetition = new("Abyzova-038-061.xml", new[] { Preprocess.DoubleBass, Preprocess.Transposition });
    public static readonly Pack MainTriadThirdToneLeap = new("Abyzova-055-102.xml", new[] { Preprocess.DoubleBass });

    public enum Preprocess
    {
        DoubleBass,
        Transposition
    }
}
