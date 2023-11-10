using Abyzova.Data;

namespace Abyzova;

public static class ObviousConnection
{
    public static bool Pass(Unit lhs, Unit rhs)
    {
        // TODO (vadimii): Check weak to strong beat trantision (Syncopation)
        return lhs.Chord.Harm() == rhs.Chord.Harm();
    }
}
