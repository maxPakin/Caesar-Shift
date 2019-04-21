using Caesar_Shift.Business;
using Caesar_Shift.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Caesar_Shift.Controllers
{
    public class CryptController : Controller
    {
        FileContext db = new FileContext();

        public object FileService { get; private set; }

        private CryptionData GetCryptionData()
        {
            var data = new CryptionData();
            HttpCookie cookie = Request.Cookies.Get("FileID");
            if (cookie == null)
                return CryptionData.None;

            if (!int.TryParse(cookie.Value, out int fileId))
                    return CryptionData.None;

            var file = db.GetById(fileId);
            if (file == null)
                return CryptionData.None;

            data.File = file;

            cookie = Request.Cookies.Get("Key");

            if (int.TryParse(cookie.Value, out int key))
            {
                data.Shift = key;
            }

            return data;
        }

        public ActionResult Cryption()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                return RedirectToAction("Index", "Home");

            ViewBag.Key = data.Shift;
            ViewBag.Text = Caesar.Shift(data.File.Text, data.Shift);;

            return View();
        }

        public ActionResult Decryption()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                return RedirectToAction("Index", "Home");

            ViewBag.Key = data.Shift;
            ViewBag.Text = Caesar.Shift(data.File.Text, -data.Shift); ;

            return View();
        }

        public ActionResult KeySearch()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                return RedirectToAction("Index", "Home");
            
            ViewBag.Keys = Caesar.GetBestKeys(data.File.Text);
            ViewBag.Text = data.File.Text;

            return View();
        }

        public ActionResult Download(string shift)
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                return RedirectToAction("Index", "Home");
            
            int iShift = int.Parse(shift);
            byte[] bytes;
            string appType;
            string text;
            if (data.File.Name.EndsWith(".txt"))
            {
                appType = "text/plain";
                text = Caesar.Shift(data.File.Text, iShift);
                bytes = TextService.GetTxtFileWithText(text);
            }
            else
            {
                appType = "application/msword";
                text = Caesar.Shift(data.File.Text, iShift);
                bytes = TextService.GetDocFileWithText(text);
            }


            return File(bytes, appType, data.File.Name);
        }

    }
}