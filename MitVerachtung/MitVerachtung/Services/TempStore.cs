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

namespace CreatePass.Services
{
     public static class TempStore
    {
        public static string CryptoKeySiteList { get; set; }
    }
}