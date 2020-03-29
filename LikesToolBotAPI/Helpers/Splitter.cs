using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikesToolBotAPI.Helpers
{
    public class Splitter
    {
        public static string ElementBetween(string start, string stop, string body)
        {
            int pFrom = body.IndexOf(start) + start.Length;
            int pTo = body.LastIndexOf(stop);

            return body.Substring(pFrom, pTo - pFrom);
        }
    }
}
