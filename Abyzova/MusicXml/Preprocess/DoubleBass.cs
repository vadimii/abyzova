using Abyzova.MusicXml.Nodes;

namespace Abyzova.MusicXml.Preprocess;

public class DoubleBass
{
    public IEnumerable<MeasureParts> Unfold(IEnumerable<MeasureParts> parts)
    {
        foreach (var part in parts)
        {
            if (!part.B.Items.OfType<Note>().Any(x => x.Chord.HasValue))
            {
                yield return part;
            }
            else
            {
                foreach (var unfold in Unfold(part))
                {
                    yield return unfold;
                }
            }
        }
    }

    private static IEnumerable<MeasureParts> Unfold(MeasureParts parts)
    {
        var list = parts.B.Items.ToList();
        var chordIndex = list.FindIndex(x => x is Note { Chord: not null });
        var chordNote = (Note)list[chordIndex];
        list.Remove(chordNote);

        yield return parts with { B = parts.B with { Items = list.ToArray() } };

        list[chordIndex - 1] = chordNote with { Chord = default };

        yield return parts with { B = parts.B with { Items = list.ToArray() } };
    }
}
