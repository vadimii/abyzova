using Abyzova.Data;
using Abyzova.Reader;
using Abyzova.Reader.MusicXml;

namespace Abyzova.Exercises;

public class Brigadny035068
{
    private const string Score = "Brigadny-035-068.xml";
    private static readonly Music[] ScoreMusic;
    private static readonly HarmonyTeacher HarmonyTeacher;

    static Brigadny035068()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        var connections = connectionParser.Parse(Pack.MainTriad);

        HarmonyTeacher = new HarmonyTeacher(connections);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).ToArray();
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        HarmonyTeacher.Check(music).Should().BeEmpty();
    }
}
