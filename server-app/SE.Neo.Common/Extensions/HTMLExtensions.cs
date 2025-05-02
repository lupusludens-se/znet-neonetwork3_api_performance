using System.Text.RegularExpressions;

namespace SE.Neo.Common.Extensions
{
    public static class HTMLExtensions
    {
        public static string RemoveAllHTML(this string originalString)
        {
            string pattern = "<a\\s+[^>]*href=[\"'](?:[^\"'>]*\\/)?(?<=\\b)([^\"'>]*\\.wav)(?=\\b)[\"'][^>]*>(.*?)<\\/a>";
            string result = Regex.Replace(originalString, pattern, string.Empty);// remove audio files url from keyword search 'neo'

            result = Regex.Replace(result, "<[^>]*(>|$)", string.Empty);
            result = result.Replace("[neo_video]", string.Empty);
            result = result.Replace("[neo_pdf]", string.Empty);

            return result;
        }
    }
}
