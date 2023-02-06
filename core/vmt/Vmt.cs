using System.Text;
using Gameloop.Vdf;
using Gameloop.Vdf.Linq;

namespace core.vmt;

public sealed class Vmt {
    public VProperty Vdf { get; init; }

    public Vmt(FileSystemInfo vmtFileInfo) {
        var fileContent = File.ReadAllText(vmtFileInfo.FullName);
        Vdf = VdfExtension.Deserialize(fileContent);
    }

    public byte[] GetBytes() {
        return Encoding.UTF8.GetBytes(ToString());
    }

    public override string ToString() {
        return VdfConvert.Serialize(Vdf);
    }
}