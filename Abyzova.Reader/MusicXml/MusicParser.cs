using Abyzova.Data;
using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Reader.MusicXml;

public class MusicParser
{
    public IEnumerable<Music> Parse(Score score)
    {
        var splitter = new MeasureSplitter();
        var parts = MeasureParts.Create(score.Parts).ToArray();
        var sGroups = splitter.Split(parts);

        ChordComposer? composer = null;

        foreach (var group in sGroups)
        {
            if (group.Id.Caesura == 0)
            {
                var shifter = new KeyShifter(group.Measures[0].S.Attributes!.Value.Key);
                composer = new ChordComposer(shifter);
            }

            var chords = group.Measures.SelectMany(measure =>
                composer!.Build(measure).Select((chord, i) =>
                    new Unit(measure.S.Number, i + 1, chord))).ToArray();

            yield return new Music(score.Work.Title, group.Id.Rehearsal, group.Id.Caesura, chords);
        }
    }
}
