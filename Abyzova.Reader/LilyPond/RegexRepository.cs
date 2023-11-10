using System.Text.RegularExpressions;

namespace Abyzova.Reader.LilyPond;

internal static partial class RegexRepository
{
    [GeneratedRegex(@"^\w+\s*\=\s*{(.+)}")]
    public static partial Regex VoiceRegex();

    [GeneratedRegex(@"^\s*\\key\s+([cdefgabis]+)\s+\\(major|minor)")]
    public static partial Regex KeyRegex();

    [GeneratedRegex(@"^([cdefgabr])([eis]{0,4}){0,1}([,']){0,1}(\d[\d\.]*){0,1}")]
    public static partial Regex NoteRegex();
}
