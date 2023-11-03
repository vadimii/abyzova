using Abyzova.Data;
using Abyzova.Reader;
using Abyzova.Reader.MusicXml;

namespace Abyzova.Exercises;

public class Abyzova055102
{
    private const string Score = "Abyzova-055-102.xml";
    private static readonly Music[] ScoreMusic;
    private static readonly HarmonyTeacher HarmonyTeacher;

    static Abyzova055102()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        var connections = connectionParser.Parse(
            Pack.MainTriad,
            Pack.MainTriadRepetition,
            Pack.MainTriadThirdToneLeap);

        HarmonyTeacher = new HarmonyTeacher(connections);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).ToArray();

        ScoreMusic[0] = ScoreMusic[0] with { Units = ScoreMusic[0].Units[..^1] };
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        HarmonyTeacher.Check(music).Should().BeEmpty();
    }
}
