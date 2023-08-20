using Abyzova.MusicXml;
using FluentAssertions;

namespace Abyzova.Tests.MusicXml;

[TestFixture]
public class MeasureSplitterTests
{
    [Test]
    public void Split_OnRegularData_ShouldSplitCorrectly()
    {
        var resource = ScoreResource.Get("Abyzova-032-049.xml");
        var splitter = new MeasureSplitter();

        var actual = splitter.Split(MeasureParts.Create(resource.Parts)).ToArray();

        actual.Should().HaveCount(11);
        actual.Where(x => x.Id.Rehearsal == "6").Select(x => x.Id.Caesura).Should().BeEquivalentTo(new[] { 0, 1 });
        actual.SelectMany(x => x.Measures).Should().HaveCount(73);
    }
}
