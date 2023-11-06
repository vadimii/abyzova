namespace Abyzova.Tests.LilyPond;

public class LilyPondResource
{
    public static string Get(string name)
    {
        var assembly = typeof(LilyPondResource).Assembly;
        var path = $"{assembly.GetName().Name}.Resources.{name}";
        using var stream = assembly.GetManifestResourceStream(path)!;
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
