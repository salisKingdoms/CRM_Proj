using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WS_CRM.Helper
{
    public class HelperStr
    {
        public static string newLineChar()
        {
            return "/r/n";
        }

        public static bool isEmptyOrNull(string data)
        {
            if (data == null || data.Trim().Length == 0)
                return true;
            return false;
        }

        public static string generateRandomStr(int length)
        {
            const string src = "abcdefghijklmnopqrstuvwxyz0123456789";
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }
            return sb.ToString().ToUpper();
        }

        public static string generateRandomStr(int seed, int length)
        {
            const string src = "abcdefghijklmnopqrstuvwxyz0123456789";
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }
            return sb.ToString().ToUpper();
        }

        public static string nvl(string data, string defaultValue)
        {
            if (data == null || data.Trim().Length == 0)
                return defaultValue;
            return data;
        }

        public static bool validateEmail(string email)
        {
            if (isEmptyOrNull(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[[0-9a-zA-Z\._]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool validateAlphabet(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^[a-zA-Z ]*$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }
        public static bool validateNumeric(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^[0-9]*$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }

        public static bool validateAlphaNumeric(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^[a-zA-Z0-9 ]*$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }

        public static bool validateAdress(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^[a-zA-Z0-9/-_,.()#|& ]*$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool validateName(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^[a-zA-Z0-9/-_,. ]*$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }

        public static bool validateMobilePhone(string data)
        {
            if (isEmptyOrNull(data))
                return false;
            try
            {
                return Regex.IsMatch(data,
                    "^(\\+62|62)?[\\s-]?0?8[1-9]{1}\\d{1}[\\s-]?\\d{3,8}$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

        }


        public static string limitString(string data, int length)
        {
            if (data != null)
            {
                if (data.Length > length)
                    return data.Substring(0, length);
                else
                    return data;
            }
            return null;
        }
    }
}
