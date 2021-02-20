using AppLogger;
using DBHelper.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils
{
    public static class Utilities
    {
        public static AppSetting GetAppSettings()
        {
            using (var context = new DBLAccountOpeningContext())
            {
                return context.AppSettings.FirstOrDefault();
            }
        }

        public static string EncodeBase64(string rawString)
        {
            try
            {
                if (string.IsNullOrEmpty(rawString))
                {
                    return string.Empty;
                }
                var plainTextBytes = Encoding.UTF8.GetBytes(rawString);
                string encodedText = Convert.ToBase64String(plainTextBytes);
                return encodedText;
            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }

        }
        public static string DecodeBase64(string encrypted)
        {
            try
            {
                var encodedTextBytes = Convert.FromBase64String(encrypted);
                string plainText = Encoding.UTF8.GetString(encodedTextBytes);
                return plainText;

            }
            catch (Exception ex)
            {

                Logger.Instance.logError(ex);
                return null;
            }
        }

    }
}
