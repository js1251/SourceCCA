using core.vmt;
using core.vtf;

namespace core;

public class ContentProtector {
    public bool RemoveExistingComments { get; init; }
    public bool AddNoToolTexture { get; init; }

    public string[] Comment {
        get => _comment;
        init {
            _comment = new string[_watermark.Length + value.Length];
            _watermark.CopyTo(_comment, 0);
            value.CopyTo(_comment, _watermark.Length);
        }
    }

    private readonly string[] _comment;
    private readonly DirectoryInfo _rootDirInfo;

    private readonly string[] _watermark = {
        "================================",
        "= Watermark added by SourceCCA =",
        "=   Created by Jakob Sailer    =",
        "=    Visit jakobsailer.com     =",
        "================================"
    };

    public ContentProtector(DirectoryInfo dirInfo) {
        _rootDirInfo = dirInfo;
        _comment = Array.Empty<string>();

        if (!Directory.Exists(dirInfo.FullName)) {
            throw new ArgumentException("Directory does not exist", nameof(dirInfo));
        }
    }

    public void Protect() {
        var files = GetFiles(_rootDirInfo);
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

        if (AddNoToolTexture) {
            var vmtNoToolTextureAdder = new VmtNoToolTextureAdder(vmt);
            vmtNoToolTextureAdder.AddNoToolTexture();
        }

        var vmtCommentsRemover = new VmtCommentsRemover(vmt);
        if (RemoveExistingComments) {
            vmtCommentsRemover.RemoveAllComments();
        }

        var vmtCommentsAdder = new VmtCommentsAdder(vmt);
        vmtCommentsAdder.AddComment(Comment);

        WriteFile(fileInfo, vmt.GetBytes());
    }

    private void ProtectVtf(FileSystemInfo fileInfo) {
        if (fileInfo.Extension != ".vtf") {
            return;
        }

        var vtf = new Vtf(fileInfo);
        var vtfComments = new VtfComments(vtf);

        vtfComments.AddComment(Comment);

        WriteFile(fileInfo, vtf.GetBytes());
    }

    private void WriteFile(FileSystemInfo fileInfo, byte[] bytes) {
        // take the root directory and make a copy with an _copyright suffix next to it

        var newDir = new DirectoryInfo(Path.Combine(_rootDirInfo.Parent!.FullName, _rootDirInfo.Name + "_protected"));

        if (!Directory.Exists(newDir.FullName)) {
            Directory.CreateDirectory(newDir.FullName);
        }

        // get the new file location (trim the prefix until the root directory)

        var newFilePath = fileInfo.FullName.Replace(_rootDirInfo.FullName, newDir.FullName);

        // create the directory if it doesn't exist

        var newFileDir = new DirectoryInfo(Path.GetDirectoryName(newFilePath)!);

        if (!Directory.Exists(newFileDir.FullName)) {
            Directory.CreateDirectory(newFileDir.FullName);
        }

        File.WriteAllBytes(newFilePath, bytes);
    }
}