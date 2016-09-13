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
using CreatePass;
using Java.Security;

namespace MitVerachtung.Services
{
    public class SettingService
    {
        private const string SET_PASSWORDLENGHT = "SET_PASSWORDLENGHT";
        private const string SET_USESPECIAL = "SET_USESPECIAL";
        private const string SET_USENUM = "SET_USENUM";
        private const string SET_USEALPHANUM = "SET_USEALPHANUM";
        private const string SET_AUTOCOPY = "SET_AUTOCOPY";
        private const string SET_ISNOTFIRSTLAUNCH = "SET_ISNOTFIRSTLAUNCH";
        private const string SET_SALT = "SET_SALT";

        private const string APP_NAME = "CreatePass";


        public int PwLength { get; private set; }
        public bool AutoCopy { get; private set; }

        public bool UseNumChars { get; private set; }
        public bool UseAlphaNumChars { get; private set; }
        public bool UseSpecialChars { get; private set; }

        public bool IsNotFirstLaunch { get; private set; }

        public string Salt { get; private set; }


        //public string SitekeySalt { get; private set; }


        public delegate void SettingsChanged();
        public event SettingsChanged OnSettingsChangedEvent;


        private static SettingService self;


        public SettingService()
        {
            CreateOnFirstLaunch();
            LoadAutoCopy();
            LoadIsNotFirstLoad();
            LoadPwChars();
            LoadSalt();
            LoaPwLen();
        }


        public static SettingService GetInstance()
        {
            if (self == null)
            {
                self = new SettingService();
            }

            return self;
        }


        private void CreateOnFirstLaunch()
        {
            IsNotFirstLaunch = ReadBoolSetting(SET_ISNOTFIRSTLAUNCH);
            if (!IsNotFirstLaunch)
            {
                WriteSetting(SET_ISNOTFIRSTLAUNCH, true);
                WriteSetting(SET_AUTOCOPY, false);
                WriteSetting(SET_USENUM, true);
                WriteSetting(SET_USEALPHANUM, true);
                WriteSetting(SET_USESPECIAL, true);
                WriteSetting(SET_PASSWORDLENGHT, 18);
                WeNeedASalt();
            }
        }

        private void WeNeedASalt()
        {
            var hashPW = new PasswordGeneration(DateTime.Now.Ticks.ToString());
            var random = new SecureRandom();
            var salt = hashPW.Generate(random.NextInt().ToString(), random.NextInt().ToString());

            WriteSetting(SET_SALT, salt); //todo: createsalt
        }

        private void LoadAutoCopy()
        {
            AutoCopy = ReadBoolSetting(SET_AUTOCOPY);
        }

        private void LoaPwLen()
        {
            PwLength = ReadIntSetting(SET_PASSWORDLENGHT);
        }

        private void LoadPwChars()
        {
            UseNumChars = ReadBoolSetting(SET_USENUM);
            UseAlphaNumChars = ReadBoolSetting(SET_USEALPHANUM);
            UseSpecialChars = ReadBoolSetting(SET_USESPECIAL);
        }

        private void LoadSalt()
        {
            Salt = ReadStringSetting(SET_SALT);
        }
        
        private void LoadIsNotFirstLoad()
        {
            IsNotFirstLaunch = ReadBoolSetting(SET_ISNOTFIRSTLAUNCH);
        }

        public void UpdatePwLen(int pwLen)
        {
            WriteSetting(SET_PASSWORDLENGHT, pwLen);
            PwLength = pwLen;

            OnSettingsChangedEvent();
        }

        public void UpdateAutoCopy(bool autoCopy)
        {
            WriteSetting(SET_AUTOCOPY, autoCopy);
            AutoCopy = autoCopy;


            OnSettingsChangedEvent();
        }

        public void UpdatePwChars(bool num, bool alphaNum, bool special)
        {
            WriteSetting(SET_USENUM, num);
            WriteSetting(SET_USEALPHANUM, alphaNum);
            WriteSetting(SET_USESPECIAL, special);

            UseNumChars = num;
            UseAlphaNumChars = alphaNum;
            UseSpecialChars = special;


            OnSettingsChangedEvent();

        }

        public void UpdateSalt(string newSalt)
        {
            WriteSetting(SET_SALT, newSalt);
            Salt = newSalt;


            OnSettingsChangedEvent();
        }


        private void WriteSetting(string id, string value)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(id, value);
            prefEditor.Commit();

        }
        private void WriteSetting(string id, int value)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutInt(id, value);
            prefEditor.Commit();

        }
        private void WriteSetting(string id, bool value)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutBoolean(id, value);
            prefEditor.Commit();

        }

        private bool ReadBoolSetting(string id)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            return prefs.GetBoolean(id, false);
        }

        private int ReadIntSetting(string id)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            return prefs.GetInt(id, 0);
        }

        private string ReadStringSetting(string id)
        {
            var prefs = Application.Context.GetSharedPreferences(APP_NAME, FileCreationMode.Private);
            return prefs.GetString(id, string.Empty);
        }
    }
}