namespace core.vtf;

internal class Vtf {
    public byte[] Bytes { get; set; }

    public Vtf(FileSystemInfo vmtFileInfo) {
        Bytes = File.ReadAllBytes(vmtFileInfo.FullName);
    }

    public byte[] GetBytes() {
        return Bytes;
    }
}