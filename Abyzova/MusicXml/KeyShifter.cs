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

    public Data.Step Step(Step step)
    {
        var i = _notes.IndexOf(step.ToString()[0]);

        return (Data.Step)(i + 1);
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
