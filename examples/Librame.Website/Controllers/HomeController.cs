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
            var strategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;
            
            strategy.SignIn(name, string.Empty, true,
                (k, p) => Session.Add(k, p));

            var user1 = User;
            var user2 = Session.ResolvePrincipal();

            if (user2.Identity.IsAuthenticated)
                return RedirectToAction("About");
            
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
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}