using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LikesToolBotAPI.Helpers
{
    public class StringTreatement
    {
        public static string DeleteWhiteSpace(string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }
    }
}
