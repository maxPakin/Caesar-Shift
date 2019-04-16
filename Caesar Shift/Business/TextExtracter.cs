using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Business
{
    public static class TextExtracter
    {
        public static string GetTextFromTxt(Stream stream)
        {
            return new StreamReader(stream).ReadToEnd();
        }

        public static string GetTextFromDocx(Stream stream)
        {
            return "";
        }
    }
}