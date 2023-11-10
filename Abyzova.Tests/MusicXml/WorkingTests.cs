using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class WorkingTests
{
    [Test]
    public void NoteToIntTest()
    {
        const string notesIn = "C0 D0 E0 F0 G0 A0 B0 C1 D1 E1 F1 G1 A1 B1 C2 D2 E2 F2 G2 A2 B2 C3 D3";
        const string numbsIn = "00 01 02 03 04 05 06 07 08 09 10 11 12 13 14 15 16 17 18 19 20 21 22";

        var notes = notesIn.Split().Select(Conv);
        var numbs = numbsIn.Split().Select(int.Parse);

        notes.Should().BeEquivalentTo(numbs);

        return;

        int Conv(string note)
        {
            const string cdefgab = "CDEFGAB";
            var (n, o) = (cdefgab.IndexOf(note[0]), int.Parse(note[1].ToString()));

            return 7 * o + n;
        }
    }

    [Test]
    public void YolochkaTest()
    {
        var notes = "D4 B4 B4 A4 B4 G4 D4 D4 D4 B4 B4 C5 A4 D5 D5 E4 E4 C5 C5 B4 A4 G4 G4 B4 B4 A4 B4 G4".Split();
        var lilyp = "d, b' b a b g d d d b' b c a d d e, e c' c b a g g b b a b g".Split();

        var rel = new Pitch { Step = Step.C, Octave = 5, Alter = 0 };

        var pithes = notes.Select(x => new Pitch
        {
            Step = Enum.Parse<Step>(x[0].ToString()),
            Octave = int.Parse(x[1].ToString()),
            Alter = 0
        }).Prepend(rel).ToArray();

        var score = pithes.Zip(pithes.Skip(1)).Select(x => Lily(x.First, x.Second));

        score.Should().BeEquivalentTo(lilyp);

        return;

        int ToInt(Pitch pitch)
        {
            const string cdefgab = "CDEFGAB";
            var (n, o) = (cdefgab.IndexOf(pitch.Step.ToString(), StringComparison.Ordinal), pitch.Octave);

            return 7 * o + n;
        }

        string Lily(Pitch lhs, Pitch rhs)
        {
            var diff = ToInt(lhs) - ToInt(rhs);
            var mod = Math.Abs(diff) > 3
                ? diff > 0
                    ? ","
                    : "'"
                : "";

            return rhs.Step.ToString().ToLower() + mod;
        }
    }
}
