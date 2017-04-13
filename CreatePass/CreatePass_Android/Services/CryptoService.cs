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
using DevOne.Security.Cryptography.BCrypt;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CreatePass.Services
{
    public class CryptoService
    {
        public bool CompareBCryptKey(string plaintext, string hash)
        {
            bool isValid = false;
            try
            {
                isValid = BCryptHelper.CheckPassword(plaintext, hash);
            }
            catch (Exception)
            {

            }

            return isValid;
        }

        public string HashBCrypt(string toHash)
        {
            return BCryptHelper.HashPassword(toHash, BCryptHelper.GenerateSalt(11));
        }

        private byte[] HashSHA512(string key)
        {
            SHA512 algProvider = new SHA512Managed();

            byte[] buffMsg1 = Encoding.BigEndianUnicode.GetBytes(key);

            return algProvider.ComputeHash(buffMsg1);
        }

        public string HashSHA512AsString(string key)
        {
            byte[] hashByteArray = HashSHA512(key);

            return BitConverter.ToString(hashByteArray, 0);
        }

        public ulong HashSHA512AsLong(string key)
        {
            byte[] hashByteArray = HashSHA512(key);

            return BitConverter.ToUInt64(hashByteArray, 0);
        }
    }
}