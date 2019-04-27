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
            Encoding encoding = GetEncoding(stream);
            return new StreamReader(stream, encoding).ReadToEnd();
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

        private static Encoding GetEncoding(Stream stream)
        {
            var Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            var UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            var UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //with BOM

            var binaryReader = new BinaryReader(stream, Encoding.Default);
            int i = (int)stream.Length;
            byte[] bytes = binaryReader.ReadBytes(i);
            stream.Position = 0;
            if (IsUTF8Bytes(bytes) || bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                return Encoding.UTF8;
            }

            if (bytes[0] == 0xFE && bytes[1] == 0xFF && bytes[2] == 0x00)
            {
                return Encoding.BigEndianUnicode;
            }

            if (bytes[0] == 0xFF && bytes[1] == 0xFE && bytes[2] == 0x41)
            {
                return Encoding.Unicode;
            }

            return Encoding.Default;
        }

        // Some magic from StackOverflow...
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;
            byte currentByte;
            for (int i = 0; i < data.Length; i++)
            {
                currentByte = data[i];
                if (charByteCounter == 1)
                {
                    if (currentByte >= 0x80)
                    {
                        while (((currentByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }

                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if ((currentByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                return false;
            }
            return true;
        }
    }
}