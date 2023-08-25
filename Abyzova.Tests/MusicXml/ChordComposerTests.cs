using Abyzova.MusicXml;
using Abyzova.MusicXml.Nodes;
using Abyzova.MusicXml.Preprocess;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ChordComposerTests
{
    [Test]
    public void Build_MainTriads_ShouldPass()
    {
        var resource = ScoreResource.Get("Abyzova-032-049.xml", typeof(ChordComposer).Assembly);
        var doubleBass = new DoubleBass();
        var parts = doubleBass.Unfold(MeasureParts.Create(resource.Parts));
        var shifter = new KeyShifter(resource.Parts[0].Measures[0].Attributes!.Value.Key);
        var composer = new ChordComposer(shifter);

        var actual = parts.SelectMany(composer.Build);

        actual.Should().HaveCount(164);
    }

    [Test]
    public void Build_TriadPosition_ShouldPass()
    {
        var resource = ScoreResource.Get("Abyzova-038-061.xml", typeof(ChordComposer).Assembly);
        var doubleBass = new DoubleBass();
        var transposition = new Transposition(Step.F, Step.G);
        var parts = transposition.Move(doubleBass.Unfold(MeasureParts.Create(resource.Parts)));
        var shifter = new KeyShifter(resource.Parts[0].Measures[0].Attributes!.Value.Key);
        var composer = new ChordComposer(shifter);

        var actual = parts.SelectMany(composer.Build);

        actual.Should().HaveCount(438);
    }
}
