namespace Abyzova.Data;

public readonly record struct Unit(int Measure, int Number, Chord Chord)
{
    public bool Ok => new[] { Chord.S.Tag, Chord.A.Tag, Chord.T.Tag, Chord.B.Tag }.Any(x => x == "✓");
}
