using Abyzova.Data;

namespace Abyzova;

public class VoiceLeading
{
    public ErrorEntry? Check(Unit lhs, Unit rhs)
    {
        var err = Overlapping(lhs.Chord, rhs.Chord) ?? Parallel(lhs.Chord, rhs.Chord);

        return err.HasValue ? new ErrorEntry(err.Value, lhs) : null;
    }

    public ErrorEntry? Check(Unit first, Unit second, Unit third)
    {
        if (first == default)
        {
            return null;
        }

        // TODO (vadimii): Research
        // if (!first.Chord.IsHarmConnected(second.Chord) ||
        //     !second.Chord.IsHarmConnected(third.Chord))
        // {
        //     return null;
        // }

        var step1 = Chord.Diff(first.Chord, second.Chord).B;
        var step2 = Chord.Diff(second.Chord, third.Chord).B;

        var abs = Math.Abs(step1);

        return step1 == step2 && abs is 3 or 4 // two fourths or two fifths
            ? new ErrorEntry(ErrorType.SeventhInBass, first)
            : null;
    }

    private static ErrorType? Overlapping(Chord lhs, Chord rhs)
    {
        return lhs.B > rhs.T || lhs.T < rhs.B ||
               lhs.T > rhs.A || lhs.A < rhs.T ||
               lhs.A > rhs.S || lhs.S < rhs.A
            ? ErrorType.Overlapping
            : null;
    }

    private static ErrorType? Parallel(Chord lhs, Chord rhs)
    {
        return (lhs.S < rhs.S && lhs.A < rhs.A && lhs.T < rhs.T && lhs.B < rhs.B) ||
               (lhs.S > rhs.S && lhs.A > rhs.A && lhs.T > rhs.T && lhs.B > rhs.B)
               ? ErrorType.Parallel
               : null;
    }
}
