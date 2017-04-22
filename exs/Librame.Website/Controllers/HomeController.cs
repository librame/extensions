using Librame.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Librame.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var name = "test name";
            var strategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;
            
            strategy.SignIn(name, string.Empty, true,
                (k, p) => Session.Add(k, p));

            var user1 = User;
            var user2 = Session.ResolvePrincipal();

            if (user2.Identity.IsAuthenticated)
                return RedirectToAction("About");

            //strategy.SignOut(name);
            
            //validated = user.Identity.IsAuthenticated;

            return View();
        }

        public ActionResult About()
        {
            var user1 = User;
            var user2 = Session.ResolvePrincipal();
            
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            // 格式化服务器 URL
            var auth = Archit.Adapters.AuthorizationAdapter;

            var serverUrl = AuthorizeHelper.FormatSsoServerSignInUrl(auth.AuthSettings.SsoServerSignInUrl,
                auth.Settings.AuthId, Request.UrlReferrer.ToString());
            
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}