using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class TypeUtilityTests
    {
        [TestMethod()]
        public void GetNameAndAssemblyTest()
        {
            string str = TypeUtility.AssemblyQualifiedNameWithoutVCP<TypeUtilityTests>();

            Assert.IsNotNull(str);
        }

        [TestMethod()]
        public void GetAssignableTypesTest()
        {
            var managers = TypeUtility.GetAssignableTypes<Adaptation.IAdapterManager>();

            Assert.IsNotNull(managers);
        }
    }

}
