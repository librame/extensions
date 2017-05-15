using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.MediaInfo;

namespace Librame.UnitTests.MediaInfo
{
    [TestClass()]
    public class MediaInfoAdapterTests
    {
        [TestMethod()]
        public void MediaInfoAnalyzeTest()
        {
            var adapter = LibrameArchitecture.AdapterManager.MediaInfo;
            var info = adapter.Analyze(@"E:\Temp\test.mp4");
            Assert.IsNotNull(info);
            Assert.IsNotNull(info.Video);
            Assert.IsNotNull(info.Audio);
        }


        [TestMethod()]
        public void MediaInfoExportTest()
        {
            var adapter = LibrameArchitecture.AdapterManager.MediaInfo;
            var info = adapter.Analyze(@"E:\Temp\Love.on.Delivery.1994.1080p.BluRay.x264-WiKi.Sample.mkv");
            Assert.IsNotNull(info);

            var fileName = @"E:\Temp\Love.on.Delivery.1994.1080p.BluRay.x264-WiKi.Sample.txt";
            MediaInfoHelper.Export(info, fileName, LibrameArchitecture.AdapterManager.Settings.Encoding);

            Assert.IsTrue(System.IO.File.Exists(fileName));
        }

    }
}