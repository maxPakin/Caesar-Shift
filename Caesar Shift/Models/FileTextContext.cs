using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Models
{
    public class FileTextContext : DbContext
    {
        public DbSet<FileText> FileTexts { get; set; }


        public int Add(string text)
        {
            return Add("Untiled.txt", text);
        }

        public int Add(string name, string text)
        {
            var file = new FileText() { Name = name, Text = text };
            FileTexts.Add(file);
            SaveChanges();
            return file.Id;
        }

    }
}