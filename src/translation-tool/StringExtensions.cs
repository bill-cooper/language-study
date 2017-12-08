using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace translation_tool
{
    public static class StringExtensions
    {
        public static string RemoveAccents(this string word) {
            return word
                    .Replace("а́", "а")
                    .Replace("е́", "е")
                    .Replace("у́", "у")
                    .Replace("о́", "о")
                    .Replace("ю́", "ю")
                    .Replace("ы́", "ы")
                    .Replace("и́", "и")
                    .Replace("э́", "э")

                    ;
        }
    }
}
