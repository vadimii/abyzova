using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Reader.MusicXml.Preprocess;

public class Transposition
{
    private readonly Step[] _steps;

    public Transposition(params Step[] steps)
    {
        _steps = steps;
    }

    public IEnumerable<MeasureParts> Move(IEnumerable<MeasureParts> parts)
    {
        return parts.SelectMany(x => Move(x).Append(x));
    }

    private IEnumerable<MeasureParts> Move(MeasureParts part)
    {
        return _steps.Select(x =>
            new MeasureParts(Move(part.S, x), Move(part.A, x), Move(part.T, x), Move(part.B, x))
        );
    }

    private static Measure Move(Measure measure, Step step)
    {
        return measure with
        {
            Items = measure.Items.Select(x =>
            {
                if (x is Note note)
                {
                    return Move(note, step);
                }

                return x;
            }).ToArray()
        };
    }

    private static Note Move(Note note, Step step)
    {
        const int mod = (int)Step.B + 1;
        var res = (int)note.Pitch.Step + (int)step;

        return note with
        {
            Pitch = new Pitch
            {
                Step = (Step)(res % mod),
                Octave = note.Pitch.Octave + (mod > res ? 0 : 1)
            }
        };
    }
}
