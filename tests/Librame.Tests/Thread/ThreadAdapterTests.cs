using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Thread;
using Librame.Utility;
using System.IO;

namespace Librame.Tests.Thread
{
    [TestClass()]
    public class ThreadAdapterTests
    {
        private readonly IThreadAdapter _adapter;

        public ThreadAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.Thread;
        }


        [TestMethod()]
        public void AddCopyFileByNewThreadTest()
        {
            var fromFile = PathUtility.BinDirectory.AppendPath("Librame.xml");
            var toFile = TestHelper.DefaultDirectory.AppendPath("Librame.xml.bak");

            var result = _adapter.AddCopyFile(fromFile, toFile);
            Assert.IsNotNull(result);
            //Assert.IsTrue(result.IsCompleted);
            
            //Assert.IsTrue(File.Exists(toFile));
            //File.Delete(toFile);
        }

    }
}