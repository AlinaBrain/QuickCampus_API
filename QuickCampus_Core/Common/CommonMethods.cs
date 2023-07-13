using System.Text;

namespace QuickCampus_Core.Common
{
    public static class CommonMethods
    {
        public static string Key = "afsr@@het@#$@@";
        public static string ConvertToEncrypt(string Password)
        {
            if (string.IsNullOrEmpty(Password)) return "";

            {
                Password += Key;
                var PasswordBytes = Encoding.UTF8.GetBytes(Password);
                return Convert.ToBase64String(PasswordBytes);
            }
        }

        public static string ConvertToDecrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }
    }
}
