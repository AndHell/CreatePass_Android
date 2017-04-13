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
using CreatePass.Controller;
using System.Threading.Tasks;

namespace CreatePass.Activitys
{
    [Activity(Label = "FirstLaunchActivity", NoHistory = true)]
    public class FirstLaunchActivity : Activity
    {
        private LoginController login;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //Todo: Ask user to set up a decryption key
            login = new LoginController();
            SetContentView(Resource.Layout.FirstLaunch);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "CreatePass";

            var userKey = FindViewById<EditText>(Resource.Id.txt_userkey);
            var userKeyRepeated = FindViewById<EditText>(Resource.Id.txt_userkeyRepeated);

            var confirmButton = FindViewById<Button>(Resource.Id.btn_enter);


            confirmButton.Click += async delegate
            {
                if (userKey.Text.Length >= 8 && userKey.Text != userKeyRepeated.Text)
                {
                    Toast.MakeText(this, "You are a fool.", ToastLength.Short).Show();
                }
                else
                {
                    confirmButton.Text = "generating keys";
                    confirmButton.Enabled = false;
                    userKey.Enabled = false;
                    userKeyRepeated.Enabled = false;



                    if(string.IsNullOrEmpty(Services.TempStore.CryptoKeySiteList))
                    {
                        await CreateUserkey(userKey.Text);
                    }
                    else
                    {
                        await UpdateUserkey(userKey.Text);
                    }
                    confirmButton.Text = "returned";
                    confirmButton.Enabled = true;
                    userKey.Enabled = true;
                    userKeyRepeated.Enabled = true;
                    //redirect to mainpage on sucess
                    StartActivity(typeof(SitesActivity));

                };
            };
        }

        private async Task CreateUserkey(string key)
        {
            await login.SaveUserKey(key);         //save BCrypt Hash
            await login.SetDecryptionKey(key);    //create sha512 temp
            
        }

        private async Task UpdateUserkey(string key)
        {
            var siteController = new SitesController();
            var sites = await siteController.GetSites();

            await login.SaveUserKey(key);         //save BCrypt Hash
            await login.SetDecryptionKey(key);    //create sha512 temp

            await siteController.UpdateSitesEncryption(sites);
        }
    }
}