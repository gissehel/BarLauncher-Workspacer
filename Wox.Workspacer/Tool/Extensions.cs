using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Wox.Workspacer.Tool
{
    public static class Extensions
    {
        // white space, em-dash, en-dash, underscore
        private static readonly Regex WordDelimiters = new Regex(@"[\s—–_]", RegexOptions.Compiled);

        // characters that are not valid
        private static readonly Regex InvalidChars = new Regex(@"[^a-z0-9\-]", RegexOptions.Compiled);

        // multiple hyphens
        private static readonly Regex MultipleHyphens = new Regex(@"-{2,}", RegexOptions.Compiled);

        public static string ToSlug(this string value)
        {
            // convert to lower case
            value = value.ToLowerInvariant();

            // remove diacritics (accents)
            value = value.RemoveDiacritics();

            // ensure all word delimiters are hyphens
            value = WordDelimiters.Replace(value, "-");

            // strip out invalid characters
            value = InvalidChars.Replace(value, "");

            // replace multiple hyphens (-) with a single hyphen
            value = MultipleHyphens.Replace(value, "-");

            // Ok, this is just too much, but I prefer to do it again now.
            value = value.ToLowerInvariant();

            // trim hyphens (-) from ends
            return value.Trim('-');
        }

        /// See: http://www.siao2.com/2007/05/14/2629747.aspx
        private static string RemoveDiacritics(this string stIn)
        {
            string stFormD = stIn.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string ToSlug(this string value, string separator) => value.ToSlug().Replace("-", separator);
    }
}