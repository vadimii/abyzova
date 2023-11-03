using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Reader.MusicXml.Preprocess;

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
        var replaceIndex = -1;
        var chordNotes = new List<Note>();
        var items = new List<object>(parts.B.Items);

        foreach (var (item, i) in parts.B.Items.Select((x, i) => (x, i)))
        {
            if (item is not Note { Chord: not null } note)
            {
                continue;
            }

            if (replaceIndex == -1)
            {
                replaceIndex = i - 1;
            }

            items.Remove(item);
            chordNotes.Add(note with { Chord = null });
        }

        yield return parts with { B = parts.B with { Items = items.ToArray() } };

        foreach (var note in chordNotes)
        {
            items[replaceIndex] = note;

            yield return parts with { B = parts.B with { Items = items.ToArray() } };
        }
    }
}
