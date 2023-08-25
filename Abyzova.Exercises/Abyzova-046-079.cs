using Abyzova.Data;
using Abyzova.Data.Connection;
using Abyzova.MusicXml;

namespace Abyzova.Exercises;

public class Abyzova046079
{
    private const string Score = "Abyzova-046-079.xml";
    private static readonly Music[] ScoreMusic;
    private static readonly HarmonyTeacher HarmonyTeacher;

    static Abyzova046079()
    {
        var connectionParser = new ConnectionParser();
        var musicParser = new MusicParser();

        var connections = connectionParser.Parse(
            Pack.Triads_Ⅰ_Ⅳ_Ⅴ,
            Pack.Position_Ⅰ_Ⅳ_Ⅴ);

        HarmonyTeacher = new HarmonyTeacher(connections);
        ScoreMusic = musicParser.Parse(ScoreResource.Get(Score)).Take(6).ToArray();
    }

    [TestCaseSource(nameof(ScoreMusic))]
    public void CheckConnections(Music music)
    {
        HarmonyTeacher.Check(music).Should().BeEmpty();
    }
}
