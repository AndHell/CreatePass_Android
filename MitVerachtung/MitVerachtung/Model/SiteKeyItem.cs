using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CreatePass.Model
{
    public class SiteKeyItem
    {
        [PrimaryKey, AutoIncrement]
        public int SiteKeyItemId { get; set; }

        [Ignore]
        public string Url_PlainText { get; set; }

        public string Url_Encrypted { get; set; }
        
        public string UserName { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
