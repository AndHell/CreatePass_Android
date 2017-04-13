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
using Android.Util;
using System.Threading.Tasks;
using SQLite;
using System.IO;

namespace CreatePass.Services
{
    public class DataBaseService
    {
        private string dbPath = "";

        public DataBaseService()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "CreatePass.db");
            dbPath = path;
        }
        
        public async Task<SQLiteAsyncConnection> GetConnetion()
        {
            var con = new SQLiteAsyncConnection(dbPath);
            if(await createDatabase(con))
                return con;
            else
            {
                //trow db error
            }
            return con;
        }
        

        private async Task<bool> createDatabase(SQLiteAsyncConnection con)
        {
            try
            {
                await con.CreateTableAsync<SiteKeyItem>();
                await con.CreateTableAsync<SitePasswordSetting>();
                Log.Info("Create Db", "db is alive");
                return true;
            }
            catch (Exception exc)
            {
                Log.Error("Create Db", $"Creating failed: {exc.Message}");
                return false;
            }
        }

    }

}
