using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Caesar_Shift.Business
{
    public class FileContext : DbContext
    {
        public DbSet<File> Files { get; set; }

        

    }
}