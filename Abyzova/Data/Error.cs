// ReSharper disable NotAccessedPositionalProperty.Global
namespace Abyzova.Data;

public readonly record struct ErrorEntry(ErrorType Error, Unit Unit);

public enum ErrorType
{
    Connection,
    Overlapping,
    Parallel,
    Crossing,
    BassTenorGap
}
