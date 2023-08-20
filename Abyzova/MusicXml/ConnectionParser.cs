using Abyzova.Data.Connection;
using Abyzova.MusicXml.Preprocess;

namespace Abyzova.MusicXml;

public class ConnectionParser
{
    private static readonly DoubleBass DoubleBass = new();

    public ISet<Pair> Parse(Pack[] packs)
    {
        var result = new HashSet<Pair>();

        foreach (var pack in packs)
        {
            var pairs = Load(pack);

            foreach (var pair in pairs)
            {
                result.Add(pair);
            }
        }

        return result;
    }

    private static IEnumerable<Pair> Load(Pack pack)
    {
        var resource = ScoreResource.Get(pack.Name);
        var parts = MeasureParts.Create(resource.Parts);

        foreach (var type in pack.Preprocess)
        {
            if (type == typeof(DoubleBass))
            {
                parts = DoubleBass.Unfold(parts);
            }
        }

        var shifter = new KeyShifter(resource.Parts[0].Measures[0].Attributes!.Value.Key);
        var composer = new ChordComposer(shifter);
        var builder = new PairBuilder(composer);

        return builder.Build(parts);
    }
}
