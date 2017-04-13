using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using CreatePass.Services;
using CreatePass.Model;
using Newtonsoft.Json;

namespace CreatePass.Activitys
{
    [Activity(Label = "CreatePass", Icon = "@drawable/icon")]
    public class PasswordActivity : Activity
    {
        CreatePass.PasswordGeneration passwordGerneator;
        EditText txt_site;
        EditText txt_master;
        EditText txt_password;
        Button btn_createPW;

        Services.SettingService settings;

        SiteKeyItem site;

        SitePasswordSetting pwSettings;


        SeekBar sl_pwLenght;
        TextView lbl_pwLenght;
        TextView lbl_chars;
        TextView lbl_lenght_num;
        CheckBox cb_useSpecial;
        CheckBox cb_useAlphaNum;
        CheckBox cb_useNum;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            settings = Services.SettingService.GetInstance();

            var siteExtra = Intent.GetStringExtra("Site");
            site = string.IsNullOrEmpty(siteExtra) ? null : JsonConvert.DeserializeObject<SiteKeyItem>(siteExtra);
            
            pwSettings = new SitePasswordSetting()
            {
                Lenght = site?.Setting?.Lenght ?? settings.PwLength,
                UseLetters = site?.Setting?.UseLetters ?? settings.UseAlphaNumChars,
                UseNumbers = site?.Setting?.UseNumbers ?? settings.UseNumChars,
                UseSpecialKeys = site?.Setting?.UseSpecialKeys ?? settings.UseSpecialChars
            };

            passwordGerneator = new CreatePass.PasswordGeneration(pwSettings.Lenght,
                                                    settings.Salt, pwSettings.UseNumbers,
                                                    pwSettings.UseLetters, pwSettings.UseSpecialKeys);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Password);

            btn_createPW = FindViewById<Button>(Resource.Id.btn_createPW);
            txt_master = FindViewById<EditText>(Resource.Id.txt_master);
            txt_site = FindViewById<EditText>(Resource.Id.txt_site);
            txt_password = FindViewById<EditText>(Resource.Id.txt_finalPW);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            btn_createPW.Click += async delegate
            {
                await btn_CreatePasswordClicked();

                //hide keyboard
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(btn_createPW.WindowToken, 0);
            };


            sl_pwLenght = FindViewById<SeekBar>(Resource.Id.sl_pwLenght);
            lbl_pwLenght = FindViewById<TextView>(Resource.Id.lbl_pwLenght);
            cb_useSpecial = FindViewById<CheckBox>(Resource.Id.cb_useSpecial);
            cb_useAlphaNum = FindViewById<CheckBox>(Resource.Id.cb_useAlphabet);
            cb_useNum = FindViewById<CheckBox>(Resource.Id.cb_useNumber);
            lbl_chars = FindViewById<TextView>(Resource.Id.lbl_chars);
            lbl_lenght_num = FindViewById<TextView>(Resource.Id.lbl_lenght_num);

            var lbl_userName = FindViewById<TextView>(Resource.Id.lbl_userName);
            var lbl_version = FindViewById<TextView>(Resource.Id.lbl_version);

            sl_pwLenght.Progress = pwSettings.Lenght-8;
            lbl_pwLenght.Text = (pwSettings.Lenght).ToString();
            cb_useSpecial.Checked = pwSettings.UseSpecialKeys;
            cb_useAlphaNum.Checked = pwSettings.UseLetters;
            cb_useNum.Checked = pwSettings.UseNumbers;
            var btn_siteSettings = FindViewById<Button>(Resource.Id.btn_sitepasswortsettings);

