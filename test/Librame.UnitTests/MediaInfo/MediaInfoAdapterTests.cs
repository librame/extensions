using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.UnitTests.MediaInfo
{
    [TestClass()]
    public class MediaInfoAdapterTests
    {
        [TestMethod()]
        public void MediaInfoAnalyzeTest()
        {
            var adapter = LibrameArchitecture.AdapterManager.MediaInfoAdapter;
            var info = adapter.Analyze(@"D:\Temp\test.mp4");
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.Video);
            Assert.IsNotNull(info.Audio);
        }

    }
}