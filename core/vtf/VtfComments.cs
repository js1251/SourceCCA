using System.Text;

namespace core.vtf;

internal class VtfComments {
    private readonly Vtf _vtf;

    public VtfComments(Vtf vtf) {
        _vtf = vtf;
    }

    public void AddComment(string comment) {
        var commentBytes = Encoding.UTF8.GetBytes(comment);

        var newBytes = new byte[_vtf.Bytes.Length + commentBytes.Length];
        _vtf.Bytes.CopyTo(newBytes, 0);
        commentBytes.CopyTo(newBytes, _vtf.Bytes.Length);

        _vtf.Bytes = newBytes;
    }

    public void AddComment(string[] comments) {
        foreach (var comment in comments) {
            AddComment(comment);
        }
    }
}