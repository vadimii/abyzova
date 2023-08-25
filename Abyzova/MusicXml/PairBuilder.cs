using Abyzova.Data.Connection;
using Chord = Abyzova.Data.Chord;

namespace Abyzova.MusicXml;

public class PairBuilder
{
    private readonly ChordComposer _chordComposer;

    public PairBuilder(ChordComposer chordComposer)
    {
        _chordComposer = chordComposer;
    }

    public IEnumerable<Pair> Build(IEnumerable<MeasureParts> parts)
    {
        foreach (var (first, second) in Zip(parts.SelectMany(_chordComposer.Build)))
        {
            yield return new Pair(first.Harm(), Chord.Diff(first, second));
        }
    }

    private static IEnumerable<(Chord First, Chord Second)> Zip(IEnumerable<Chord> chords)
    {
        using var enumerator = chords.GetEnumerator();

        while (enumerator.MoveNext())
        {
            var first = enumerator.Current;
            enumerator.MoveNext();
            var second = enumerator.Current;

            yield return (first, second);
        }
    }
}
