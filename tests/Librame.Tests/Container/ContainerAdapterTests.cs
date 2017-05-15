using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Container;

namespace Librame.Tests.Container
{
    [TestClass()]
    public class ContainerAdapterTests
    {
        private readonly IContainerAdapter _adapter = null;

        public ContainerAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Container;
        }
        

        [TestMethod()]
        public void BuildContainerTest()
        {
            Assert.IsNotNull(_adapter.BuildContainer());
        }

    }
}
