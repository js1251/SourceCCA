using Gameloop.Vdf.Linq;

namespace core.vmt;

public class VmtComments {
    private readonly Vmt _vmt;

    public VmtComments(Vmt vmt) {
        _vmt = vmt;
    }

    public void RemoveAllComments() {
        RemoveComments(_vmt.Vdf.Value);
    }

    private void RemoveComments(VToken token, VToken? parent = null) {
        if (token.Type is VTokenType.Comment) {
            if (parent is not VObject vParentObject) {
                return;
            }

            for (var i = vParentObject.Count - 1; i >= 0; i--) {
                var current = vParentObject[i];
                if (current != token) {
                    continue;
                }

                vParentObject.RemoveAt(i);
                break;
            }

            return;
        }

        if (token is VObject vObject) {
            for (var i = vObject.Count - 1; i >= 0; i--) {
                RemoveComments(vObject[i], token);
            }

            return;
        }

        if (token is not VProperty vProperty) {
            return;
        }

        RemoveComments(vProperty.Value, token);
    }

    public void RemoveComments() {
        // Note: with current version of Gameloop.Vdf,
        // leading comments are always removed by the parser
        return;
    }

    public void AddComment(string comment) {
        if (_vmt.Vdf.Value is not VObject vObject) {
            return;
        }

        vObject.Add(VValue.CreateComment(comment));
    }

    public void AddComment(IEnumerable<string> comments) {
        foreach (var comment in comments) {
            AddComment(comment);
        }
    }
}