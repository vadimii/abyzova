using Abyzova.Data;
using Abyzova.Data.Connection;

namespace Abyzova;

public class HarmonyTeacher
{
    private static readonly ChordStructure ChordStructure = new();
    private static readonly VoiceLeading VoiceLeading = new();
    private readonly ISet<Pair> _connections;

    public HarmonyTeacher(ISet<Pair> connections)
    {
        _connections = connections;
    }

    public IEnumerable<ErrorEntry> Check(Music music)
    {
        foreach (var (lhs, rhs) in music.Units.Zip(music.Units.Skip(1)))
        {
            if (ChordStructure.Check(lhs) is { } chordStructureError)
            {
                yield return chordStructureError;
            }

            if (VoiceLeading.Check(lhs, rhs) is { } voiceLeadingError)
            {
                yield return voiceLeadingError;
            }

            var pair = new Pair(lhs.Chord.Harm(), Chord.Diff(lhs.Chord, rhs.Chord));

            if (!_connections.Contains(pair))
            {
                yield return new ErrorEntry(ErrorType.Connection, lhs);
            }
        }
    }
}
