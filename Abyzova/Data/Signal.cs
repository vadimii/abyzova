namespace Abyzova.Data;

public readonly record struct SignalPoint(Signal Signal, Unit Unit, bool Ok);

public enum Signal
{
    Connection,
    Overlapping,
    Parallel,
    Crossing,
    BassTenorGap,
    SeventhInBass
}
