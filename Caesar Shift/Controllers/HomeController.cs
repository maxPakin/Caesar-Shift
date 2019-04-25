using Caesar_Shift.Business;
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

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        private string GetNeededText(HttpPostedFileBase file, string text)
        {
            if (file != null)
            {
                var fileName = file.FileName;

                if (fileName.EndsWith(".txt"))
                {
                    return TextService.GetTextFromTxt(file.InputStream);
                }

                if (fileName.EndsWith(".docx"))
                {
                    return TextService.GetTextFromDocx(file.InputStream);
                }
            }
            else if (!string.IsNullOrEmpty(text))
            {
                return text;
            }

            return null;
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Encryption")]
        public ActionResult Cryption(HttpPostedFileBase file, string text, int encryptKey)
        {
            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("Index");
            string fileName = file?.FileName ?? "Untitled.txt";

            ViewBag.Text = CaesarEncoder.Encryption(text, encryptKey);
            ViewBag.FileName = fileName;
            ViewBag.Key = encryptKey;

            return View("Encryption");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Decryption")]
        public ActionResult Decryption(HttpPostedFileBase file, string text, int decryptKey)
        {
            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("Index");
            string fileName = file?.FileName ?? "Untitled.txt";

            ViewBag.Text = CaesarEncoder.Decryption(text, decryptKey);
            ViewBag.FileName = fileName;
            ViewBag.Key = decryptKey;

            return View("Decryption");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "KeySearch")]
        public ActionResult KeySearch(HttpPostedFileBase file, string text)
        {
            text = GetNeededText(file, text);
            if (text == null) return RedirectToAction("Index");
            string fileName = file?.FileName ?? "Untitled.txt";

            ViewBag.Keys = CaesarEncoder.GetBestKeys(text);
            ViewBag.FileName = fileName;

            return View("KeySearch");
        }

        public ActionResult Download(string text, string fileName)
        {
            string appType;
            byte[] bytes;
            if (fileName.EndsWith(".txt"))
            {
                appType = "text/plain";
                bytes = TextService.GetTxtFileWithText(text);
            }
            else if (fileName.EndsWith(".docx"))
            {
                appType = "application/msword";
                bytes = TextService.GetDocFileWithText(text);
            }
            else
            {
                return null;
            }

            return File(bytes, appType, fileName);
        }
    }
}