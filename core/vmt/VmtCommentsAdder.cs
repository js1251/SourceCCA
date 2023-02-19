using Gameloop.Vdf.Linq;

namespace core.vmt;

public class VmtCommentsAdder {
    private readonly Vmt _vmt;

    public VmtCommentsAdder(Vmt vmt) {
        _vmt = vmt;
    }

    public void AddComment(string comment) {
        if (_vmt.Vdf.Value is not VObject vObject) {
            return;
        }

        comment = comment.Replace("/*", "");
        comment = comment.Replace("//", "");
        comment = comment.Replace("*/", "");

        vObject.Add(VValue.CreateComment(comment));
    }

    public void AddComment(IEnumerable<string> comments) {
        foreach (var comment in comments) {
            AddComment(comment);
        }
    }
}