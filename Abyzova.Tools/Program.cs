using System.CommandLine;
using Abyzova.Tools.Lilypond;

var rootCommand = new RootCommand("Console tools for Abyzova project");
rootCommand.AddCommand(MusicXmlToLilyPond.GetCommand());

await rootCommand.InvokeAsync(args);
