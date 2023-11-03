using Abyzova.Data;
using Abyzova.Reader;
using Abyzova.Reader.MusicXml;

namespace Abyzova.Exercises;

public class Abyzova032049
{
    private const string Score = "Abyzova-032-049.xml";
    private static readonly Music[] ScoreMusic;
    private static readonly HarmonyTeacher HarmonyTeacher;

    static Abyzova032049()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        var connections = connectionParser.Parse(
            Pack.MainTriad,
            Pack.MainTriadRepetition); // TODO (vadimii): m.33
        HarmonyTeacher = new HarmonyTeacher(connections);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).ToArray();
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        HarmonyTeacher.Check(music).Should().BeEmpty();
    }
}
