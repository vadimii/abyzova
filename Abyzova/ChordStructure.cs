using Abyzova.Data;

namespace Abyzova;

public class ChordStructure
{
    public ErrorEntry? Check(Unit unit)
    {
        var err = Crossing(unit.Chord) ?? BassTenorGap(unit.Chord);

        return err.HasValue ? new ErrorEntry(err.Value, unit) : null;
    }

    private static ErrorType? Crossing(Chord chord)
    {
        return chord.B > chord.T || chord.T > chord.A || chord.A > chord.S
            ? ErrorType.Crossing
            : null;
    }

    private static ErrorType? BassTenorGap(Chord chord)
    {
        return chord.T.Abs() - chord.B.Abs() > 14
            ? ErrorType.BassTenorGap
            : null;
    }
}
