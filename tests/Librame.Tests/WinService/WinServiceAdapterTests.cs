using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.WinService;

namespace Librame.Tests.WinService
{
    [TestClass()]
    public class WinServiceAdapterTests
    {
        private readonly IWinServiceAdapter _adapter;

        public WinServiceAdapterTests()
        {
            _adapter = LibrameArchitecture.Adapters.WinService;
        }


        [TestMethod()]
        public void AddWinServiceTest()
        {
            _adapter.AddService<TestServiceControl>("LibrameService", "天平服务", "天平架构服务");
        }

    }
}