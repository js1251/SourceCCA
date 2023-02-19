using Gameloop.Vdf.Linq;

namespace core.vmt;

public class VmtNoToolTextureAdder {
    private readonly Vmt _vmt;

    public VmtNoToolTextureAdder(Vmt vmt) {
        _vmt = vmt;
    }

    public void AddNoToolTexture() {
        if (_vmt.Vdf.Value is not VObject vObject) {
            return;
        }

        // check if already contained
        foreach (var (key, token) in vObject) {
            if (string.Equals(key.ToLower(), "%notooltexture")) {
                return;
            }
        }

        vObject.Add("%notooltexture", new VValue(1));
    }
}