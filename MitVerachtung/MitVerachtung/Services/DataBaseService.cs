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
using SQLite;
using CreatePass.Model;
using Android.Util;

namespace MitVerachtung.Services
{
    public class DataBaseService
    {
        private string path = "";
        public DataBaseService(string pathToDb)
        {
            createDatabase(pathToDb);
            path = pathToDb;
        }

        private void createDatabase(string pathtoDb)
        {
            try
            {
                var con = new SQLiteConnection(pathtoDb);
                con.CreateTable<SiteKeyItem>();
                con.Close();
                Log.Info("Create Db", "db is alive");
            }
            catch (Exception exc)
            {
                Log.Error("Create Db", $"Creating failed: {exc.Message}");
            }
        }

        public bool AddSiteKey(SiteKeyItem siteKey)
        {
            var sucessed = false;
            using (var con = new SQLiteConnection(path))
            {
                try
                {
                    siteKey.DateAdded = DateTime.Now;

                    con.Insert(siteKey);

                    sucessed = true;
                    Log.Info("db.AddSiteKey", "SiteKey added");
                }
                catch (Exception exc)
                {
                    Log.Error("db.AddSiteKey", $"Adding failed: {exc.Message}");
                    sucessed = false;
                }
            }
            return sucessed;
        }

        public bool UpdateSiteKey(SiteKeyItem siteKey)
        {
            var sucessed = false;
            using (var con = new SQLiteConnection(path))
            {
                try
                {
                    con.Update(siteKey);

                    sucessed = true;
                    Log.Info("db.AddSiteKey", "SiteKey added");
                }
                catch (Exception exc)
                {
                    Log.Error("db.AddSiteKey", $"Adding failed: {exc.Message}");
                    sucessed = false;
                }
            }
            return sucessed;
        }


        public SiteKeyItem GetSiteKey(int id)
        {
            var siteKey = new SiteKeyItem();
            using(var con = new SQLiteConnection(path))
            {
                try
                {
                    siteKey = con.Get<SiteKeyItem>(id);
                }
                catch (Exception exc)
                {
                    Log.Error("db.GetSiteKey", $"loading sitekey (id {id}) failed: {exc.Message}");
                }
            }
            return siteKey;
        }

        public List<SiteKeyItem> GetAllSitekeys()
        {
            var siteKeys = new List<SiteKeyItem>();
            using (var con = new SQLiteConnection(path))
            {
                try
                {
                    siteKeys = con.Table<SiteKeyItem>().ToList();
                }
                catch (Exception exc)
                {
                    Log.Error("db.GetAllSitekeys", $"loading sitekeys  failed: {exc.Message}");
                }
            }
            return siteKeys;
        }
    }
}