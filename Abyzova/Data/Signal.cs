namespace Abyzova.Data;

public readonly record struct SignalPoint(Signal Signal, Unit Unit);

public enum Signal
{
    Connection,
    Overlapping,
    Parallel,
    Crossing,
    BassTenorGap,
    SeventhInBass
}
