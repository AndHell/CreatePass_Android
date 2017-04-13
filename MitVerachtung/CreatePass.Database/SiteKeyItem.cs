using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CreatePass.Database.Model
{
    public class SiteKeyItem
    {
        [Key]
        public int SiteKeyItemId { get; set; }
        
        [NotMapped]
        public string Url_PlainText { get; set; }

        public string Url_Encrypted { get; set; }


        [NotMapped]
        public string UserName_PlainText { get; set; }

        public string UserName_Encrypted { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateUpdated { get; set; }
        
        public SitePasswordSetting Settings { get; set; }
        
    }
}
