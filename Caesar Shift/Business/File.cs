using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Business
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}