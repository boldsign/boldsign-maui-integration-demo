using System.Text.RegularExpressions;

namespace BoldSignMauiIntegration.Extensions
{
    public static class URL
    {
        public static (bool isValid, string correctedUrl) ValidateAndCorrectUrl(this string url)
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return (true, url);
            }

            string corrected = url.Replace(@"\/", "/");

            if (Uri.IsWellFormedUriString(corrected, UriKind.Absolute))
            {
                return (true, corrected);
            }

            var s3Pattern = new Regex(@"^https:\\/\\/[\w\-]+\.s3\.[\w\-]+\.amazonaws\.com\\/[\w\-\.]+$");

            if (s3Pattern.IsMatch(url))
            {
                string s3Corrected = url.Replace(@"\", "");
                if (Uri.IsWellFormedUriString(s3Corrected, UriKind.Absolute))
                {
                    return (true, s3Corrected);
                }
            }

            return (false, url);
        }
    }
}
