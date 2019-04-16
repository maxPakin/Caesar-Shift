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
        private CryptionData GetCryptionData()
        {
            HttpCookie cookie = Request.Cookies.Get("CryptionData");
            if (cookie == null)
                return CryptionData.None;
            
            if (!int.TryParse(cookie["Shift"], out int shift))
                    return CryptionData.None;

            string fileName;
            if ((fileName = cookie["FileName"]) == null)
                return CryptionData.None;

            string fileText;
            if ((fileText = cookie["FileText"]) == null)
                return CryptionData.None;

            var arr = Encoding.Unicode.GetBytes(fileText);
            fileText = Encoding.UTF8.GetString(arr);

            return new CryptionData()
            {
                Shift = shift,
                File = new FileText()
                {
                    Name = fileName,
                    Text = fileText
                }
            };
        }

        public ActionResult Cryption()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                return RedirectToAction("Index", "Home");

            int shift = data.Shift;
            ViewBag.Key = data.Shift;

            string text = data.File.Text;
            text = Caesar.Shift(text, shift);

            ViewBag.Text = text;

            return View();
        }

        public ActionResult Decryption()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                RedirectToAction("Index", "Home");

            ViewBag.Key = data.Shift;

            return View();
        }

        public ActionResult KeySearch()
        {
            var data = GetCryptionData();
            if (data == CryptionData.None)
                RedirectToAction("Index", "Home");

            return View();
        }
    }
}