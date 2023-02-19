using core.vmt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core.test;

[TestClass]
public class VmtNoToolTextureTest {
    [TestMethod]
    [DeploymentItem(@"Resources\")]
    public void Test() {
        // get all files in folder root folder
        var fileNames = Directory.GetFiles(@"vmts");

        foreach (var fileName in fileNames) {
            var paramString = "%notooltexture";

            System.Diagnostics.Debug.WriteLine($"===== {fileName} before =====\n{File.ReadAllText(fileName)}");

            var vmt = new Vmt(new FileInfo(fileName));

            var noToolTextureAdder = new VmtNoToolTextureAdder(vmt);
            noToolTextureAdder.AddNoToolTexture();

            var vmtContent = vmt.ToString();
            
            System.Diagnostics.Debug.WriteLine($"===== {fileName} after =====\n{vmtContent}\n\n");

            Assert.IsTrue(vmtContent.ToLower().Contains(paramString), $"VMT file does not contain {paramString}");
            // check that its not contained more than once
            var count = vmtContent.ToLower().Split(paramString).Length - 1;
            Assert.AreEqual(1, count, $"VMT file contains {paramString} more than once\n{vmtContent}");

        }
    }
}