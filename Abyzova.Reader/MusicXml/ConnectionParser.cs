using Abyzova.Data.Connection;
using Abyzova.Reader.MusicXml.Nodes;
using Abyzova.Reader.MusicXml.Preprocess;

namespace Abyzova.Reader.MusicXml;

public class ConnectionParser
{
    private static readonly DoubleBass DoubleBass = new();
    private static readonly Transposition Transposition = new(Step.F, Step.G);

    public ISet<Pair> Parse(params Pack[] packs)
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
        var parts = MeasureParts.Create(resource.Parts).ToArray();

        parts = pack.Preprocesses.Aggregate(parts, (current, preprocess) =>
            preprocess switch
            {
                Pack.Preprocess.DoubleBass => DoubleBass.Unfold(current).ToArray(),
                Pack.Preprocess.Transposition => Transposition.Move(current).ToArray(),
                _ => parts
            });

        var shifter = new KeyShifter(resource.Parts[0].Measures[0].Attributes!.Value.Key);
        var composer = new ChordComposer(shifter);
        var builder = new PairBuilder(composer);

        return builder.Build(parts);
    }
}
