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
        private Services.SettingService settings;
        
        EditText txt_synckey;
        CheckBox cb_useSpecial;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            settings = Services.SettingService.GetInstance();
            
            SetContentView(Resource.Layout.Settings);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            var sl_pwLenght = FindViewById<SeekBar>(Resource.Id.sl_pwLenght);
            var lbl_pwLenght = FindViewById<TextView>(Resource.Id.lbl_pwLenght);
            var btn_toggelSynckey = FindViewById<Button>(Resource.Id.btn_toggleSyncKey);
            txt_synckey = FindViewById<EditText>(Resource.Id.txt_synckey);
            var btn_saveSynckey = FindViewById<Button>(Resource.Id.btn_saveSynckey);
            cb_useSpecial = FindViewById<CheckBox>(Resource.Id.cb_useSpecial);
            var cb_useAlphaNum = FindViewById<CheckBox>(Resource.Id.cb_useAlphabet);
            var cb_useNum = FindViewById<CheckBox>(Resource.Id.cb_useNumber);

            sl_pwLenght.Progress = settings.PwLength - 8;
            lbl_pwLenght.Text = settings.PwLength.ToString();
            cb_useSpecial.Checked = settings.UseSpecialChars;
            cb_useAlphaNum.Checked = settings.UseAlphaNumChars;
            cb_useNum.Checked = settings.UseNumChars;
            txt_synckey.Text = settings.Salt;

            txt_synckey.TextChanged += Txt_synckey_TextChanged;

            cb_useSpecial.CheckedChange += Cb_useSpecial_CheckedChange;

            sl_pwLenght.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    lbl_pwLenght.Text = (e.Progress + 8).ToString();
                    settings.UpdatePwLen(e.Progress + 8);
                }
            };

            btn_toggelSynckey.Click += (sender, e) => {
                if(txt_synckey.Visibility == ViewStates.Gone)
                {
                    txt_synckey.Visibility = ViewStates.Visible;
                    btn_saveSynckey.Visibility = ViewStates.Visible;
                    btn_toggelSynckey.Text = "Hide Synckey";
                }
                else
                {
                    txt_synckey.Visibility = ViewStates.Gone;
                    btn_saveSynckey.Visibility = ViewStates.Gone;
                    btn_toggelSynckey.Text = "Show Synckey";
                }
            };

            btn_saveSynckey.Click += Btn_saveSynckey_Click;


            SetActionBar(toolbar);

            ActionBar.Title = "Settings";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
        }

        private void Btn_saveSynckey_Click(object sender, EventArgs e)
        {
            settings.UpdateSalt(txt_synckey.Text);

            Toast.MakeText(this, "Synckey changed", ToastLength.Short);
        }

        private void Cb_useSpecial_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            settings.UpdatePwChars(true, true, cb_useSpecial.Checked);
        }

        private void Txt_synckey_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            Toast.MakeText(this, "Changing the synckey will result in different passwords. \n\r Only save if you realy want to.", ToastLength.Long);
        }

        public override bool OnNavigateUp()
        {
            Finish();
            return base.OnNavigateUp();
        }
    }
}