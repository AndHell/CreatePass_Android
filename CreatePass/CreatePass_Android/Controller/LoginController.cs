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
using CreatePass.Services;
using System.Threading.Tasks;
using CreatePass.Model;

namespace CreatePass.Controller
{
    public class LoginController
    {
        private CryptoService crypto = new CryptoService();
        
        public async Task SaveUserKey(string key)
        {
            var hash = await Task.Run(() => crypto.HashBCrypt(key));
            var settings = SettingService.GetInstance();
            settings.UpdateUserkey(hash);
            
        }

        public async Task<bool> CompareUserkey(string key)
        {
            try
            {
                var settings = SettingService.GetInstance();
                return await Task.Run(() => crypto.CompareBCryptKey(key, settings.HashedUserKey));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SetDecryptionKey(string plaintextKey)
        {
            var hashedKey = await Task.Run(() => crypto.HashSHA512AsString(plaintextKey));
            TempStore.CryptoKeySiteList = hashedKey;
        }

    }
}