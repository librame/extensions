using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Authorization;
using Librame.Utility;
using System;

namespace Librame.Tests.Utility
{
    [TestClass()]
    public class ByteUtilityTests
    {
        [TestMethod()]
        public void AsOrFromBytesTest()
        {
            // 必须在类名加 [StructLayout(LayoutKind.Sequential)] 属性特殊，否则会抛出异常
            // 此方法比 [Serializable] 序列化方法生成的字节数组短了很多，非常节省空间
            var ticket = new AuthenticateTicket(1, 1, "Test Name", "Test Token", DateTime.Now);

            var str = ticket.AsBytes().AsBase64();
            Assert.IsTrue(str.Length > 0);

            // 手动还原对象会出现 “尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
            //str = "AQAAAAEAAAA4eQ0FeHgNBcBQX+dN7+RAwFBf5y3v5EAAAAAAAQAAANhRsAAoU7AAAQAAAAAAAAA=";

            var resolve = str.FromBase64().FromBytes<AuthenticateTicket>();
            Assert.IsNotNull(resolve);
            Assert.AreEqual(ticket.Name, resolve.Name);
        }

    }

}
