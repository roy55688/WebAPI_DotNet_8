using System.Security.Cryptography;
using System.Text;

namespace WebAPISample.Services
{
    public interface ISignatureService
    {
        /// <summary>
        /// MAC加密
        /// </summary>
        /// <param name="LISTR_Content"></param>
        /// <returns></returns>
        string MACDataEncode(string LISTR_Content);
    }
    public class SignatureService : ISignatureService
    {
        public string MACDataEncode(string LISTR_Value)
        {
            return BASE64Encod(SHA256Encod(AES256Encod(LISTR_Value)));
        }

        private string BASE64Encod(string LISTR_Content)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(LISTR_Content));
        }
        private string AES256Encod(string LISTR_Content)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("APISAMPLEKEY這是API樣本在AES加密所使用的KEY");
            var aesObject = Aes.Create();
            aesObject.Mode = CipherMode.ECB;
            aesObject.FeedbackSize = 8;
            aesObject.KeySize = 256;
            aesObject.Key = bytes;
            ICryptoTransform transform = aesObject.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(LISTR_Content);
            streamWriter.Close();
            cryptoStream.Close();
            byte[] inArray = memoryStream.ToArray();
            memoryStream.Close();
            return Convert.ToBase64String(inArray);
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
