using Caesar_Shift.Business;
using Caesar_Shift.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Caesar_Shift.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string text, string shift, string action)
        {
            // Update page, if nothing is loaded
            if (file == null && text.Length < 1)
                return RedirectToAction("Index", "Home");

            switch(action)
            {
                case "Зашифровка": action = "Cryption"; break;
                case "Расшифровка": action = "Decryption"; break;
                case "Найти ключ": action = "KeySearch"; break;
            }

            string fileName;

            if (file != null)
            {
                fileName = file.FileName;
                if (file.FileName.EndsWith(".txt"))
                    text = TextExtracter.GetTextFromTxt(file.InputStream);
                else
                    text = TextExtracter.GetTextFromDocx(file.InputStream);
            }
            else
            {
                fileName = "Untiled.txt";
            }



            Response.Cookies.Clear();

            var cookie = new HttpCookie("FileID", );
            Response.Cookies.Add(cookie);
            return RedirectToAction(action, "Crypt");
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