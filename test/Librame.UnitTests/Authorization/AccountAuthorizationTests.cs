using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Authorization;

namespace Librame.UnitTests.Authorization
{
    [TestClass()]
    public class AccountAuthorizationTests
    {
        private IAuthorizeAdapter _adapter = null;

        public AccountAuthorizationTests()
        {
            _adapter = LibrameArchitecture.AdapterManager.AuthorizationAdapter;
        }

        [TestMethod()]
        public void AuthenticateTest()
        {
            var name = "test name";
            var passwd = "test password";
            
            // SignIn
            var authInfo = _adapter.Strategy.SignIn(name, passwd, true,
                (k, p) => System.Threading.Thread.CurrentPrincipal = p);

            var user = System.Threading.Thread.CurrentPrincipal;
            Assert.IsTrue(user?.Identity?.Name == name);

            // SignOut
            _adapter.Strategy.SignOut((k) => System.Threading.Thread.CurrentPrincipal = null);

            user = System.Threading.Thread.CurrentPrincipal;
            Assert.IsFalse(user.Identity.IsAuthenticated);
        }

        [TestMethod()]
        public void PasswdTest()
        {
            var passwd = _adapter.Passwd;

            var test = "123456";
            var testPasswd = passwd.Encode(test);

            // False (Test is not encode string)
            var result = passwd.Equals(testPasswd, test);
            Assert.IsFalse(result);

            // True
            result = passwd.RawEquals(testPasswd, test);
            Assert.IsTrue(result);
        }

    }
}
