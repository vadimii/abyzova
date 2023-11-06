using System.CommandLine;
using Abyzova.Reader;
using Abyzova.Reader.MusicXml;
using MusicParser = Abyzova.Reader.LilyPond.MusicParser;

namespace Abyzova.Tools.LilyPond;

public class CheckExercise
{
    public static Command GetCommand()
    {
        var command = new Command("check", "Check one assignment from LilyPond file");
        var limit = new Option<int?>("--limit", "Limit measure number to check")
        {
            Arity = ArgumentArity.ZeroOrOne
        };

        var target = new Argument<FileInfo>("input", "LilyPond file to check")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        command.Add(limit);
        command.AddArgument(target);
        command.SetHandler(Check, target, limit);

        return command;
    }

    private static void Check(FileInfo target, int? limit)
    {
        var source = Read(target);
        var parser = new MusicParser();
        var connectionParser = new ConnectionParser();
        var connections = connectionParser.Parse(
            Pack.MainTriad,
            Pack.MainTriadRepetition,
            Pack.MainTriadThirdToneLeap);
        var teacher = new HarmonyTeacher(connections);
        var music = parser.Parse(source, limit);

        var signals = teacher.Check(music);

        foreach (var signal in signals)
        {
            Console.WriteLine($"E: {signal.Signal} at {signal.Unit.Measure}/{signal.Unit.Number}");
        }
    }

    private static string Read(FileInfo target)
    {
        using var stream = target.OpenRead();
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
