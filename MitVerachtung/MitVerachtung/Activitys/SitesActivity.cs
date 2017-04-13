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
using CreatePass.Model;
using CreatePass.Controller;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CreatePass.Activitys
{
    [Activity(Label = "Sites")]
    public class SitesActivity : Activity
    {

        List<SiteKeyItem> list = new List<SiteKeyItem>();
        private ListView lv_sites;
        private SitesController controller = new SitesController();

        private SiteKeyItem selectedItem = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Site);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "Sites";

            lv_sites = FindViewById<ListView>(Resource.Id.sites);
            lv_sites.ItemClick += Lv_sites_ItemClick;
            lv_sites.ItemLongClick += Lv_sites_ItemLongClick;

            toolbar.MenuItemClick += Toolbar_MenuItemClick;

            RegisterForContextMenu(lv_sites);

            LoadList();
        }

        private async Task LoadList()
        {

            list = await controller.GetSites();
            var adapter = new SitesAdapter(this, list.ToArray());

            lv_sites.Adapter = adapter;
        } 

        private void Toolbar_MenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            toolbarClicked(e.Item);
        }

       

        private async void Lv_sites_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var item = list[e.Position];
            item = await controller.GetSite(item.SiteKeyItemId);

            var intent = new Intent(this, typeof(PasswordActivity));
            Bundle bun = new Bundle();
            intent.PutExtra("Site", JsonConvert.SerializeObject(item));
            StartActivity(intent);

        }

        private void Lv_sites_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            selectedItem = list[e.Position];
            OpenContextMenu(lv_sites);
        }


        private void toolbarClicked(IMenuItem item) 
        {
            Intent intent;
            switch (item.ItemId)
            {
                case Resource.Id.menu_addSite:
                    intent = new Intent(this, typeof(AddSiteActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.menu_preferences:
                    intent = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent);
                    break;
                case Resource.Id.menu_newPW:
                    intent = new Intent(this, typeof(PasswordActivity));
                    StartActivity(intent);
                    break;
                default:
                    break;
            }
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.site_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            MenuInflater.Inflate(Resource.Menu.siteItem_menus, menu);
            base.OnCreateContextMenu(menu, v, menuInfo);
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            Intent intent;
            switch (item.ItemId)
            {
                case Resource.Id.menu_deleteSite:
                    intent = new Intent(this, typeof(DeleteSiteActivity));
                    intent.PutExtra("Site", JsonConvert.SerializeObject(selectedItem));
                    StartActivity(intent);
                    break;
                case Resource.Id.menu_editSite:
                    intent = new Intent(this, typeof(AddSiteActivity));
                    intent.PutExtra("Site", JsonConvert.SerializeObject(selectedItem));
                    StartActivity(intent);
                    break;
                default:
                    break;
            }
            return base.OnContextItemSelected(item);
        }

        protected override void OnRestart()
        {
            LoadList();
            base.OnRestart();
        }
    }
    public class SitesAdapter : BaseAdapter<SiteKeyItem>
    {
        SiteKeyItem[] items;
        Activity context;
        public SitesAdapter(Activity context, SiteKeyItem[] items) : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override SiteKeyItem this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Length; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Resource.Layout.SitesItem, null);
            view.FindViewById<TextView>(Resource.Id.firstLine).Text = items[position].Url_PlainText;
            view.FindViewById<TextView>(Resource.Id.secondLine).Text = items[position].UserName_PlainText;
            return view;
        }
    }
}