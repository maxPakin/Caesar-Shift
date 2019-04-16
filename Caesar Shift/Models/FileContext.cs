using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Caesar_Shift.Models
{
    public class FileContext : DbContext
    {
        public DbSet<File> Files { get; set; }

        public FileContext() : base("FileContext") { }

        public int Add(string text)
        {
            return Add("Untiled.txt", text);
        }

        public int Add(string name, string text)
        {
            var file = new File() { Name = name, Text = text };
            Files.Add(file);
            SaveChanges();
            return file.Id;
        }

        public File GetById(int id)
        {
            return Files.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}