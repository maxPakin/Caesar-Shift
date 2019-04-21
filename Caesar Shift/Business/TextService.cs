using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Caesar_Shift.Business
{
    public static class TextService
    {
        private static string GetTempDocxPath(Controller controll)
        {
            return controll.Server.MapPath("~/Business/" + DateTime.Now.ToFileTime());
        }

        public static string GetTextFromTxt(Stream stream)
        {
            return new StreamReader(stream).ReadToEnd();
        }

        public static string GetTextFromDocx(Stream stream, Controller controll)
        {
            StringBuilder wholeDocument = new StringBuilder();
            XWPFDocument document = new XWPFDocument(stream);
            foreach (var item in document.Paragraphs)
                wholeDocument.AppendLine(item.Text);
            return wholeDocument.ToString();
        }

        public static byte[] GetTxtFileWithText(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static byte[] GetDocFileWithText(string text)
        {
            XWPFDocument document = new XWPFDocument();
            foreach (var item in text.Split('\n'))
            {
                var paragraph = document.CreateParagraph();
                var run = paragraph.CreateRun();
                run.SetText(item);
            }


            MemoryStream stream = new MemoryStream();
            document.Write(stream);

            return stream.ToArray();
        }


    }
}