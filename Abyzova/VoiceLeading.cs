using Abyzova.Data;

namespace Abyzova;

public class VoiceLeading
{
    public bool Check(Chord lhs, Chord rhs)
    {
        // TODO (vadimii): More checks...
        return lhs.T >= rhs.B;
    }
}