            sl_pwLenght.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
                if (e.FromUser)
                {
                    var lenght = e.Progress + 8;
                    lbl_pwLenght.Text = lenght.ToString();
                    pwSettings.Lenght = lenght;
                    passwordGerneator = new CreatePass.PasswordGeneration(pwSettings.Lenght,
                                                            settings.Salt, pwSettings.UseNumbers,
                                                            pwSettings.UseLetters, pwSettings.UseSpecialKeys);
                }
            };
            cb_useSpecial.CheckedChange += Cb_useSpecial_CheckedChange;

            txt_site.Text = site?.Url_PlainText ?? "";
            lbl_userName.Text = "user    : " + site?.UserName_PlainText ?? "";
            lbl_version.Text =  "version : " + site?.Version.ToString() ?? "";

            if(site == null)
            {
                lbl_userName.Visibility = ViewStates.Gone;
                lbl_version.Visibility = ViewStates.Gone;
            }

            btn_siteSettings.Click += delegate
            {
                switchPwSettingsVisibility();
            };
        }


        private void switchPwSettingsVisibility()
        {
            if (lbl_chars.Visibility == ViewStates.Gone)
            {
                lbl_chars.Visibility = ViewStates.Visible;
                sl_pwLenght.Visibility = ViewStates.Visible;
                lbl_pwLenght.Visibility = ViewStates.Visible;
                lbl_lenght_num.Visibility = ViewStates.Visible;
                cb_useAlphaNum.Visibility = ViewStates.Visible;
                cb_useNum.Visibility = ViewStates.Visible;
                cb_useSpecial.Visibility = ViewStates.Visible;
            }
            else
            {
                lbl_chars.Visibility = ViewStates.Gone;
                sl_pwLenght.Visibility = ViewStates.Gone;
                lbl_pwLenght.Visibility = ViewStates.Gone;
                lbl_lenght_num.Visibility = ViewStates.Gone;
                cb_useAlphaNum.Visibility = ViewStates.Gone;
                cb_useNum.Visibility = ViewStates.Gone;
                cb_useSpecial.Visibility = ViewStates.Gone;


                sl_pwLenght.Progress = pwSettings.Lenght - 8;
                lbl_pwLenght.Text = pwSettings.Lenght.ToString();
                cb_useSpecial.Checked = pwSettings.UseSpecialKeys;
                cb_useAlphaNum.Checked = pwSettings.UseLetters;
                cb_useNum.Checked = pwSettings.UseNumbers;
            }
        }

        private void Cb_useSpecial_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            pwSettings.UseSpecialKeys = e.IsChecked;
            passwordGerneator = new CreatePass.PasswordGeneration(pwSettings.Lenght,
                                                    settings.Salt, pwSettings.UseNumbers,
                                                    pwSettings.UseLetters, pwSettings.UseSpecialKeys);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.pw_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_copy:
                    if (!string.IsNullOrEmpty(txt_password.Text))
                    {
                        ((ClipboardManager)GetSystemService(ClipboardService)).PrimaryClip = ClipData.NewPlainText("CreatePass", txt_password.Text);
                        Toast.MakeText(this, "Copied Password to Clipboard.", ToastLength.Short).Show();
                    }
                    break;
                default:
                    break;
            }
           
            return base.OnOptionsItemSelected(item);
        }

        private void ResetInputs()
        {
            txt_master.Text = "";
            txt_site.Text = "";
            txt_password.Text = "";
            //remove pw from Clipboard
            ((ClipboardManager)GetSystemService(ClipboardService)).PrimaryClip = ClipData.NewPlainText("CreatePass", "");
        }


        private async Task btn_CreatePasswordClicked()
        {
            var versionPrefix = site?.Version == 1 ? "" : site?.Version.ToString() ?? "";
            var masterkey = txt_master.Text;
            var sitekey = versionPrefix + txt_site.Text;


            if (string.IsNullOrEmpty(sitekey) || string.IsNullOrEmpty(masterkey))
            {
                return;
            }

            if (masterkey.Length < 8)
            {
                Toast.MakeText(this, "Your Masterkey is to short. 8 chars min.", ToastLength.Long).Show();
                return;
            }
            string pw = "";
            await Task.Run(() => { pw = passwordGerneator.Generate(masterkey, sitekey); });

            txt_password.Text = pw;
        }
    }
}

