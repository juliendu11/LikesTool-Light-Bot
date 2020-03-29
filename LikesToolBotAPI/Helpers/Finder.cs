using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace LikesToolBotAPI.Helpers
{
    public static class Finder
    {
        /// <summary>
        /// Get home coins and delete white space
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string GetCoins(string body)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(body);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"page-content-wrapper\"]/div[1]/div[2]/div[1]/p/span");

            return StringTreatement.DeleteWhiteSpace(node.InnerText);
        }

        public static string GetCoinsHeader(string body)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(body);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"navbar\"]/ul[2]/li[1]/a/i");

            return StringTreatement.DeleteWhiteSpace(node.InnerText);
        }

        public static string GetToken(string responseBody)
        {
            string csrfToken = string.Empty;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(responseBody);

            var list = htmlDoc.DocumentNode.SelectNodes("//meta[@name='csrf-token']");
            foreach (var node in list)
            {
                csrfToken = node.GetAttributeValue("content", "");
            }

            return csrfToken;
        }
    }
}
