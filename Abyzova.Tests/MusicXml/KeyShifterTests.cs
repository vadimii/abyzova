using Abyzova.MusicXml;
using Abyzova.MusicXml.Nodes;
using AutoFixture;
using FluentAssertions;
using ModelStep = Abyzova.Model.Step;

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

    [TestCase(Major.C, Step.D, ModelStep.Supertonic)]
    [TestCase(Minor.A, Step.D, ModelStep.Subdominant)]
    [TestCase(Major.F, Step.D, ModelStep.Submediant)]
    [TestCase(Minor.D, Step.D, ModelStep.Tonic)]
    [TestCase(Major.D, Step.D, ModelStep.Tonic)]
    [TestCase(Minor.B, Step.D, ModelStep.Mediant)]
    [TestCase(Major.CSharp, Step.D, ModelStep.Supertonic)]
    [TestCase(Minor.GFlat, Step.D, ModelStep.Subdominant)]
    public void Step_OnAnyKey_ShouldGetCorrectStep(ValueType mode, Step step, ModelStep result)
    {
        var (keyMode, fifths) = mode switch
        {
            Major maj => (Mode.Major, (short)maj),
            Minor min => (Mode.Minor, (short)min),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };

        var key = new Key { Mode = keyMode, Fifths = fifths };
        var shifter = new KeyShifter(key);

        var actual = shifter.Step(step);

        actual.Should().Be(result);
    }

    [TestCase(Major.CSharp + 1)]
    [TestCase(Minor.GFlat - 1)]
    public void Step_OnTooLargeFifths_ShouldThrow(short fifths)
    {
        var key = new Key { Mode = _fixture.Create<Mode>(), Fifths = fifths };
        var act = () => _ = new KeyShifter(key);

        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    // ReSharper disable UnusedMember.Local

    private enum Major : short
    {
        CFlat = -7, GFlat = -6, DFlat = -5, AFlat = -4, EFlat = -3, BFlat = -2, F = -1,
        C = 0, G = 1, D = 2, A = 3, E = 4, B = 5, FSharp = 6, CSharp = 7
    }

    private enum Minor : short
    {
        GFlat = -7, EFlat = -6, BFlat = -5, F = -4, C = -3, G = -2, D = -1,
        A = 0, E = 1, B = 2, FSharp = 3, CSharp = 4, GSharp = 5, DSharp = 6
    }

    // ReSharper restore UnusedMember.Local

}
