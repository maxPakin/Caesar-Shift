using Caesar_Shift.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xceed.Words.NET;

namespace Caesar_Shift.Controllers
{
    public class HomeController : Controller
    {
        FileContext db = new FileContext();
        public ActionResult Index()
        {
            db.Files.
            return View();
        }

        [HttpPost]
        public string Upload(HttpPostedFileBase upload, string text, string action)
        {
            if (upload == null)
            {
                if ()
            }

            return action;
        }

        public ActionResult About()
        {
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