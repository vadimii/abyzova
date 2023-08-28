using Abyzova.Data.Connection;

namespace Abyzova.Data;

public readonly record struct Chord(Pitch S, Pitch A, Pitch T, Pitch B)
{
    public static Diff Diff(Chord lhs, Chord rhs)
    {
        return new Diff(Val(lhs.S, rhs.S), Val(lhs.A, rhs.A), Val(lhs.T, rhs.T), Val(lhs.B, rhs.B));

        int Val(Pitch x, Pitch y) => y.Abs() - x.Abs();
    }

    public Harm Harm()
    {
        return new Harm((int)S.Step, (int)A.Step, (int)T.Step, (int)B.Step);
    }

    public bool IsHarmConnected(Chord chord)
    {
        return S == chord.S || A == chord.A || T == chord.T;
    }
}
