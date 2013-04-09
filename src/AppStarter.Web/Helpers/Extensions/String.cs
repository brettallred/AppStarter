using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AppStarter.Helpers.Extensions
{

    public static partial class ExtensionMethods
    {
        private static readonly Regex Humps = new Regex("(?:^[a-zA-Z][^A-Z]*|[A-Z][^A-Z]*)");


        public static readonly Regex UrlRegex =
            new Regex(@"((https?|ftp|gopher|telnet|file|notes|ms-help):((//)|(\\\\))+[\w\d:#@%/;$()~_?\+-=\\\.&]*)",
                      RegexOptions.Compiled);

        public static string Ellipsize(this string text, int characterCount)
        {
            return text.Ellipsize(characterCount, "...");
        }

        public static string Ellipsize(this string text, int characterCount, string ellipsis)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            if (characterCount < 0 || text.Length <= characterCount)
                return text;

            return Regex.Replace(text.Substring(0, characterCount + 1), @"\s+\S*$", "") + ellipsis;
        }

        public static string HtmlClassify(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            string friendlier = text.CamelFriendly();
            return Regex.Replace(friendlier, @"[^a-zA-Z]+", m => m.Index == 0 ? "" : "-").ToLowerInvariant();
        }

        public static string CamelFriendly(this string camel)
        {
            if (string.IsNullOrWhiteSpace(camel))
                return "";

            IEnumerable<string> matches = Humps.Matches(camel).OfType<Match>().Select(m => m.Value);
            return matches.Any()
                       ? matches.Aggregate((a, b) => a + " " + b).TrimStart(' ')
                       : camel;
        }

        public static string Truncate(this string s, int length)
        {
            if (s.Length > length) return s.Substring(0, length);
            return s;
        }

        public static string AppendQueryParameter(this string url, string key, string value)
        {
            if (url.IndexOf('?') == url.Length - 1)
            {
                return url + string.Format("{0}={1}", key, value);
            }

            if (url.Contains('?'))
            {
                return url + string.Format("&{0}={1}", key, value);
            }

            if (url.EndsWith("/"))
            {
                return url.TrimEnd('/') + string.Format("?{0}={1}", key, value);
            }

            return url + string.Format("?{0}={1}", key, value);
        }

        public static string ToDollarString(this string data)
        {
            return Convert.ToDecimal(data).ToDollarString();
        }

        public static string ToDollarString(this decimal data)
        {
            data = Decimal.Round(data, 2, MidpointRounding.AwayFromZero);

            return String.Format("{0:C}", data);
        }


        public static string ToDollarString(this double data)
        {
            return String.Format("{0:C}", data);
        }


        public static string ToMoneyDecimal(this string data)
        {
            return Convert.ToDecimal(data).ToDollarString();
        }

        public static string ToMoneyDecimal(this decimal data)
        {
            return String.Format("{0:f}", data);
        }


        public static string ToMoneyDecimal(this double data)
        {
            return String.Format("{0:f}", data);
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Determines if a given string is not null or empty string
        /// </summary>
        /// <param name="string">String to test</param>
        /// <returns>False if null or empty. True otherwise.</returns>
        public static bool IsNotNullOrEmpty(this string @string)
        {
            return !string.IsNullOrEmpty(@string);
        }


        /// <summary>
        /// Determines if a given string is a URL
        /// </summary>
        /// <param name="string">Strign to test for URL match</param>
        /// <returns>True if value is a URL, false otherwise</returns>
        public static bool IsUrl(this string @string)
        {
            return @string.IsNotNullOrEmpty() && UrlRegex.IsMatch(@string);
        }

        /// <summary>
        /// Removes a specified string from a string
        /// </summary>
        /// <param name="string">String from which to remove substrings</param>
        /// <param name="substrings">Strings to remove from origal string</param>
        /// <returns>String without all substrings specified</returns>
        public static string Without(this string @string, params string[] substrings)
        {
            if (@string.IsNullOrEmpty() || substrings == null || substrings.Length == 0)
                return string.Empty;

            return substrings.Where(s => s.IsNotNullOrEmpty())
                .Aggregate(@string, (orig, without) => orig.Replace(without, string.Empty));
        }

    }
}