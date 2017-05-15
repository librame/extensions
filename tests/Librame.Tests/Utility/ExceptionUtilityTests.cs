using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Librame.Utility;

namespace Librame.Tests.Utility
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

                obj.NotNull(nameof(obj));
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

                str.NotNullOrEmpty(nameof(str));
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

                file.FileExists();
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        [TestMethod()]
        public void AssignableFromTest()
        {
            try
            {
                var baseType = typeof(Adaptation.IAdapterManager);

                baseType.AssignableFrom(typeof(Adaptation.AdapterManager));
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

    }
}
