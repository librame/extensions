using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Socket;

namespace Librame.Tests.Socket
{
    [TestClass()]
    public class SocketAdapterTests
    {
        private readonly ISocketAdapter _adapter;

        public SocketAdapterTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.Socket;
        }


        [TestMethod()]
        public void RequestTest()
        {
            Assert.IsNotNull(_adapter);

            //// 服务端：监听本机80端口
            //_adapter.ListeningServer(80);

            //// 客户端：发起连接
            //_adapter.ConnectedServer(80);
        }

    }
}