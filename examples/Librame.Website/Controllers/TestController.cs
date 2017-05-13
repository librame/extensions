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
        private readonly IRepository<Prefecture> _repository = null;

        public TestController(IRepository<Prefecture> repository)
        {
            _repository = repository.NotNull(nameof(repository));
        }


        // GET: Test
        public ActionResult Index()
        {
            ViewBag.Paging = _repository.GetPagingByIndex(1, 10, (order) => order.Asc(p => p.Id));

            return View();
        }

    }
}