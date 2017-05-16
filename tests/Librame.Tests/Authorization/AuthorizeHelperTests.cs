using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Librame.Tests.Authorization
{
    [TestClass()]
    public class AuthorizeHelperTests
    {
        [TestMethod()]
        public void GenerateTokenTest()
        {
            var token = LibrameArchitecture.Adapters.Authorization.Managers.Token.Generate();
            Assert.IsNotNull(token);
        }

    }
}
