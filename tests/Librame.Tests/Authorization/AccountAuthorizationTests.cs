using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Authorization;
using Librame.Utility;

namespace Librame.Tests.Authorization
{
    [TestClass()]
    public class AccountAuthorizationTests
    {
        private const string _passwd = "123456";

        private IAuthorizeAdapter _adapter = null;

        public AccountAuthorizationTests()
        {
            _adapter = LibrameArchitecture.Adapters.Authorization;
        }

        [TestMethod()]
        public void AuthenticateTest()
        {
            var name = "test name";
            
            // SignIn
            var authInfo = _adapter.Strategy.SignIn(name, _passwd, true, (k, p) =>
            {
                System.Threading.Thread.CurrentPrincipal = p;
            });

            var user = System.Threading.Thread.CurrentPrincipal;
            Assert.IsTrue(user?.Identity?.Name == name);

            // SignOut
            _adapter.Strategy.SignOut((k) =>
            {
                var ticket = System.Threading.Thread.CurrentPrincipal.AsTicket();
                System.Threading.Thread.CurrentPrincipal = null;

                return ticket;
            });

            user = System.Threading.Thread.CurrentPrincipal;
            Assert.IsFalse(user.Identity.IsAuthenticated);
        }

        [TestMethod()]
        public void EncryptTicketTest()
        {
            var name = "TestAccount";

            // SignIn
            var authInfo = _adapter.Strategy.SignIn(name, _passwd, true, (k, p) =>
            {
                System.Threading.Thread.CurrentPrincipal = p;
            });
            Assert.AreEqual(authInfo.Status, AuthenticateStatus.Success);

            var ticket = (System.Threading.Thread.CurrentPrincipal.Identity as AccountIdentity).Ticket;

            // 加密票根
            //var encrypt = _adapter.Strategy.EncryptTicket(ticket);
            var json = ticket.AsJson();
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var encrypt = System.Convert.ToBase64String(bytes);

            Assert.IsTrue(!string.IsNullOrEmpty(encrypt));
        }

        [TestMethod()]
        public void PasswdTest()
        {
            var passwdManager = _adapter.Managers.Passwd;
            var encodePasswd = passwdManager.Encode(_passwd);
            
            // EncodePasswd vs RawPasswd
            Assert.IsTrue(passwdManager.EncodeRawEquals(encodePasswd, _passwd));
            
            // Both EncodePasswd
            Assert.IsTrue(passwdManager.EncodeEquals(encodePasswd, passwdManager.Encode("123456")));

            // Both RawPasswd
            Assert.AreEqual(_passwd, "123456");
        }

    }
}
