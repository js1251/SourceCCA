using Gameloop.Vdf.Linq;

namespace core.vmt;

public sealed class Vmt {
    public VProperty Vdf { get; init; }

    public Vmt(FileSystemInfo vmtFileInfo) {
        var fileContent = File.ReadAllText(vmtFileInfo.FullName);
        Vdf = VdfExtension.Deserialize(fileContent);
    }
}