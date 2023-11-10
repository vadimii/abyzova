using Abyzova.Reader.LilyPond;

namespace Abyzova.Tests;

public class ObviousConnectionTests
{
    [TestCaseSource(nameof(Data))]
    public void PassTest(string[] lily)
    {
        var parser = new MusicParser();
        var music = parser.Parse(default, lily);

        var actual = ObviousConnection.Pass(music.Units[0], music.Units[1]);

        actual.Should().BeTrue();
    }

    public static readonly object[] Data =
    {
        new[]
        {
            "c4 c",
            "e2",
            "g2",
            "c2"
        },
        new[]
        {
            "c4 c",
            "e4 e",
            "g4 g",
            "c4 c"
        },
        new[]
        {
            "c2",
            "e2",
            "g2",
            "c4 c,"
        },
        new[]
        {
            "c2",
            "e2",
            "g2",
            "c4 c'"
        }
    };
}
