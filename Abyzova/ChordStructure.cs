using Abyzova.Data;

namespace Abyzova;

public static class ChordStructure
{
    public static SignalPoint? Check(Unit unit)
    {
        var signal = Crossing(unit.Chord) ?? BassTenorGap(unit.Chord);

        return signal.HasValue ? new SignalPoint(signal.Value, unit) : null;
    }

    private static Signal? Crossing(Chord chord)
    {
        return chord.B > chord.T || chord.T > chord.A || chord.A > chord.S
            ? Signal.Crossing
            : null;
    }

    private static Signal? BassTenorGap(Chord chord)
    {
        return chord.T.Abs() - chord.B.Abs() > 14
            ? Signal.BassTenorGap
            : null;
    }
}
