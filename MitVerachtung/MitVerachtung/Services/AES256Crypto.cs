using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using CreatePass.Exceptions;

namespace CreatePass.Services
{
    public class AES256Crypto
    {
        public string Encrypt(string plaintext, string pw, string salt)
        {
            byte[] result;

            var key = CreateDerivedKey(pw, Encoding.UTF8.GetBytes(salt));
            var provider = GetProvider(key);
            provider.GenerateIV();
            var iv = provider.IV;

            try
            {
                using (var encrypter = provider.CreateEncryptor(key, iv))
                {
                    using (var cipherStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
                        using (var tBinaryWriter = new BinaryWriter(cryptoStream))
                        {
                            tBinaryWriter.Write(plaintext);
                            cryptoStream.FlushFinalBlock();
                        }

                        result = cipherStream.ToArray();
                    }
                }
            }
            catch (Exception exc)
            {
                throw new EncryptionFailedException("encryption failed", exc);
            }
          

            byte[] rv = new byte[iv.Length + result.Length];
            System.Buffer.BlockCopy(iv, 0, rv, 0, iv.Length);
            System.Buffer.BlockCopy(result, 0, rv, iv.Length, result.Length);

            
            return Convert.ToBase64String(rv);
        }

        public string Decrypt(string encryptedData, string pw, string salt)
        {
            var key = CreateDerivedKey(pw, Encoding.UTF8.GetBytes(salt));
            var provider = GetProvider(key);
            var data = Convert.FromBase64String(encryptedData);

            var iv = new byte[16];
            Array.Copy(data, iv, 16);
            var encryptedBytes = new byte[data.Length - 16];
            Array.Copy(data, 16, encryptedBytes, 0, data.Length - 16);
            provider.IV = iv;

            return Decrypt(provider, encryptedBytes);
        }

        private string Decrypt(AesManaged provider, byte[] encryptedBytes)
        {
            try
            {
                using (var decryptor = provider.CreateDecryptor(provider.Key, provider.IV))
                {

                    var decrypted = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    if ((int)decrypted[0] == decrypted.Length - 1)
                    {
                        var decryptedData = new byte[decrypted[0]];
                        Array.Copy(decrypted, 1, decryptedData, 0, decrypted[0]);
                        decrypted = decryptedData;
                    }
                    else
                    {
                        throw new Exception("length invalid");
                    }
                    return Encoding.Default.GetString(decrypted);
                }
            }
            catch (Exception exc)
            {
                throw new EncryptionFailedException("decryption failed", exc);
            }
        }

        public static byte[] CreateDerivedKey(string password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            var test = new Rfc2898DeriveBytes(password, salt, iterations);
            return test.GetBytes(keyLengthInBytes);
        }

        private AesManaged GetProvider(byte[] key)
        {
            var provider = new AesManaged();
            provider.Padding = PaddingMode.PKCS7;
            provider.Mode = CipherMode.CBC;
            provider.Key = key;
            return provider;
        }
    }
}