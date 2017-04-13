using System;
using System.Collections.Generic;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Android.OS;
using Android.Runtime;

namespace CreatePass.Model
{
    public class SiteKeyItem
    {
        [PrimaryKey, AutoIncrement]
        public int SiteKeyItemId { get; set; }

        /// <summary>
        /// unencrypted url
        /// </summary>
        [Ignore]
        public string Url_PlainText { get; set; }

        /// <summary>
        /// encrypted url
        /// </summary>
        public string Url_Encrypted { get; set; }

        /// <summary>
        /// unencrypted user name
        /// </summary>
        [Ignore]
        public string UserName_PlainText { get; set; }

        /// <summary>
        /// encrypted user name
        /// </summary>
        public string UserName_Encrypted { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime DateEdited { get; set; }

        /// <summary>
        /// version of Password
        /// can be counted up to get a new password for this site
        /// </summary>
        public int Version { get; set; }

        [Indexed]
        public int SettingsId { get; set; }
        
        [Ignore]
        public SitePasswordSetting Setting { get; set; }
    }
}
