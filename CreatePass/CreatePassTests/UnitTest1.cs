using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CreatePass.Services;
using System.Text;

namespace CreatePassTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AES()
        {
            AES256Crypto aes = new AES256Crypto();
            var plain = "465djgz94";
            var salt = "i lisgfjszkeulkfdke you";
            var key = "this is somsajklöhgfdsadfghje text";
            var encrypted = aes.Encrypt(plain, key, salt);

            var decryped = aes.Decrypt(encrypted, key, salt);

            Assert.AreEqual(plain, decryped);

        }
    }
}
