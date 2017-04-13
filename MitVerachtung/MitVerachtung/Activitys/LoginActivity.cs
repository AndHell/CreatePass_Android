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
using System.Threading.Tasks;
using CreatePass.Controller;

namespace CreatePass.Activitys
{
    [Activity(Label = "LoginActivity", NoHistory = true)]
    public class LoginActivity : Activity
    {
        private LoginController login;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Login);
            
            

            login = new LoginController();

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            //ToDo: User should enter the decryption key fot his list
            //or skip and work without the list
            var userKey = FindViewById<EditText>(Resource.Id.txt_userkey);
            var btn_login = FindViewById<Button>(Resource.Id.btn_enter);
            var btn_skip = FindViewById<Button>(Resource.Id.btn_skip);

            btn_login.Click += async delegate
            {
                if (string.IsNullOrEmpty(userKey.Text))
                {
                    Toast.MakeText(this, "You are a fool.", ToastLength.Short).Show();
                    return;
                }

                btn_login.Enabled = false;
                userKey.Enabled = false;
                btn_skip.Enabled = false;

                bool isRightPw = await CheckUserKey(userKey.Text);

                if (isRightPw)
                {
                    var intent = new Intent(this, typeof(SitesActivity));
                    StartActivity(intent);
                }
                else
                {
                    Toast.MakeText(this, "You are a fool.", ToastLength.Short).Show();
                    btn_login.Enabled = true;
                    userKey.Enabled = true;
                    btn_skip.Enabled = true;
                }
            };

            btn_skip.Click += delegate
            {
                var intent = new Intent(this, typeof(PasswordActivity));
                StartActivity(intent);
            };
        }

        private async Task<bool> CheckUserKey(string key)
        {
            var isValid = await login.CompareUserkey(key);
            if (isValid)
            {
                await login.SetDecryptionKey(key);
                return true;
            }
            return false;
        }
    }
}