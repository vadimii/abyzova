using Abyzova.Data;
using Abyzova.Data.Connection;
using Abyzova.MusicXml;

namespace Abyzova.Exercises;

public class Abyzova032049
{
    private const string Score = "Abyzova-032-049.xml";

    private static readonly ISet<Pair> Connections;
    private static readonly Music[] ScoreMusic;

    static Abyzova032049()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        Connections = connectionParser.Parse(Pack.Triads_Ⅰ_Ⅳ_Ⅴ);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score))
            .Where(x => x.Rehearsal != "3") // TODO (vadimii): Fix It!
            .ToArray();
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        foreach (var (lhs, rhs) in music.Units.Zip(music.Units.Skip(1)))
        {
            var pair = new Pair(lhs.Chord.Harm(), Chord.Diff(lhs.Chord, rhs.Chord));

            Assert.That(Connections.Contains(pair), Is.True, lhs.ToString());
        }
    }
}
