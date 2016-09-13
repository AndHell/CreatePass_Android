using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;

namespace MitVerachtung
{
    [Activity(Label = "CreatePass", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        CreatePass.PasswordGeneration passwordGerneator;
        EditText txt_site;
        EditText txt_master;
        EditText txt_password;
        Button btn_createPW;

        Services.SettingService settings;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            settings = Services.SettingService.GetInstance();

            settings.OnSettingsChangedEvent += Settings_OnSettingsChangedEvent;

            passwordGerneator = new CreatePass.PasswordGeneration(settings.PwLength,
                                                    settings.Salt, settings.UseNumChars,
                                                    settings.UseAlphaNumChars, settings.UseSpecialChars);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            btn_createPW = FindViewById<Button>(Resource.Id.btn_createPW);
            txt_master = FindViewById<EditText>(Resource.Id.txt_master);
            txt_site = FindViewById<EditText>(Resource.Id.txt_site);
            txt_password = FindViewById<EditText>(Resource.Id.txt_finalPW);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            editToolbar.InflateMenu(Resource.Menu.edit_menus);
            editToolbar.MenuItemClick += (sender, e) => {
                editToolbarClicked(e.Item);
            };


            btn_createPW.Click += delegate {
                btn_CreatePasswordClicked();

                //hide keyboard
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(btn_createPW.WindowToken, 0);
            };
        }

        private void Settings_OnSettingsChangedEvent()
        {
            passwordGerneator = new CreatePass.PasswordGeneration(settings.PwLength,
                                                    settings.Salt, settings.UseNumChars,
                                                    settings.UseAlphaNumChars, settings.UseSpecialChars);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.tops_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_preferences)
            {
                StartActivity(new Intent(this, typeof(SettingsActivity)));
            }
            return base.OnOptionsItemSelected(item);
        }

        private void editToolbarClicked(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_reset:
                    ResetInputs();
                    break;
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
        }

        private void ResetInputs()
        {
            txt_master.Text = "";
            txt_site.Text = "";
            txt_password.Text = "";
            //remove pw from Clipboard
            ((ClipboardManager)GetSystemService(ClipboardService)).PrimaryClip = ClipData.NewPlainText("CreatePass", "");
        }


        private void btn_CreatePasswordClicked()
        {
            var masterkey = txt_master.Text;
            var sitekey = txt_site.Text;


            if (string.IsNullOrEmpty(sitekey)||string.IsNullOrEmpty(masterkey))
            {
                return;
            }

            if (masterkey.Length < 8)
            {
                Toast.MakeText(this, "Your Masterkey is to short. 8 chars min.", ToastLength.Long).Show();
                return;
            }

            txt_password.Text = passwordGerneator.Generate(masterkey, sitekey);
        }
    }
}

