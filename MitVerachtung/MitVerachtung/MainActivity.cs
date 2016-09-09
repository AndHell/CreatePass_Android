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


        protected override void OnCreate(Bundle bundle)
        {
            passwordGerneator = new CreatePass.PasswordGeneration(18, "saltneedstogohere", true, true, true);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            btn_createPW = FindViewById<Button>(Resource.Id.btn_createPW);
            txt_master = FindViewById<EditText>(Resource.Id.txt_master);
            txt_site = FindViewById<EditText>(Resource.Id.txt_site);
            txt_password = FindViewById<EditText>(Resource.Id.txt_finalPW);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            editToolbar.InflateMenu(Resource.Menu.edit_menus);
            editToolbar.MenuItemClick += (sender, e) => {

                if (e.Item.ItemId == Resource.Id.menu_reset)
                {
                    txt_master.Text = "";
                    txt_site.Text = "";
                    txt_password.Text = "";
                }
                else if(e.Item.ItemId == Resource.Id.menu_copy)
                {
                    ((ClipboardManager)GetSystemService(ClipboardService)).PrimaryClip = ClipData.NewPlainText("CreatePass", txt_password.Text);
                    Toast.MakeText(this, "Copied Password to Clipboard.", ToastLength.Short).Show();
                }
            };

            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            btn_createPW.Click += delegate {
                btn_CreatePasswordClicked();

                //hide keyboard
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(btn_createPW.WindowToken, 0);
            };
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

