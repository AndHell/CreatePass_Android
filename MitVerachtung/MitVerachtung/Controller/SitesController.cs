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
using CreatePass.Exceptions;
using CreatePass.Model;
using System.Threading.Tasks;

namespace CreatePass.Controller
{
    public class SitesController
    {
        private AES256Crypto aes;
        private SettingService settingService;

        public SitesController()
        {
            aes = new AES256Crypto();
            settingService = SettingService.GetInstance();
        }

        //Add keys
        public async Task<int> AddSite(string url, string userName, SitePasswordSetting settings)
        {
            var settingsId = 0;
            if (settings != null)
            {
                settingsId = await AddSitePwSettings(settings);
            }
          

            var newSite = new SiteKeyItem()
            {
                Url_PlainText = url,
                UserName_PlainText = userName,
                SettingsId = settingsId,
                DateAdded = DateTime.Now,
                UserName_Encrypted = aes.Encrypt(userName, TempStore.CryptoKeySiteList, settingService.CryptoSalt),
                Url_Encrypted = aes.Encrypt(url, TempStore.CryptoKeySiteList, settingService.CryptoSalt),
                Version = 1
            };

            var con = await new DataBaseService().GetConnetion();
            await con.InsertAsync(newSite);

            return newSite.SiteKeyItemId;
        }

        public async Task<int> AddSite(SiteKeyItem site, SitePasswordSetting settings)
        {
            return await AddSite(site.Url_PlainText, site.UserName_PlainText, settings);
        }

        public async Task<int> UpdateSite(SiteKeyItem site, SitePasswordSetting settings)
        {
            var settingsId = 0;
            if (settings != null && settings?.SitePasswordSettingId != 0)
            {
                settingsId = await UpdateSitePwSettings(settings);
            }
            else if (settings != null && settings?.SitePasswordSettingId == 0)
            {
                settingsId = await AddSitePwSettings(settings);
            }
            else if (site.SettingsId != 0 && settings == null)
            {
                //delete settings;
            }

            site.DateEdited = DateTime.Now;
            site.Url_Encrypted = aes.Encrypt(site.Url_PlainText, TempStore.CryptoKeySiteList, settingService.CryptoSalt);
            site.UserName_Encrypted = aes.Encrypt(site.UserName_PlainText, TempStore.CryptoKeySiteList, settingService.CryptoSalt);

            var con = await new DataBaseService().GetConnetion();
            await con.UpdateAsync(site);

            return site.SiteKeyItemId;
        }

        private async Task<int> AddSitePwSettings(SitePasswordSetting settings)
        {
            var con = await new DataBaseService().GetConnetion();
            await con.InsertAsync(settings);
            return settings.SitePasswordSettingId;
        }

        private async Task<int> UpdateSitePwSettings(SitePasswordSetting settings)
        {
            var con = await new DataBaseService().GetConnetion();
            await con.UpdateAsync(settings);
            return settings.SitePasswordSettingId;
        }

        public async Task<List<SiteKeyItem>> GetSites()
        {
            var con = await new DataBaseService().GetConnetion();
            var Sites = await con.Table<SiteKeyItem>().ToListAsync();
            foreach (var site in Sites)
            {
                site.Url_PlainText = aes.Decrypt(site.Url_Encrypted, TempStore.CryptoKeySiteList, settingService.CryptoSalt);
                site.UserName_PlainText = aes.Decrypt(site.UserName_Encrypted, TempStore.CryptoKeySiteList, settingService.CryptoSalt);
            }
            return Sites;
        }

        public async Task<SiteKeyItem> GetSite(int siteId)
        {
            var con = await new DataBaseService().GetConnetion();
            var site = await con.Table<SiteKeyItem>().Where(x => x.SiteKeyItemId == siteId).FirstAsync();
            

            if (site == null)
            {
                //throw exc
                return null;
            }

            if (site.SettingsId != 0)
            {
                site.Setting = await con.Table<SitePasswordSetting>().Where(x => x.SitePasswordSettingId == site.SettingsId).FirstAsync();
            }

            return DecryptSite(site);

        }

        //Delete keys
        public async Task<bool> DeleteSite(int siteId)
        {
            var con = await new DataBaseService().GetConnetion();

            try
            {
                var site = await GetSite(siteId);
                if (site.Setting != null)
                {
                    await con.DeleteAsync(site.Setting);
                }
                await con.DeleteAsync(site);
                return true;
            }
            catch (Exception exc)
            {
                Logger.Error(exc.Message);
                return false;
            }
        }

        public async Task<bool> DeleteSite(SiteKeyItem site)
        {
            return await DeleteSite(site.SiteKeyItemId);
        }

        //decryt keys
        private SiteKeyItem DecryptSite(SiteKeyItem site)
        {
            if (string.IsNullOrEmpty(TempStore.CryptoKeySiteList))
                throw new NoDecryptionKeyException();

            site.Url_PlainText = aes.Decrypt(site.Url_Encrypted, TempStore.CryptoKeySiteList, settingService.CryptoSalt);
            site.UserName_PlainText = aes.Decrypt(site.UserName_Encrypted, TempStore.CryptoKeySiteList, settingService.CryptoSalt);

            return site;
        }

        public async Task UpdateSitesEncryption(List<SiteKeyItem> sites)
        {
            foreach (var site in sites)
            {
                await UpdateSite(site, site.Setting);
            }
        }
        
    }
}