using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace EncryptDecrypt.API.Helper
{
    public class EncryptDecryptHelper
    {
        private static string EncryptionKey = "2B7E151628AED2A6ABF7158809CF4F3C"; // Replace with a strong key

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                aesAlg.GenerateIV(); // Generate a random IV for each encryption
                byte[] iv = aesAlg.IV; // Get the IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();

                    // Combine IV and encrypted data for storage/transmission
                    byte[] combinedData = new byte[iv.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(iv, 0, combinedData, 0, iv.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, combinedData, iv.Length, encryptedBytes.Length);

                    return Convert.ToBase64String(combinedData);
                }
            }
        }

        public object Decrypt(string cipherText)
        {
            string decryptedData = DecryptData(cipherText);

            try
            {
                JObject jObject = JObject.Parse(decryptedData);
                return jObject;
            }
            catch
            {
                // If parsing fails, return the decrypted string
                return decryptedData;
            }
        }

        private string DecryptData(string cipherText)
        {
            try
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);

                    byte[] combinedData = Convert.FromBase64String(cipherText);
                    byte[] iv = new byte[aesAlg.BlockSize / 8];
                    byte[] encryptedBytes = new byte[combinedData.Length - iv.Length];

                    Buffer.BlockCopy(combinedData, 0, iv, 0, iv.Length);
                    Buffer.BlockCopy(combinedData, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                    aesAlg.IV = iv;
                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "The given input is not a encrypted text. " + ex.Message;
            }
        }
    }
}
