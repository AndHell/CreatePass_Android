using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace CreatePass.Model
{
    public class SitePasswordSetting
    {

        [PrimaryKey, AutoIncrement]
        public int SitePasswordSettingId { get; set; }

        public bool UseNumbers { get; set; }
        public bool UseSpecialKeys { get; set; }
        public bool UseLetters { get; set; }
        public int Lenght { get; set; }
    }
}