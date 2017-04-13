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
using CreatePass.Services;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CreatePass.Model;
using CreatePass.Controller;

namespace CreatePass.Activitys
{
    [Activity(Label = "AddSite", NoHistory = true)]
    public class AddSiteActivity : Activity
    {

        private EditText txt_site;
        private EditText txt_username;
        private CheckBox cb_useNum;
        private CheckBox cb_useAlpha;
        private CheckBox cb_useSpecial;
        private SeekBar sl_pwLenght;
        private TextView lbl_lenght;
        private TextView lbl_lenght_num;
        private TextView lbl_chars;

        private bool useSiteSettings = false;

        private SiteKeyItem currentSite;
        private SitePasswordSetting pwSettings;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var settings = SettingService.GetInstance();
            SetContentView(Resource.Layout.AddSite);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Add Site";
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            toolbar.MenuItemClick += Toolbar_MenuItemClick;
            // Create your application here
            txt_site = FindViewById<EditText>(Resource.Id.txt_site);
            txt_username = FindViewById<EditText>(Resource.Id.txt_username);
            var btn_siteSettings = FindViewById<Button>(Resource.Id.btn_sitepasswortsettings);

            lbl_lenght = FindViewById<TextView>(Resource.Id.lbl_length_site);
            sl_pwLenght = FindViewById<SeekBar>(Resource.Id.sl_pwLenght_site);
            lbl_lenght_num = FindViewById<TextView>(Resource.Id.lbl_pwLenght_site);
            lbl_chars = FindViewById<TextView>(Resource.Id.lbl_chars_site);
            cb_useNum = FindViewById<CheckBox>(Resource.Id.cb_useNumber_site);
            cb_useAlpha = FindViewById<CheckBox>(Resource.Id.cb_useAlphabet_site);
            cb_useSpecial = FindViewById<CheckBox>(Resource.Id.cb_useSpecial_site);

         


            var siteExtra = Intent.GetStringExtra("Site");
            var site = string.IsNullOrEmpty(siteExtra) ? null : 
                await new SitesController().GetSite(JsonConvert.DeserializeObject<SiteKeyItem>(siteExtra).SiteKeyItemId);

            currentSite = site ?? new SiteKeyItem();

            txt_site.Text = site?.Url_PlainText;
            txt_username.Text = site?.UserName_PlainText;

            if (site?.Setting != null)
                switchPwSettingsVisibility();

            pwSettings = new SitePasswordSetting()
            {
                Lenght = site?.Setting?.Lenght ?? settings.PwLength,
                UseLetters = site?.Setting?.UseLetters ?? settings.UseAlphaNumChars,
                UseNumbers = site?.Setting?.UseNumbers ?? settings.UseNumChars,
                UseSpecialKeys = site?.Setting?.UseSpecialKeys ?? settings.UseSpecialChars
            };


            sl_pwLenght.Progress = pwSettings.Lenght - 8;
            lbl_lenght_num.Text = pwSettings.Lenght.ToString();
            cb_useSpecial.Checked = pwSettings.UseSpecialKeys;
            cb_useAlpha.Checked = pwSettings.UseLetters;
            cb_useNum.Checked = pwSettings.UseNumbers;

            sl_pwLenght.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    lbl_lenght_num.Text = (e.Progress + 8).ToString();
                    
                }
            };

            btn_siteSettings.Click += delegate
            {
                switchPwSettingsVisibility();
            };
        }

        private void switchPwSettingsVisibility()
        {
            if (lbl_chars.Visibility == ViewStates.Gone)
            {
                useSiteSettings = true;
                lbl_chars.Visibility = ViewStates.Visible;
                sl_pwLenght.Visibility = ViewStates.Visible;
                lbl_lenght_num.Visibility = ViewStates.Visible;
                lbl_chars.Visibility = ViewStates.Visible;
                cb_useAlpha.Visibility = ViewStates.Visible;
                cb_useNum.Visibility = ViewStates.Visible;
                cb_useSpecial.Visibility = ViewStates.Visible;
            }
            else
            {
                useSiteSettings = false;
                lbl_chars.Visibility = ViewStates.Gone;
                sl_pwLenght.Visibility = ViewStates.Gone;
                lbl_lenght_num.Visibility = ViewStates.Gone;
                lbl_chars.Visibility = ViewStates.Gone;
                cb_useAlpha.Visibility = ViewStates.Gone;
                cb_useNum.Visibility = ViewStates.Gone;
                cb_useSpecial.Visibility = ViewStates.Gone;


                sl_pwLenght.Progress = pwSettings.Lenght - 8;
                lbl_lenght_num.Text = pwSettings.Lenght.ToString();
                cb_useSpecial.Checked = pwSettings.UseSpecialKeys;
                cb_useAlpha.Checked = pwSettings.UseLetters;
                cb_useNum.Checked = pwSettings.UseNumbers;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.addSite_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private async void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.menu_addSite:
                    //toDo Save
                    Toast.MakeText(this, "save . . . ", ToastLength.Short);
                    var isSaved = await Save();
                    if (isSaved)
                    {
                        base.OnBackPressed();
                    }
                    break;
                default:
                    break;
            }
        }

        private async Task<bool> Save()
        {
            var siteContoller = new Controller.SitesController();
            if (string.IsNullOrEmpty(txt_site.Text))
                return false;

            var siteSettings = useSiteSettings == true ? 
                                new Model.SitePasswordSetting()
                                        {
                                            SitePasswordSettingId = currentSite.Setting?.SitePasswordSettingId ?? 0, 
                                            Lenght = sl_pwLenght.Progress +8 ,
                                            UseLetters = cb_useAlpha.Checked,
                                            UseNumbers = cb_useNum.Checked,
                                            UseSpecialKeys = cb_useSpecial.Checked
                                        } 
                                : null;

            currentSite.Url_PlainText = txt_site.Text;
            currentSite.UserName_PlainText = txt_username.Text;

            try
            {
                if (currentSite.SiteKeyItemId != 0)
                {
                    await siteContoller.UpdateSite(currentSite, siteSettings);
                }
                else
                {
                    await siteContoller.AddSite(currentSite, siteSettings);
                }
                return true;
            }
            catch (Exception exc)
            {
                Logger.Debug(exc.Message);
                return false;
            }

        }

        public override bool OnNavigateUp()
        {
            Finish();
            return base.OnNavigateUp();
        }
    }
}