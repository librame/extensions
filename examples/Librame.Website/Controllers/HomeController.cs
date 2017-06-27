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
        //private readonly IList<Prefecture> _prefectures = null;

        //public HomeController()
        //{
        //    _prefectures = new List<Prefecture>()
        //    {
        //        new Prefecture() { Id = 1, ParentId = 0, Name = "北京市" },
        //        new Prefecture() { Id = 2, ParentId = 0, Name = "上海市" },
        //        new Prefecture() { Id = 3, ParentId = 0, Name = "天津市" },
        //        new Prefecture() { Id = 4, ParentId = 0, Name = "重庆市" },
        //        new Prefecture() { Id = 5, ParentId = 4, Name = "渝中区" },
        //        new Prefecture() { Id = 6, ParentId = 4, Name = "渝北区" },
        //        new Prefecture() { Id = 7, ParentId = 4, Name = "沙坪坝区" },
        //        new Prefecture() { Id = 8, ParentId = 4, Name = "九龙坡区" },
        //        new Prefecture() { Id = 9, ParentId = 4, Name = "江津区" },
        //        new Prefecture() { Id = 10, ParentId = 4, Name = "永川区" },
        //        new Prefecture() { Id = 11, ParentId = 10, Name = "胜利路" },
        //        new Prefecture() { Id = 12, ParentId = 10, Name = "中山路" }
        //    };
        //}


        public ActionResult Index()
        {
            //var selectList = _prefectures.AsTreeSelectListItems(v => v.Id.ToString(),
            //    t => t.Name, (v, t) => v == "10");
            
            var name = "test name";
            var strategy = LibrameArchitecture.Adapters.Authorization.Strategy;
            
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