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
        FileContext db = new FileContext();

        private string ButtonTextToAction(string action)
        {
            switch (action)
            {
                case "Зашифровка": return "Cryption"; 
                case "Расшифровка": return "Decryption"; 
                case "Найти ключ": return "KeySearch";
            }
            return "";
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, string text, string cryptionKey, string decryptionKey, string action)
        {
            if (file == null && text.Length < 1)
                return RedirectToAction("Index", "Home");

            action = ButtonTextToAction(action);
            string key = action == "Cryption" ? cryptionKey : decryptionKey;


            string fileName;
            int id;
            if (file != null)
            {
                fileName = file.FileName;
                if (file.FileName.EndsWith(".txt"))
                {
                    text = TextService.GetTextFromTxt(file.InputStream);
                }
                else
                    text = TextService.GetTextFromDocx(file.InputStream, this);
                id = db.Add(file.FileName, text);
            }
            else
                id = db.Add(text);

            var cookie = new HttpCookie("FileID", id.ToString());
            Response.Cookies.Add(cookie);

            if (!"KeySearch".Equals(action))
            {
                cookie = new HttpCookie("Key", key);
                Response.Cookies.Add(cookie);
            }

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