using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Strings
{
    public static class StringExtras
    {
        /// <summary>
        /// Splits a string but keeps the delimiter.
        /// </summary>
        public static IEnumerable<string> SplitAndKeep(this string pStr, string pDelimiter)
        {
            List<string> strings = new List<string>();
            int i;
            while ((i = pStr.IndexOf(pDelimiter)) != -1)
            {
                if (i != 0)
                {
                    strings.Add(pStr.Substring(0, i));
                }
                strings.Add(pStr.Substring(i, pDelimiter.Length));
                pStr = pStr.Substring(i + pDelimiter.Length);
            }
            if (pStr != "")
            {
                strings.Add(pStr);
            }
            return strings;
        }

        /// <summary>
        /// Ensures that a string is never longer than.
        /// </summary>
        public static string MaxLength(this string pStr, int pLength)
        {
            if (pStr == null || pStr.Length < pLength)
            {
                return pStr;
            }
            return pStr.Substring(0, pLength);
        }

        /// <summary>
        /// Encodes a string a UTF8
        /// </summary>
        public static string ToUTF8(this string pStr)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(pStr));
        }
    }
}
