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
using Android.Util;

namespace CreatePass.Services
{
    static class Logger
    {
        private const string TAG = "CreatePass";
        
        public static void Debug(string message)
        {
            Log.Debug(TAG, message);
        }

        public static void Info(string message)
        {
            Log.Info(TAG, message);
        }

        public static void Warn(string message)
        {
            Log.Warn(TAG, message);
        }

        public static void Error(string message)
        {
            Log.Error(TAG, message);
        }

        public static void Debug(string tagExt, string message)
        {
            Log.Debug(TAG+"."+tagExt, message);
        }

        public static void Info(string tagExt, string message)
        {
            Log.Info(TAG + "." + tagExt, message);
        }

        public static void Warn(string tagExt, string message)
        {
            Log.Warn(TAG + "." + tagExt, message);
        }

        public static void Error(string tagExt, string message)
        {
            Log.Error(TAG + "." + tagExt, message);
        }
    }
}