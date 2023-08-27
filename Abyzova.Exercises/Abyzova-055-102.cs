using Abyzova.Data;
using Abyzova.Data.Connection;
using Abyzova.MusicXml;

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
            Pack.Triads_Ⅰ_Ⅳ_Ⅴ,
            Pack.Position_Ⅰ_Ⅳ_Ⅴ,
            Pack.Position_Thirds_Ⅰ_Ⅳ_Ⅴ);

        HarmonyTeacher = new HarmonyTeacher(connections);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).Take(1).ToArray();

        ScoreMusic[0] = ScoreMusic[0] with { Units = ScoreMusic[0].Units[..^1] };
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        HarmonyTeacher.Check(music).Should().BeEmpty();
    }
}
