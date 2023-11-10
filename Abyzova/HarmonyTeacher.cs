using Abyzova.Data;
using Abyzova.Data.Connection;

namespace Abyzova;

public class HarmonyTeacher
{
    private readonly ISet<Pair> _connections;

    public HarmonyTeacher(ISet<Pair> connections)
    {
        _connections = connections;
    }

    public IEnumerable<SignalPoint> Check(Music music)
    {
        var third = default(Unit);

        foreach (var (lhs, rhs) in music.Units.Zip(music.Units.Skip(1)))
        {
            if (ChordStructure.Check(lhs) is { } chordStructureSignal)
            {
                yield return chordStructureSignal;
            }

            if (VoiceLeading.Check(lhs, rhs) is { } voiceLeadingSignal)
            {
                yield return voiceLeadingSignal;
            }

            if (VoiceLeading.Check(third, lhs, rhs) is { } voiceLeadingSignal2)
            {
                yield return voiceLeadingSignal2;
            }

            third = lhs;

            if (ObviousConnection.Pass(lhs, rhs))
            {
                continue;
            }

            var pair = new Pair(lhs.Chord.Harm(), Chord.Diff(lhs.Chord, rhs.Chord));

            if (!_connections.Contains(pair))
            {
                yield return new SignalPoint(Signal.Connection, lhs);
            }
        }
    }
}
