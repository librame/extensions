using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Authorization;

namespace Librame.UnitTests.Authorization
{
    [TestClass()]
    public class AuthorizeHelperTests
    {
        [TestMethod()]
        public void GenerateAuthIdTest()
        {
            var authId = AuthorizeHelper.GenerateAuthId();
            Assert.IsNotNull(authId);
        }
        
        [TestMethod()]
        public void GenerateTokenTest()
        {
            var token = AuthorizeHelper.GenerateToken();
            Assert.IsNotNull(token);
        }

    }
}
