using Abyzova.MusicXml.Nodes;
using Chord = Abyzova.Data.Chord;

namespace Abyzova.MusicXml;

public class ChordComposer
{
    private readonly KeyShifter _shifter;

    public ChordComposer(KeyShifter shifter)
    {
        _shifter = shifter;
    }

    public IEnumerable<Chord> Build(IEnumerable<MeasureParts> parts)
    {
        return parts.SelectMany(Zip);
    }

    private IEnumerable<Chord> Zip(MeasureParts measure)
    {
        var sNotes = measure.S.Items.OfType<Note>();
        var aNotes = measure.A.Items.OfType<Note>();
        var tNotes = measure.T.Items.OfType<Note>();
        var bNotes = measure.B.Items.OfType<Note>();

        var ranges = new List<Entry>();

        FillEntries(sNotes, Voice.S);
        FillEntries(aNotes, Voice.A);
        FillEntries(tNotes, Voice.T);
        FillEntries(bNotes, Voice.B);

        var starts = ranges.Select(x => x.Range.Start).Distinct();

        foreach (var start in starts)
        {
            var entries = ranges.Where(x => x.Range.Start <= start && x.Range.Stop > start).ToArray();

            var s = entries.Single(x => x.Voice == Voice.S).Pitch;
            var a = entries.Single(x => x.Voice == Voice.A).Pitch;
            var t = entries.Single(x => x.Voice == Voice.T).Pitch;
            var b = entries.Single(x => x.Voice == Voice.B).Pitch;

            yield return new Chord(Convert(s), Convert(a), Convert(t), Convert(b));
        }

        yield break;

        void FillEntries(IEnumerable<Note> notes, Voice voice)
        {
            var start = 0;

            foreach (var note in notes)
            {
                ranges.Add(new Entry(new Range(start, start + note.Duration), voice, note.Pitch));
                start += note.Duration;
            }
        }

        Data.Pitch Convert(Pitch pitch)
        {
            return new Data.Pitch(_shifter.Step(pitch.Step), pitch.Octave);
        }
    }

    private enum Voice
    {
        S, A, T, B
    }

    private readonly record struct Range(int Start, int Stop);
    private readonly record struct Entry(Range Range, Voice Voice, Pitch Pitch);
}
