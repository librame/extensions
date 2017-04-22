using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Container;

namespace Librame.UnitTests.Container
{
    [TestClass()]
    public class ContainerAdapterTests
    {
        private readonly IContainerAdapter _adapter = null;

        public ContainerAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.ContainerAdapter;
        }
        

        [TestMethod()]
        public void BuildContainerTest()
        {
            Assert.IsNotNull(_adapter.BuildContainer());
        }

    }
}
