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

        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
