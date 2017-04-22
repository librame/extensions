using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Librame.Utility;

namespace Librame.UnitTests.Utility
{
    [TestClass()]
    public class ExceptionUtilityTests
    {
        
    }


    [TestClass()]
    public class ExceptionUtilityExtensionsTests
    {
        [TestMethod()]
        public void GuardArgumentNullTest()
        {
            try
            {
                object obj = null;

                obj.GuardNull(nameof(obj));
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod()]
        public void GuardArgumentNullOrEmptyTest()
        {
            try
            {
                string str = string.Empty;

                str.GuardNullOrEmpty(nameof(str));
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod()]
        public void GuardFileNotFoundTest()
        {
            try
            {
                string file = "c:\\test.txt";

                file.GuardFileNotFound();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod()]
        public void GuardAssignableFromTest()
        {
            try
            {
                var baseType = typeof(Adaptation.IAdapterManager);

                baseType.GuardAssignableFrom(typeof(Adaptation.AdapterManager));
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

    }
}
