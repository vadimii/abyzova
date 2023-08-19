using Abyzova.Data;
using Abyzova.Data.Connection;
using Abyzova.MusicXml.Nodes;
using Chord = Abyzova.Data.Chord;

namespace Abyzova.MusicXml;

public class PairBuilder
{
    private readonly ChordComposer _chordComposer;

    public PairBuilder(ChordComposer chordComposer)
    {
        _chordComposer = chordComposer;
    }

    public IEnumerable<Pair> Build(Part[] parts)
    {
        foreach (var (first, second) in Zip(_chordComposer.Build(MeasureParts.Create(parts))))
        {
            yield return new Pair(first, new Move());
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
