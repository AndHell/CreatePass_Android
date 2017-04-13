using CreatePass.Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatePass.Database
{
    public class CreatePassContext : DbContext
    {
        public DbSet<SiteKeyItem> SiteKeys { get; set; }
        public DbSet<SitePasswordSetting> SiteSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=CreatePass.db");
        }
    }
}
