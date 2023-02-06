using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace core;

public static class VdfExtension {
    public static VProperty Deserialize(string value) {
        var vdfSanitized = Sanitize(value);
        return VdfConvert.Deserialize(vdfSanitized);
    }

    private static string Sanitize(string vdf) {
        vdf = vdf.Replace("\\", "/");
        vdf = vdf.Replace("\r\n", "\n");
        vdf = vdf.Replace("\r", "\n");

        var lines = vdf.Split("\n");

        var commentStarted = false;
        for (var i = 0; i < lines.Length; i++) {
            var line = lines[i].Trim();
            if (line.StartsWith("/*")) {
                commentStarted = true;
            }

            if (commentStarted) {
                lines[i] = $"//{line.Replace("/*", "").Replace("*/", "")}";
            }

            if (line.EndsWith("*/")) {
                commentStarted = false;
            }
        }

        var linesCopy = new List<string>(lines);
        for (var i = linesCopy.Count - 1; i >= 0; i--) {
            var line = linesCopy[i].Trim();
            if (string.IsNullOrEmpty(line.Replace("//", ""))) {
                linesCopy.RemoveAt(i);
            }
        }

        return string.Join("\n", linesCopy);
    }
}