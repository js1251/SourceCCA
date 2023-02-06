using core.vmt;
using core.vtf;

namespace core;

public class ContentProtector {
    public bool RemoveExistingComments { get; init; } = true;

    public string[] Comment {
        get => _comment;
        init {
            _comment = new string[_watermark.Length + value.Length];
            _watermark.CopyTo(_comment, 0);
            value.CopyTo(_comment, _watermark.Length);
        }
    }

    private readonly string[] _comment;

    private readonly string[] _watermark = {
        "===============================",
        "= Comments added by SourceCCA =",
        "=   Created by Jakob Sailer   =",
        "=    Visit jakobsailer.com    =",
        "==============================="
    };

    public ContentProtector(DirectoryInfo dirInfo) {
        _comment = Array.Empty<string>();

        if (!Directory.Exists(dirInfo.FullName)) {
            throw new ArgumentException("Directory does not exist", nameof(dirInfo));
        }

        var files = GetFiles(dirInfo);
        foreach (var file in files) {
            switch (file.Extension) {
                case ".vmt":
                    ProtectVmt(file);
                    break;
                case ".vtf":
                    ProtectVtf(file);
                    break;
            }
        }
    }

    private List<FileSystemInfo> GetFiles(DirectoryInfo root, List<FileSystemInfo>? results = null) {
        results ??= new List<FileSystemInfo>();

        // get all subdirectories
        var subDirectories = Directory.GetDirectories(root.FullName);

        foreach (var directory in subDirectories) {
            GetFiles(new DirectoryInfo(directory), results);
        }

        // get all files in the root directory
        var files = Directory.GetFiles(root.FullName);

        foreach (var file in files) {
            results.Add(new FileInfo(file));
        }

        return results;
    }

    private void ProtectVmt(FileSystemInfo fileInfo) {
        if (fileInfo.Extension != ".vmt") {
            return;
        }

        var vmt = new Vmt(fileInfo);
        var vmtComments = new VmtComments(vmt);

        if (RemoveExistingComments) {
            vmtComments.RemoveAllComments();
        }

        vmtComments.AddComment(Comment);

        File.WriteAllBytes(fileInfo.FullName, vmt.GetBytes());
    }

    private void ProtectVtf(FileSystemInfo fileInfo) {
        if (fileInfo.Extension != ".vtf") {
            return;
        }

        var vtf = new Vtf(fileInfo);
        var vtfComments = new VtfComments(vtf);

        vtfComments.AddComment(Comment);

        File.WriteAllBytes(fileInfo.FullName, vtf.GetBytes());
    }
}