﻿using Abyzova.Reader.MusicXml.Nodes;

namespace Abyzova.Reader.MusicXml;

public class MeasureSplitter
{
    public IEnumerable<Group> Split(IEnumerable<MeasureParts> measures)
    {
        var id = new Id();
        var group = new List<MeasureParts>();
        var prevCaesura = false;

        foreach (var measure in measures)
        {
            var rehearsal = measure.S.Items.OfType<Direction>().Select(x => x.Value.Rehearsal)
                .FirstOrDefault(x => !string.IsNullOrEmpty(x));

            var caesura = measure.S.Items.OfType<Note>().Select(x => x.Notations.Articulations.Caesura)
                .FirstOrDefault(x => x.HasValue);

            if (rehearsal is not null)
            {
                if (group.Count > 0)
                {
                    yield return new Group(id, group.ToArray());
                }

                id = new Id(rehearsal);
                group = new List<MeasureParts>();
            }

            if (prevCaesura)
            {
                if (group.Count > 0)
                {
                    yield return new Group(id, group.ToArray());
                }

                id = id with { Caesura = (ushort)(id.Caesura + 1) };
                group = new List<MeasureParts>();
            }

            group.Add(measure);
            prevCaesura = caesura.HasValue;
        }

        if (group.Count > 0)
        {
            yield return new Group(id, group.ToArray());
        }
    }

    public readonly record struct Id(string Rehearsal, int Caesura = 0);

    public readonly record struct Group(Id Id, MeasureParts[] Measures);
}
