using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class ProcessExtensionsTests
    {
        [Fact]
        public void StartLocateInExplorerTest()
        {
            var fileName = @"c:\test.txt";
            File.WriteAllText(fileName, nameof(StartLocateInExplorerTest));

            var explorer = fileName.StartLocateInExplorer();
            Assert.False(explorer.HasExited);

            explorer.Close();
            File.Delete(fileName);
        }

        [Fact]
        public void StartProcessTest()
        {
            var cmd = "cmd.exe".StartProcess(startInfoAction: info =>
            {
                info.UseShellExecute = true;
            });
            Assert.False(cmd.HasExited);

            cmd.WaitForExit();
            cmd.Close();
        }

    }
}
