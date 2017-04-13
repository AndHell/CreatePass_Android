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
using System.Threading.Tasks;
using CreatePass.Services;

namespace CreatePass.Activitys
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Logger.Debug("SplashActivity.OnCreate");
        }

        protected override void OnResume()
        {
            base.OnResume();
            bool isFirstLaunch = false;
            Type startView = typeof(FirstLaunchActivity);
             Task startupWork = new Task(() => {
                 var settings = SettingService.GetInstance();
                 isFirstLaunch = string.IsNullOrEmpty(settings.HashedUserKey);
                 startView = isFirstLaunch ? typeof(FirstLaunchActivity) : typeof(LoginActivity);
             });
            
            
            startupWork.ContinueWith(t => {
                Logger.Debug("Launch CreatePass");
                    StartActivity(new Intent(Application.Context, startView));
                    
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();
        }
    }
}