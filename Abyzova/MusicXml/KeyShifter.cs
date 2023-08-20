using Abyzova.MusicXml.Nodes;

namespace Abyzova.MusicXml;

public class KeyShifter
{
    private const string Fifths = "CGDAEBF";
    private const string Notes = "CDEFGAB";

    private readonly string _notes;

    public KeyShifter(Key key)
    {
        if (key.Fifths is < -7 or > 7)
        {
            throw new ArgumentOutOfRangeException(nameof(key.Fifths));
        }

        _notes = ShiftNotes(key);
    }

    public Data.Pitch Step(Pitch pitch)
    {
        var key = pitch.Step.ToString()[0];
        var n = _notes.IndexOf(key);
        var p = Notes.IndexOf(key);
        var step = (Data.Step)(n + 1);
        var octave = p < n ? pitch.Octave - 1 : pitch.Octave;

        return new Data.Pitch(step, octave);
    }

    private static string ShiftNotes(Key key)
    {
        var fifths = key.Fifths % 7;
        var keyNote = Fifths[fifths < 0 ? ^-fifths : fifths];
        var keyIndex = Notes.IndexOf(keyNote);
        var modeShift = key.Mode == Mode.Minor ? -2 : 0;
        var shift = keyIndex + modeShift;
        var i = shift < 0 ? ^-shift : shift;

        return Notes[i..] + Notes[..i];
    }
}
