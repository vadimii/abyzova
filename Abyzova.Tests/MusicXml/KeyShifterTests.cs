using Abyzova.MusicXml;
using Abyzova.MusicXml.Nodes;
using AutoFixture;
using FluentAssertions;
using ModelStep = Abyzova.Data.Step;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class KeyShifterTests
{
    private IFixture _fixture = null!;

    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    [TestCase(Major.C, Step.A, ModelStep.Submediant)]
    [TestCase(Minor.A, Step.B, ModelStep.Supertonic)]
    [TestCase(Major.F, Step.C, ModelStep.Dominant)]
    [TestCase(Minor.D, Step.D, ModelStep.Tonic)]
    [TestCase(Major.D, Step.D, ModelStep.Tonic)]
    [TestCase(Minor.B, Step.E, ModelStep.Subdominant)]
    [TestCase(Major.CSharp, Step.F, ModelStep.Subdominant)]
    [TestCase(Minor.AFlat, Step.G, ModelStep.Subtonic)]
    public void Step_OnAnyKey_ShouldGetCorrectStep(ValueType mode, Step step, ModelStep result)
    {
        var (keyMode, fifths) = mode switch
        {
            Major maj => (Mode.Major, (int)maj),
            Minor min => (Mode.Minor, (int)min),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        var key = new Key { Mode = keyMode, Fifths = fifths };
        var shifter = new KeyShifter(key);

        var actual = shifter.Step(step);

        actual.Should().Be(result);
    }

    [TestCase(Major.CSharp + 1)]
    [TestCase(Minor.AFlat - 1)]
    public void Step_OnTooLargeFifths_ShouldThrow(int fifths)
    {
        var key = new Key { Mode = _fixture.Create<Mode>(), Fifths = fifths };
        var act = () => _ = new KeyShifter(key);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // ReSharper disable UnusedMember.Local

    private enum Major
    {
        CFlat = -7, GFlat = -6, DFlat = -5, AFlat = -4, EFlat = -3, BFlat = -2, F = -1,
        C = 0, G = 1, D = 2, A = 3, E = 4, B = 5, FSharp = 6, CSharp = 7
    }

    private enum Minor
    {
        AFlat = -7, EFlat = -6, BFlat = -5, F = -4, C = -3, G = -2, D = -1,
        A = 0, E = 1, B = 2, FSharp = 3, CSharp = 4, GSharp = 5, DSharp = 6, ASharp = 7
    }

    // ReSharper restore UnusedMember.Local

}
