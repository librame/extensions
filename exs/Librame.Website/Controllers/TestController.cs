using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Librame.Website.Controllers
{
    using Data;
    using Utility;

    public class TestController : Controller
    {
        private readonly IService<Prefecture> _service = null;

        public TestController(IService<Prefecture> service)
        {
            _service = service.NotNull(nameof(service));
        }


        // GET: Test
        public ActionResult Index()
        {
            ViewBag.Paging = _service.Repository.GetPagingByIndex(1, 10, (order) => order.Asc(p => p.Id));

            return View();
        }

    }
}