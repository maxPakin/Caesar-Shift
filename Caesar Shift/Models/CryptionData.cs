using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Models
{
    public class CryptionData
    {
        public static CryptionData None = new CryptionData();

        public int Shift { get; set; }
        public FileText File { get; set; }
    }
}