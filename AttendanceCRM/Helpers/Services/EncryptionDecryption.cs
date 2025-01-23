using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace AttendanceCRM.Helpers.Services
{
    public class EncryptionDecryption
    {
        #region Variable Declaration

        private static readonly string KeyString = "E01414CB-1828-4A06-8DB9-B384F1D0D9AD";

        #endregion

        #region Methods/Functions

        public static string GetEncrypt(string value)
        {
            return Encrypt(KeyString, value);
        }

        public static string GetDecrypt(string value)
        {
            return Decrypt(KeyString, value);
        }

        public static string Md5Encryption(string text)
        {
            MD5 md5 = MD5.Create();
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new();

            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return strBuilder.ToString();
        }


        private static string Encrypt(string strKey, string strData)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();
            var hashProvider = MD5.Create();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(strKey));

            var tdesAlgorithm = TripleDES.Create();

            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] dataToEncrypt = utf8.GetBytes(strData);

            try
            {
                ICryptoTransform encryptor = tdesAlgorithm.CreateEncryptor();
                results = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            return Convert.ToBase64String(results);
        }


        private static string Decrypt(string strKey, string strData)
        {
            byte[] results;
            UTF8Encoding utf8 = new UTF8Encoding();

            var hashProvider = MD5.Create();
            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(strKey));

            var tdesAlgorithm = TripleDES.Create();

            tdesAlgorithm.Key = tdesKey;
            tdesAlgorithm.Mode = CipherMode.ECB;
            tdesAlgorithm.Padding = PaddingMode.PKCS7;

            strData = strData.Replace(" ", "+");

            try
            {
                byte[] dataToDecrypt = Convert.FromBase64String(strData);

                ICryptoTransform decryptor = tdesAlgorithm.CreateDecryptor();
                results = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }

            return utf8.GetString(results);
        }

        #endregion
    }
}
