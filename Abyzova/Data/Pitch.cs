namespace Abyzova.Data;

public readonly record struct Pitch(Step Step, int Octave)
{
    public static bool operator <(Pitch lhs, Pitch rhs)
    {
        if (lhs.Octave == rhs.Octave)
        {
            return lhs.Step < rhs.Step;
        }

        return lhs.Octave < rhs.Octave;
    }

    public static bool operator >(Pitch lhs, Pitch rhs)
    {
        if (lhs.Octave == rhs.Octave)
        {
            return lhs.Step > rhs.Step;
        }

        return lhs.Octave > rhs.Octave;
    }
}
