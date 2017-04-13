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
using Newtonsoft.Json;
using CreatePass.Model;

namespace CreatePass.Activitys
{
    [Activity(Label = "DeleteSiteActivity")]
    public class DeleteSiteActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeleteSite);
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Are you sure?";


            var siteExtra = Intent.GetStringExtra("Site");
            var site = string.IsNullOrEmpty(siteExtra) ? new SiteKeyItem() : JsonConvert.DeserializeObject<SiteKeyItem>(siteExtra);

            var lbl_userName = FindViewById<TextView>(Resource.Id.lbl_userName);
            var lbl_site = FindViewById<TextView>(Resource.Id.lbl_site);

            var btn_delete = FindViewById<Button>(Resource.Id.btn_delete);
            var btn_cancel = FindViewById<Button>(Resource.Id.btn_cancel);

            lbl_site.Text = site.Url_PlainText;
            lbl_userName.Text = site.UserName_PlainText;

            btn_cancel.Click += delegate {
                base.OnBackPressed();
            };

            btn_delete.Click += async delegate
            {
                var controller = new Controller.SitesController();
                
                if (await controller.DeleteSite(site))
                {
                    base.OnBackPressed();
                }
                else
                {
                    Toast.MakeText(this, "deletion faild", ToastLength.Short).Show();
                }
            };
        }
    }
}