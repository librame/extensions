using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Storage.Tests
{
    public class InternalFileSystemServiceTests
    {
        private IFileSystemService _fileSystem;

        public InternalFileSystemServiceTests()
        {
            _fileSystem = TestServiceProvider.Current.GetRequiredService<IFileSystemService>();
        }


        [Fact]
        public void LoadDriversTest()
        {
            var drivers = _fileSystem.LoadDrivers();
            Assert.NotEmpty(drivers);
        }


        [Fact]
        public void LoadDirectoriesTest()
        {
            var dirs = _fileSystem.LoadDirectories("*.*");
            Assert.NotEmpty(dirs);
        }

    }
}
