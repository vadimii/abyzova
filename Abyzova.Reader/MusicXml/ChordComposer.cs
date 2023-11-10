using Abyzova.Reader.MusicXml.Nodes;
using Chord = Abyzova.Data.Chord;

namespace Abyzova.Reader.MusicXml;

public class ChordComposer
{
    private readonly KeyShifter _shifter;

    public ChordComposer(KeyShifter shifter)
    {
        _shifter = shifter;
    }

    public IEnumerable<Chord> Build(MeasureParts measure)
    {
        var sNotes = measure.S.Items.OfType<Note>();
        var aNotes = measure.A.Items.OfType<Note>();
        var tNotes = measure.T.Items.OfType<Note>();
        var bNotes = measure.B.Items.OfType<Note>();

        return Build(sNotes, aNotes, tNotes, bNotes);
    }

    public IEnumerable<Chord> Build(
        IEnumerable<Note> soprano, IEnumerable<Note> alt,
        IEnumerable<Note> tenor, IEnumerable<Note> bass)
    {
        var ranges = new List<Entry>();

        FillEntries(soprano, Voice.S);
        FillEntries(alt, Voice.A);
        FillEntries(tenor, Voice.T);
        FillEntries(bass, Voice.B);

        var starts = ranges.Select(x => x.Range.Start).Distinct().OrderBy(x => x);

        foreach (var start in starts)
        {
            var entries = ranges.Where(x => x.Range.Start <= start && x.Range.Stop > start).ToArray();

            var s = entries.Single(x => x.Voice == Voice.S);
            var a = entries.Single(x => x.Voice == Voice.A);
            var t = entries.Single(x => x.Voice == Voice.T);
            var b = entries.Single(x => x.Voice == Voice.B);

            yield return new Chord(Convert(s), Convert(a), Convert(t), Convert(b));
        }

        yield break;

        void FillEntries(IEnumerable<Note> notes, Voice voice)
        {
            var start = 0;
            var prevNote = default(Note);

            foreach (var note in notes.SkipWhile(x => x.Rest.HasValue))
            {
                var pitch = note.Rest.HasValue ? prevNote.Pitch : note.Pitch;
                var tag = note.Notations.OtherNotation ?? string.Empty;
                ranges.Add(new Entry(new Range(start, start + note.Duration), voice, pitch, tag));
                start += note.Duration;
                prevNote = note;
            }
        }

        Data.Pitch Convert(Entry entry)
        {
            var pitch = _shifter.Step(entry.Pitch);

            return entry.Tag.Length > 0
                ? pitch with { Tag = entry.Tag }
                : pitch;
        }
    }

    private enum Voice
    {
        S, A, T, B
    }

    private readonly record struct Range(int Start, int Stop);
    private readonly record struct Entry(Range Range, Voice Voice, Pitch Pitch, string Tag);
}
