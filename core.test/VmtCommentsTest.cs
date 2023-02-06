using core.vmt;
using Gameloop.Vdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core.test;

[TestClass]
public class VmtCommentsTest {
    [TestMethod]
    [DeploymentItem(@"Resources\")]
    public void RemoveAllComments() {
        // get all files in folder root folder
        var fileNames = Directory.GetFiles(@"vmts");

        foreach (var fileName in fileNames) {
            System.Diagnostics.Debug.WriteLine($"===== {fileName} before =====\n{File.ReadAllText(fileName)}");

            var vmt = new Vmt(new FileInfo(fileName));
            var vmtComments = new VmtComments(vmt);
            vmtComments.RemoveAllComments();

            var vmtContent = VdfConvert.Serialize(vmt.Vdf);

            Assert.IsFalse(vmtContent.Contains("//"), $"VMT file contains \"//\"\n{vmtContent}");
            Assert.IsFalse(vmtContent.Contains("/*"), $"VMT file contains \"/*\"\n{vmtContent}");
            Assert.IsFalse(vmtContent.Contains("*/"), $"VMT file contains \"*/\"\n{vmtContent}");

            System.Diagnostics.Debug.WriteLine($"===== {fileName} after =====\n{vmtContent}\n\n");
        }
    }

    [TestMethod]
    [DeploymentItem(@"Resources\")]
    public void AddSingleComment() {
        // get all files in folder root folder
        var fileNames = Directory.GetFiles(@"vmts");

        const string singleComment = "This is a test comment";

        foreach (var fileName in fileNames) {
            System.Diagnostics.Debug.WriteLine($"===== {fileName} before =====\n{File.ReadAllText(fileName)}");

            var vmt = new Vmt(new FileInfo(fileName));
            var vmtComments = new VmtComments(vmt);
            vmtComments.AddComment(singleComment);

            var vmtContent = VdfConvert.Serialize(vmt.Vdf);

            Assert.IsTrue(vmtContent.Contains($"//{singleComment}"), $"VMT file does not contain comment\n{vmtContent}");

            System.Diagnostics.Debug.WriteLine($"===== {fileName} after =====\n{vmtContent}\n\n");
        }
    }

    [TestMethod]
    [DeploymentItem(@"Resources\")]
    public void AddMultiLineComment() {
        // get all files in folder root folder
        var fileNames = Directory.GetFiles(@"vmts");

        var multiLineComment = new[] {
            "This is a test comment",
            "With multiple lines.",
            "Here is another line."
        };

        foreach (var fileName in fileNames) {
            System.Diagnostics.Debug.WriteLine($"===== {fileName} before =====\n{File.ReadAllText(fileName)}");

            var vmt = new Vmt(new FileInfo(fileName));
            var vmtComments = new VmtComments(vmt);
            vmtComments.AddComment(multiLineComment);

            var vmtContent = VdfConvert.Serialize(vmt.Vdf);

            foreach (var comment in multiLineComment) {
                Assert.IsTrue(vmtContent.Contains($"//{comment}"), $"VMT file does not contain comment\n{vmtContent}");
            }

            System.Diagnostics.Debug.WriteLine($"===== {fileName} after =====\n{vmtContent}\n\n");
        }
    }
}