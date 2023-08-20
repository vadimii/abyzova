using Abyzova.MusicXml;
using Abyzova.MusicXml.Preprocess;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class ChordComposerTests
{
    [Test]
    public void Build_OnConnectionData_ShouldPass()
    {
        var resource = ScoreResource.Get("Abyzova-032-049.xml", typeof(ChordComposer).Assembly);
        var doubleBass = new DoubleBass();
        var parts = doubleBass.Unfold(MeasureParts.Create(resource.Parts));
        var shifter = new KeyShifter(resource.Parts[0].Measures[0].Attributes!.Value.Key);
        var composer = new ChordComposer(shifter);

        var actual = composer.Build(parts).ToList();

        actual.Should().HaveCount(164);
    }
}
