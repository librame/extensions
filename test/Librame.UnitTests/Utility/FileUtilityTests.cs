using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class FileUtilityTests
    {
        [TestMethod()]
        public void WriteAndReadContentTest()
        {
            try
            {
                string content = "write test";
                string file = "c:\\text.txt";

                FileUtility.WriteContent(content, file, Encoding.UTF8);

                string newContent = FileUtility.ReadContent(file, Encoding.UTF8);
                Assert.AreEqual(content, newContent, true);

                System.IO.File.Delete(file);
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }
    }


    [TestClass()]
    public class FileUtilityExtensionsTests
    {

    }
}
