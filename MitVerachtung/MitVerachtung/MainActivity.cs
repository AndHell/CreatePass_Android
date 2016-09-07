using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MitVerachtung
{
    [Activity(Label = "CreatePass", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        CreatePass.PasswordGeneration test;
        
        protected override void OnCreate(Bundle bundle)
        {
            test = new CreatePass.PasswordGeneration(18, "saltneedstogohere", true, true, true);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button);
            var text = FindViewById<EditText>(Resource.Id.editText1);
            var text2 = FindViewById<EditText>(Resource.Id.editText2);
            var res = FindViewById<TextView>(Resource.Id.textView1);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);

            var editToolbar = FindViewById<Toolbar>(Resource.Id.edit_toolbar);
            editToolbar.InflateMenu(Resource.Menu.edit_menus);
            editToolbar.MenuItemClick += (sender, e) => {
                Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();

                if (e.Item.ItemId == Resource.Id.menu_reset)
                {
                    text.Text = "";
                    text2.Text = "";
                    res.Text = "";
                }
            };

            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            button.Click += delegate { res.Text = test.Generate(text.Text, text2.Text); };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.tops_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}

