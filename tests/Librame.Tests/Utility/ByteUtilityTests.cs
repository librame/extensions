using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Authorization;
using Librame.Authorization.Descriptors;
using Librame.Utility;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class ByteUtilityTests
    {
        [TestMethod()]
        public void ByteConvertTest()
        {
            // 必须在类名加 [StructLayout(LayoutKind.Sequential)] 属性特殊，否则会抛出异常
            // 此方法比 [Serializable] 序列化方法生成的字节数组短了很多，非常节省空间
            var ticket = new AuthenticateTicket()
            {
                Account = new AccountDescriptor("Test Name", "123456", AccountStatus.Active)
            };

            var buffer = ticket.AsBytes();
            Assert.IsTrue(buffer.Length > 0);

            var resolve = buffer.FromBytes<AuthenticateTicket>();
            Assert.IsNotNull(resolve);
            Assert.AreEqual(ticket?.Account.Name, resolve?.Account.Name);
        }

    }

}
