using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class PathUtilityTests
    {
        #region Directory

        [TestMethod()]
        public void BaseDirectoryTest()
        {
            Assert.IsFalse(string.IsNullOrEmpty(PathUtility.BaseDirectory));
        }

        [TestMethod()]
        public void BinDirectoryTest()
        {
            Assert.IsFalse(string.IsNullOrEmpty(PathUtility.BinDirectory));
        }

        [TestMethod()]
        public void ConfigDirectoryTest()
        {
            Assert.IsFalse(string.IsNullOrEmpty(PathUtility.ConfigsDirectory));
        }

        [TestMethod()]
        public void IsWebEnvironmentTest()
        {
            Assert.IsFalse(PathUtility.IsWebEnvironment);
        }

        #endregion


        [TestMethod()]
        public void CombinePathTest()
        {
            var path1 = "c:\\Program Files";
            var path2 = "test.txt";
            var path = PathUtility.CombinePath(path1, path2);
            Assert.IsFalse(string.IsNullOrEmpty(path));
        }

    }


    [TestClass()]
    public class PathUtilityExtensionsTests
    {
        [TestMethod()]
        public void AppendPathTest()
        {
            var path = "c:\\Program Files".AppendPath("test.txt");
            Assert.IsFalse(string.IsNullOrEmpty(path));
        }

        [TestMethod()]
        public void CreateDirectoryTest()
        {
            var path = "c:\\Program Files\\test.txt";
            Assert.IsFalse(string.IsNullOrEmpty(path.CreateDirectoryName()));
        }

    }
}
