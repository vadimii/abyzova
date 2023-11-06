using System.CommandLine;
using Abyzova.Tools.LilyPond;

var rootCommand = new RootCommand("Console tools for Abyzova project");
rootCommand.AddCommand(MusicXmlToLilyPond.GetCommand());
rootCommand.AddCommand(CheckExercise.GetCommand());

await rootCommand.InvokeAsync(args);
