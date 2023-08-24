using Abyzova.Data;

namespace Abyzova;

public class VoiceLeading
{
    public ErrorEntry? Check(Unit lhs, Unit rhs)
    {
        var err = Overlapping(lhs.Chord, rhs.Chord) ?? Parallel(lhs.Chord, rhs.Chord);

        return err.HasValue ? new ErrorEntry(err.Value, lhs) : null;
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
