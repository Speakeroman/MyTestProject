using System.Web;

namespace MyTestProject.Utils
{
    public static class TextUtils
    {
        public static string EncodeText(string text)
        {
            string returntext = HttpUtility.UrlEncode(text);
            return returntext;
        }
    }
}
