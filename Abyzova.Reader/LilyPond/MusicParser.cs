using System.Diagnostics;
using System.Text.RegularExpressions;
using Abyzova.Data;
using Abyzova.Reader.MusicXml;
using Abyzova.Reader.MusicXml.Nodes;
using Pitch = Abyzova.Reader.MusicXml.Nodes.Pitch;
using Step = Abyzova.Reader.MusicXml.Nodes.Step;

namespace Abyzova.Reader.LilyPond;

public partial class MusicParser
{
    public Music Parse(string raw, int? limit = null)
    {
        var voices = SplitVoices(raw);

        var key = GetKey(voices[0]);
        var shifter = new KeyShifter(key);
        var composer = new ChordComposer(shifter);
        var take = limit.GetValueOrDefault(int.MaxValue);

        var sop = ParseVoice(voices[1].Split("|").Take(take), 5);
        var alt = ParseVoice(voices[2].Split("|").Take(take), 4);
        var ten = ParseVoice(voices[3].Split("|").Take(take), 4);
        var bas = ParseVoice(voices[4].Split("|").Take(take), 3);

        var units = new List<Unit>();

        foreach (var (measure, i) in ZipIterator(sop, alt, ten, bas).Select((x, i) => (x, i)))
        {
            var chords = composer.Build(measure.First, measure.Second, measure.Third, measure.Fourth);
            units.AddRange(chords.Select((x, j) => new Unit(i + 1, j + 1, x)));
        }

        return new Music("LilyPond", "1", 0, units.ToArray());
    }

    private static string[] SplitVoices(string raw)
    {
        using var reader = new StringReader(raw);
        var regex = VoiceRegex();
        var voices = new string[5];

        while (reader.ReadLine() is { } line)
        {
            if (line.Length < 3)
            {
                continue;
            }

            var voice = line[..3] switch
            {
                "sig" => 0,
                "sop" => 1,
                "alt" => 2,
                "ten" => 3,
                "bas" => 4,
                _ => -1
            };

            if (voice == -1)
            {
                continue;
            }

            voices[voice] = regex.Match(line).Groups[1].Value;
        }

        return voices;
    }

    private static LilyNote ParseNote(string token)
    {
        var match = NoteRegex().Match(token);

        Debug.Assert(match.Success);

        var step = match.Groups[1].Value;
        var alter = match.Groups[2].Value;
        var leap = match.Groups[3].Value;
        var dur = match.Groups[4].Value;

        return new LilyNote(step, alter, leap, dur);
    }

    [GeneratedRegex(@"^\w+\s*\=\s*{(.+)}")]
    private static partial Regex VoiceRegex();

    [GeneratedRegex(@"^\s*\\key\s+([cdefgabis]+)\s+\\(major|minor)")]
    private static partial Regex KeyRegex();

    [GeneratedRegex(@"^([cdefgab])([eis]{0,4}){0,1}([,']){0,1}(\d[\d\.]*){0,1}")]
    private static partial Regex NoteRegex();

    private record struct LilyNote(string Step, string Alter, string Leap, string Duration);

    private record ParseContext
    {
        public Pitch Pitch { get; set; }
        public string Duration { get; set; } = string.Empty;
    };

    private static Key GetKey(string tokens)
    {
        var majorSharps = "g d a e b fis".Split().ToList();
        var minorSharps = "e b fis cis gis dis".Split().ToList();

        var majorFlats = "f bes es as des ges".Split().ToList();
        var minorFlats = "d g c f bes es".Split().ToList();

        var match = KeyRegex().Match(tokens);

        Debug.Assert(match.Success);

        var key = ParseNote(match.Groups[1].Value);
        var mode = Enum.Parse<Mode>(match.Groups[2].Value, true);

        var fifths = 0;

        if (mode == Mode.Major)
        {
            var majIdx = majorSharps.IndexOf(key.Step);

            if (majIdx != -1)
            {
                fifths = majIdx + 1;
            }
            else
            {
                majIdx = majorFlats.IndexOf(key.Step);

                if (majIdx != -1)
                {
                    fifths = -(majIdx + 1);
                }
            }
        }
        else
        {
            var minIdx = minorSharps.IndexOf(key.Step);

            if (minIdx != -1)
            {
                fifths = minIdx + 1;
            }
            else
            {
                minIdx = minorFlats.IndexOf(key.Step);

                if (minIdx != -1)
                {
                    fifths = -(minIdx + 1);
                }
            }
        }

        return new Key { Fifths = fifths, Mode = mode };
    }

    private static Note GetNote(LilyNote lily, ParseContext context)
    {
        const int divs = 192;
        // TODO (vadimii): need to write arithmetic expression
        var durs = new Dictionary<string, int>
        {
            { "8", divs / 2 },
            { "8.", divs / 2 + divs / 4 },
            { "4", divs },
            { "4.", divs + divs / 2 },
            { "2", divs * 2 },
            { "2.", divs * 2 + divs },
            { "1", divs * 4 },
            { "1.", divs * 4 + divs * 2 }
        };

        context.Duration = lily.Duration == string.Empty ? context.Duration : lily.Duration;

        var duration = durs[context.Duration];

        var step = Enum.Parse<Step>(lily.Step, true);

        var alter = lily.Alter switch
        {
            "es" => -1, // TODO (vadimii): double values
            "s" => -1,
            "is" => 1,
            _ => 0
        };

        var octaveMod = lily.Leap switch
        {
            "," => -1, // TODO (vadimii): double values
            "'" => 1,
            _ => 0
        };

        var octave = CalcOctave(context.Pitch, step) + octaveMod;
        var pitch = new Pitch { Step = step, Octave = octave, Alter = alter};
        context.Pitch = pitch;

        return new Note { Pitch = pitch, Notations = default, Duration = duration };

        int CalcOctave(Pitch refPitch, Step nextStep)
        {
            var nextPitch = refPitch with { Step = nextStep };
            var diff = ToInt(nextPitch) - ToInt(refPitch);

            return Math.Abs(diff) > 3
                ? diff < 0
                    ? refPitch.Octave + 1
                    : refPitch.Octave - 1
                : refPitch.Octave;

            int ToInt(Pitch p)
            {
                const string cdefgab = "CDEFGAB";
                var (n, o) = (cdefgab.IndexOf(p.Step.ToString(), StringComparison.Ordinal), p.Octave);

                return 7 * o + n;
            }
        }
    }

    private static IEnumerable<Note[]> ParseVoice(IEnumerable<string> voice, int octave)
    {
        var context = new ParseContext { Pitch = new Pitch { Step = Step.C, Alter = 0, Octave = octave } };

        foreach (var measure in voice)
        {
            var notes = measure.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            if (notes.Length > 1 && notes[0] == "\\partial")
            {
                notes = notes.Skip(2).ToArray();
            }

            yield return notes.Select(x => GetNote(ParseNote(x), context)).ToArray();
        }
    }

    private static IEnumerable<(TFirst First, TSecond Second, TThird Third, TFourth Fourth)>
        ZipIterator<TFirst, TSecond, TThird, TFourth>(
            IEnumerable<TFirst> first, IEnumerable<TSecond> second,
            IEnumerable<TThird> third, IEnumerable<TFourth> fourth)
    {
        using var e1 = first.GetEnumerator();
        using var e2 = second.GetEnumerator();
        using var e3 = third.GetEnumerator();
        using var e4 = fourth.GetEnumerator();

        while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext() && e4.MoveNext())
        {
            yield return (e1.Current, e2.Current, e3.Current, e4.Current);
        }
    }
}
