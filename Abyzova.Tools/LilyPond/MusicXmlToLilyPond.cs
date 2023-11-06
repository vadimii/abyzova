using System.CommandLine;
using System.Xml;
using System.Xml.Serialization;
using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Tools.LilyPond;

public static class MusicXmlToLilyPond
{
    public static Command GetCommand()
    {
        var command = new Command("lilypond", "Export from MusicXML to LilyPond");
        var export = new Option<FileInfo>("--export", "MusicXML file to convert")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        };

        var output = new Option<FileInfo>("--output", "LilyPond file to save result")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        };

        var rehearsal = new Option<string>("--rehearsal", "Rehearsal in the MusicXML file")
        {
            Arity = ArgumentArity.ExactlyOne,
            IsRequired = true
        };

        command.Add(export);
        command.Add(output);
        command.Add(rehearsal);
        command.SetHandler(Convert, export, output, rehearsal);

        return command;
    }

    private static void Convert(FileInfo export, FileInfo output, string rehearsal)
    {
        var score = Get(export);
        Console.WriteLine(string.Join(" ", Get(score, 0, rehearsal)));
        Console.WriteLine(string.Join(" ", Get(score, 1, rehearsal)));
        Console.WriteLine(string.Join(" ", Get(score, 2, rehearsal)));
        Console.WriteLine(string.Join(" ", Get(score, 3, rehearsal)));
    }

    private static Score Get(FileInfo fileInfo)
    {
        var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
        using var stream = fileInfo.Open(FileMode.Open);
        using var reader = XmlReader.Create(stream, settings);
        var serializer = new XmlSerializer(typeof(Score));

        return (Score)serializer.Deserialize(reader)!;
    }

    private static IEnumerable<string> Get(Score score, int voice, string rehearsal)
    {
        var divisions = score.Parts[0].Measures[0].Attributes!.Value.Divisions;
        var rehearsals = score.Parts[0].Measures.Select(x =>
        {
            var current = x.Items.OfType<Direction>().Select(y => y.Value.Rehearsal)
                .FirstOrDefault(y => !string.IsNullOrEmpty(y));

            return current;
        }).ToList();

        var start = rehearsals.FindIndex(x => x == rehearsal);
        var stop = rehearsals.FindIndex(start + 1, x => x != null);
        if (stop == -1)
        {
            stop = rehearsals.Count;
        }

        var flats = "ces des es fes ges as bes".Split().ToDictionary(x => x[0].ToString(), x => x);
        var sharps = "cis dis eis fis gis ais bis".Split().ToDictionary(x => x[0].ToString(), x => x);
        var noalt = "c d e f g a b".Split().ToDictionary(x => x, x => x);

        var pitches = new Dictionary<int, Dictionary<string, string>>
        {
            { -1, flats }, { 1, sharps }, { 0, noalt }
        };

        var startOct = voice switch
        {
            0 => 5,
            1 => 4,
            2 => 4,
            3 => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(voice), voice, null)
        };

        var prevPitch = new Pitch { Step = Step.C, Octave = startOct, Alter = 0 };
        var prevDur = string.Empty;

        // TODO (vadimii): need to write arithmetic expression
        var durs = new Dictionary<int, string>
        {
            { divisions / 2, "8" },
            { divisions / 2 + divisions / 4, "8." },
            { divisions, "4" },
            { divisions + divisions / 2, "4." },
            { divisions * 2, "2" },
            { divisions * 2 + divisions, "2." },
            { divisions * 4, "1" },
            { divisions * 4 + divisions * 2, "1." }
        };

        foreach (var measure in score.Parts[voice].Measures.Skip(start).Take(stop - start))
        {
            foreach (var note in measure.Items.OfType<Note>())
            {
                var step = note.Pitch.Step;
                var alter = note.Pitch.Alter;

                var pitch = pitches[alter][step.ToString().ToLower()];
                var leap = Leap(prevPitch, note.Pitch);
                var dur = durs[note.Duration];
                var durstr = dur == prevDur ? "" : $"{dur}";

                yield return $"{pitch}{leap}{durstr}";

                prevPitch = note.Pitch;
                prevDur = dur;
            }

            yield return "|";
        }

        yield break;

        string Leap(Pitch lhs, Pitch rhs)
        {
            var diff = ToInt(lhs) - ToInt(rhs);

            return Math.Abs(diff) > 3
                ? diff > 0
                    ? ","
                    : "'"
                : "";
        }

        int ToInt(Pitch pitch)
        {
            const string cdefgab = "CDEFGAB";
            var (n, o) = (cdefgab.IndexOf(pitch.Step.ToString(), StringComparison.Ordinal), pitch.Octave);

            return 7 * o + n;
        }
    }
}
