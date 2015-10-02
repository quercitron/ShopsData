using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopsData.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CurrentData(int locationId, int productTypeId)
        {
            return PartialView();
        }

        public ActionResult ProductDetails(int locationId, int productId)
        {
            return PartialView();
        }
    }
}