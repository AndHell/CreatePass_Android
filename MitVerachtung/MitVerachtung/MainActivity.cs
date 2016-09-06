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

            button.Click += delegate { res.Text = test.Generate(text.Text, text2.Text); };
        }
    }
}

