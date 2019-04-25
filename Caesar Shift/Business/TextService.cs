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
        public static string GetTextFromTxt(Stream stream)
        {
            return new StreamReader(stream).ReadToEnd();
        }

        public static string GetTextFromDocx(Stream stream)
        {
            var wholeDocument = new StringBuilder();
            var document = new XWPFDocument(stream);
            foreach (XWPFParagraph item in document.Paragraphs)
                wholeDocument.AppendLine(item.Text);
            // Удаляем последнюю новую строку и старый формат новой строки
            return wholeDocument.Replace("\r", "").Remove(wholeDocument.Length - 1, 1).ToString();
        }

        public static byte[] GetTxtFileWithText(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static byte[] GetDocFileWithText(string text)
        {
            var document = new XWPFDocument();
            foreach (string item in text.Split('\n'))
            {
                var paragraph = document.CreateParagraph();
                var run = paragraph.CreateRun();
                run.SetText(item);
            }


            var stream = new MemoryStream();
            document.Write(stream);

            return stream.ToArray();
        }


    }
}