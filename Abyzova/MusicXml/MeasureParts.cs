using Abyzova.MusicXml.Nodes;

namespace Abyzova.MusicXml;

public readonly record struct MeasureParts(Measure S, Measure A, Measure T, Measure B)
{
    public static IEnumerable<MeasureParts> Create(IReadOnlyList<Part> parts)
    {
        return parts[0].Measures.Select((x, i) => new MeasureParts(
            x,
            parts[1].Measures[i],
            parts[2].Measures[i],
            parts[3].Measures[i]
        ));
    }
}
