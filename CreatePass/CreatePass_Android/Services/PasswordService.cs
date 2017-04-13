using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CreatePass
{
    public class PasswordGeneration
    {
        private int passwordLength;

        private List<char> numChars;
        private List<char> smallAlphaChars;
        private List<char> bigAlphaChars;
        private List<char> speczialKeys;

        private List<char> chars;
        private string salt;

        public PasswordGeneration(string strSalt)
        {
            passwordLength = 0;
            numChars = "0123456789".ToList();
            var alphas = "abcdefghijklmnopqrstuvwxyz";
            smallAlphaChars = alphas.ToList();
            bigAlphaChars = alphas.ToUpper().ToList();
            speczialKeys = @"@#&+*/\=-.!?:(){}<>".ToList();

            UpdatePwChars(true, true, true);
            salt = strSalt;
        }

        public PasswordGeneration(int pwLen, string strSalt, bool useNum, bool useAlpha, bool useSpecial)
        {
            passwordLength = pwLen;
            numChars = "0123456789".ToList();
            var alphas = "abcdefghijklmnopqrstuvwxyz";
            smallAlphaChars = alphas.ToList();
            bigAlphaChars = alphas.ToUpper().ToList();
            speczialKeys = @"@#&+*/\=-.!?:(){}<>".ToList();

            UpdatePwChars(useNum, useAlpha, useSpecial);
            salt = strSalt;
        }

        public void UpdatePwLen(int newPwLen)
        {
            passwordLength = newPwLen;
        }

        public void UpdateSalt(string newSalt)
        {
            salt = newSalt;
        }

        public void UpdatePwChars(bool useNum, bool useAlpha, bool useSpecial)
        {
            chars = new List<char>();
            if (useAlpha)
            {
                chars.AddRange(smallAlphaChars);
                chars.AddRange(bigAlphaChars);
            }
            if (useNum)
            {
                chars.AddRange(numChars);
            }
            if (useSpecial)
            {
                chars.AddRange(speczialKeys);
            }
        }


        public string Generate(string masterKey, string sitekey)
        {
            var part1 = GetHash(salt + masterKey + sitekey);
            var part2 = GetHash(sitekey + masterKey + salt);
            var part3 = GetHash(masterKey + salt + sitekey);

            var pw1 = HashToPw(part1);
            var pw2 = HashToPw(part2);
            var pw3 = HashToPw(part3);

            var endPW = BuildSingleString(pw1, pw2, pw3);

            return (passwordLength != 0) ? endPW.Substring(0, passwordLength) : endPW;
        }

        private ulong GetHash(string passpharse)
        {
            /* SHA512 algProvider = new SHA512Managed();

             byte[] buffMsg1 = Encoding.BigEndianUnicode.GetBytes(passpharse);

             byte[] hashByteArray = algProvider.ComputeHash(buffMsg1);

             return BitConverter.ToUInt64(hashByteArray, 0);*/
            return new Services.CryptoService().HashSHA512AsLong(passpharse);
        }

        private string HashToPw(ulong hashAsInt)
        {
            var finalPass = "";
            while (hashAsInt > 0)
            {
                int pos = (int)(hashAsInt % (ulong)chars.Count);
                finalPass += chars[pos];
                hashAsInt = hashAsInt / (ulong)(chars.Count / 2);
            }

            return finalPass;
        }

        private string BuildSingleString(string part1, string part2, string part3)
        {
            var length = GetLongestString(part1, part2, part3);
            var singelstring = "";
            for (int i = 0; i < length; i++)
            {
                singelstring += GetCharAtIndex(part1, i);
                singelstring += GetCharAtIndex(part2, i);
                singelstring += GetCharAtIndex(part3, i);
            }

            return singelstring;
        }

        private string GetCharAtIndex(string masterkey, int i)
        {
            var charAtIndex = "";
            try
            {
                charAtIndex += masterkey[i];
            }
            catch (Exception)
            {
                return " ";
            }

            return charAtIndex;
        }

        private int GetLongestString(string masterkey, string sitekey, string salt)
        {
            var length = 0;
            if (masterkey.Length > sitekey.Length && masterkey.Length > salt.Length)
            {
                length = masterkey.Length;
            }
            else if (sitekey.Length > salt.Length)
            {
                length = sitekey.Length;
            }
            else
            {
                length = salt.Length;
            }

            return length;
        }
    }
}
