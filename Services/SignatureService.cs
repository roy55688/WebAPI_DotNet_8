using System.Security.Cryptography;
using System.Text;

namespace WebAPISample.Services
{
    public interface ISignatureService
    {
        /// <summary>
        /// MAC加密簽章
        /// </summary>
        /// <param name="LISTR_Content"></param>
        /// <returns></returns>
        string MACDataEncode(string LISTR_Content);

    }
    public class SignatureService : ISignatureService
    {
        readonly string key = "APISAMPLEKEY這是API樣本在AES加密所使用的KEY";
        readonly string IV = "這是API樣本在加密所使用的IV";

        public string MACDataEncode(string LISTR_Value)
        {
            return BASE64Encod(SHA256Encod(AES256Encod(LISTR_Value)));
        }

        private string BASE64Encod(string LISTR_Content)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(LISTR_Content));
        }
        public string AES256Encod(string LISTR_Content)
        {
            using Aes aesObject = Aes.Create();
            aesObject.Mode = CipherMode.CBC;
            aesObject.KeySize = 256;
            aesObject.Padding = PaddingMode.PKCS7;
            aesObject.Key = Encoding.ASCII.GetBytes(key);
            aesObject.IV = Encoding.ASCII.GetBytes(IV);

            ICryptoTransform transform = aesObject.CreateEncryptor();

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(LISTR_Content);

            byte[] ciphertextBytes = transform.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

            return Convert.ToBase64String(ciphertextBytes);
        }

        public string AES256Decod(string ciphertext)
        {
            using Aes aesObject = Aes.Create();
            aesObject.Mode = CipherMode.CBC;
            aesObject.KeySize = 256;
            aesObject.Padding = PaddingMode.PKCS7;
            aesObject.Key = Encoding.ASCII.GetBytes(key);
            aesObject.IV = Encoding.ASCII.GetBytes(IV);

            ICryptoTransform transform = aesObject.CreateDecryptor();

            byte[] ciphertextBytes = Convert.FromBase64String(ciphertext);

            byte[] plaintextBytes = transform.TransformFinalBlock(ciphertextBytes, 0, ciphertextBytes.Length);

            return Encoding.UTF8.GetString(plaintextBytes);
        }

        private string SHA256Encod(string LISTR_Content)
        {
            var shaObject = SHA256.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(LISTR_Content);
            bytes = shaObject.ComputeHash(bytes);
            string text = "";
            byte[] array = bytes;
            foreach (byte b in array)
            {
                text += b.ToString("x2");
            }
            return text;
        }
    }

}
