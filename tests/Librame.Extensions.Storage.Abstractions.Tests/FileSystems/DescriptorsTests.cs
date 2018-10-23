using System;
using System.IO;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class DescriptorsTests
    {

        [Fact]
        public void DescriptorsTest()
        {
            var driveInfo = new DriveInfo(@"C:\");
            if (driveInfo.IsReady)
            {
                var drive = new DriverDescriptor(driveInfo);
                Assert.NotNull(drive);
            }

            var dir = new DirectoryDescriptor(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
            Assert.NotNull(dir);

            var file = new FileDescriptor(new FileInfo(@"C:\Users\Administrator\NTUSER.DAT"));
            Assert.NotNull(file);

            var platform = new PlatformDescriptor();
            Assert.NotNull(platform);
        }

    }
}
