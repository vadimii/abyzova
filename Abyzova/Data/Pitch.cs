namespace Abyzova.Data;

public readonly record struct Pitch(Step Step, int Octave, string Tag)
{
    public static bool operator <(Pitch lhs, Pitch rhs)
    {
        if (lhs.Octave == rhs.Octave)
        {
            return lhs.Step < rhs.Step;
        }

        return lhs.Octave < rhs.Octave;
    }

    public static bool operator <=(Pitch lhs, Pitch rhs)
    {
        return lhs < rhs || lhs == rhs;
    }

    public static bool operator >(Pitch lhs, Pitch rhs)
    {
        if (lhs.Octave == rhs.Octave)
        {
            return lhs.Step > rhs.Step;
        }

        return lhs.Octave > rhs.Octave;
    }

    public static bool operator >=(Pitch lhs, Pitch rhs)
    {
        return lhs > rhs || lhs == rhs;
    }

    public int Abs()
    {
        return Octave * 7 + (int)Step;
    }
}
