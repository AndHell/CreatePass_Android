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

namespace MitVerachtung
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.Settings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var sl_pwLenght = FindViewById<SeekBar>(Resource.Id.sl_pwLenght);
            var lbl_pwLenght = FindViewById<TextView>(Resource.Id.lbl_pwLenght);
            var btn_toggelSynckey = FindViewById<Button>(Resource.Id.btn_toggleSyncKey);
            var txt_synckey = FindViewById<EditText>(Resource.Id.txt_synckey);
            var btn_saveSynckey = FindViewById<Button>(Resource.Id.btn_saveSynckey);


            sl_pwLenght.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    lbl_pwLenght.Text = (e.Progress + 8).ToString();
                }
            };

            btn_toggelSynckey.Click += (sender, e) => {
                if(txt_synckey.Visibility == ViewStates.Gone)
                {
                    txt_synckey.Visibility = ViewStates.Visible;
                    btn_saveSynckey.Visibility = ViewStates.Visible;
                }
                else
                {
                    txt_synckey.Visibility = ViewStates.Gone;
                    btn_saveSynckey.Visibility = ViewStates.Gone;
                }
            };


            SetActionBar(toolbar);

            ActionBar.Title = "Settings";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        public override bool OnNavigateUp()
        {
            Finish();
            return base.OnNavigateUp();
        }
    }
}