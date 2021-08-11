using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace OrderDeliveryTracker.Helpers
{
    public static class Crypto
    {
        private static readonly PasswordDeriveBytes pdb = new PasswordDeriveBytes("p@sSw0rD",
            new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d,
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
        private static readonly byte[] Key = pdb.GetBytes(32), IV = pdb.GetBytes(16);


        public static string Encrypt(string plainText)
        {
            string cipherText = null;
            try
            {
                if (plainText != null)
                {
                    byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
                    using (Aes encryptor = Aes.Create())
                    {
                        encryptor.Key = Key;
                        encryptor.IV = IV;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(clearBytes, 0, clearBytes.Length);
                                cs.Close();
                            }
                            cipherText = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return cipherText;
        }

        public static string Decrypt(string cipherText)
        {
            string plainText = null;
            try
            {
                if (cipherText != null)
                {
                    cipherText = cipherText.Replace(" ", "+");
                    byte[] cipherBytes = Convert.FromBase64String(cipherText);
                    using (Aes encryptor = Aes.Create())
                    {
                        encryptor.Key = Key;
                        encryptor.IV = IV;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                            {
                                cs.Write(cipherBytes, 0, cipherBytes.Length);
                                cs.Close();
                            }
                            plainText = Encoding.Unicode.GetString(ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return plainText;
        }
    }
}