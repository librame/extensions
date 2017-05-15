using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.WinService;

namespace Librame.UnitTests.WinService
{
    [TestClass()]
    public class WinServiceAdapterTests
    {
        private readonly IWinServiceAdapter _adapter;

        public WinServiceAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.WinService;
        }


        [TestMethod()]
        public void AddWinServiceTest()
        {
            _adapter.AddService<TestServiceControl>("LibrameService", "天平服务", "天平架构服务");
        }

    }
}