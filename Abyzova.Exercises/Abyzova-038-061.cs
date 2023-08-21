using Abyzova.Data;
using Abyzova.Data.Connection;
using Abyzova.MusicXml;

namespace Abyzova.Exercises;

public class Abyzova038061
{
    private const string Score = "Abyzova-038-061.xml";

    private static readonly ISet<Pair> Connections;
    private static readonly Music[] ScoreMusic;

    static Abyzova038061()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        Connections = connectionParser.Parse(
            Pack.Triads_Ⅰ_Ⅳ_Ⅴ,
            Pack.Position_Ⅰ_Ⅳ_Ⅴ);

        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).ToArray();
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        foreach (var (lhs, rhs) in music.Units.Zip(music.Units.Skip(1)))
        {
            if (lhs.Chord.IsBassOnly(rhs.Chord))
            {
                continue;
            }

            var pair = new Pair(lhs.Chord.Harm(), Chord.Diff(lhs.Chord, rhs.Chord));

            Assert.That(Connections.Contains(pair), Is.True, lhs.ToString());
        }
    }
}