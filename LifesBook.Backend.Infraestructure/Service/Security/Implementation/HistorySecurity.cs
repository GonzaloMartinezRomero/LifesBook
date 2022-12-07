using LifesBook.Backend.Infraestructure.Service.Security.Abstract;
using System.Security.Cryptography;

namespace LifesBook.Backend.Infraestructure.Service.Security.Implementation
{
    internal class HistorySecurity : IHistorySecurity
    {
        private const int PASSWORD_BLOQ_SIZE = 16;
        private const char EMPTY_BLOQ_CHARACTER = '0';

        public string Encrypt(string password, string text)
        {
            Aes aes = BuildAes(password);
            byte[] base64Encrypted = aes.EncryptEcb(Configuration.FileEncoding.GetBytes(text),
                                                    PaddingMode.PKCS7);//Encrypt in Base64 array

            return Convert.ToBase64String(base64Encrypted);
        }

        public string Decrypt(string password, string text)
        {
            Aes aes = BuildAes(password);

            byte[] bytesText = Convert.FromBase64String(text);

            var decrypted = aes.DecryptEcb(bytesText, PaddingMode.PKCS7); //Text saved as byte array in UTF8
            var plain = Configuration.FileEncoding.GetString(decrypted);

            return plain;
        }

        public string GenerateHash(string password)
        {
            using SHA256 sha256 = SHA256.Create();

            byte[] bytes = Configuration.FileEncoding.GetBytes(password);

            byte[] hashBytes = sha256.ComputeHash(bytes); //Note: Not produce a UTF-8 Enconding, just transform from B64

            string strBytes = Convert.ToBase64String(hashBytes);

            return strBytes;
        }

        private static Aes BuildAes(string password)
        {
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.ECB;

            //Build password -> Ensure 16 bytes

            //Remove if pass has more than 16 bytes
            string normalizedPassword;

            if (password.Length > PASSWORD_BLOQ_SIZE)
                normalizedPassword = password.Substring(0, PASSWORD_BLOQ_SIZE);   
            else
                normalizedPassword = password.PadRight(PASSWORD_BLOQ_SIZE, EMPTY_BLOQ_CHARACTER); //Refill if has less than 16

            byte[] keyBytes = Configuration.FileEncoding.GetBytes(normalizedPassword);

            aes.Key = keyBytes;

            return aes;
        }
    }
}
