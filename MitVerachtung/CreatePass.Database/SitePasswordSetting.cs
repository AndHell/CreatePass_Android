using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace CreatePass.Database.Model
{
    public class SitePasswordSetting
    {
        [Key]
        public int SitePasswordSettingID { get; set; }

        public bool UseNumbers { get; set; }
        public bool UseSpecialKeys { get; set; }
        public bool UseLetters { get; set; }

        public int Version { get; set; }

        public int Lenght { get; set; }
    }
}