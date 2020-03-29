using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LikesToolBotAPI.API.Processors
{
    /// <summary>
    /// Contains processes that do not need to have an account on a connected social network
    /// </summary>
    public class Special : ISpecial
    {
        private readonly HttpClient client;
        private readonly Classes.Session session;

        public Special(HttpClient client, Classes.Session session)
        {
            this.client = client;
            this.session = session;
        }

        /// <summary>
        /// Watch youtube videos to earn points with 45 second wait time, no account needed
        /// </summary>
        /// <param name="videoCountStack">Parameter to make the process in loop, you can also put 1 and make a loop outside</param>
        /// <returns>If there is an error the whole process is stopped and an error message is transmitted otherwise it waits for the end of the loop</returns>
        public async Task<(bool result,string message)> ViewYoutubeVideo(int videoCountStack)
        {
            if (session.YoutubeViews == null) session.YoutubeViews = new Classes.ProcessCount();

            for (int i=0; i< videoCountStack; i++)
            {
                double pointToEarn = 0;
                string videoId = string.Empty;
                string csrfToken = string.Empty;
                string id = string.Empty;
                string t = string.Empty;

                var firstRequest = await client.GetAsync(Constant.YOUTUBE_VIEWS);
                string responseBody = await firstRequest.Content.ReadAsStringAsync();

                pointToEarn = double.Parse(Helpers.StringTreatement.DeleteWhiteSpace(Helpers.Splitter.ElementBetween("<p class=\"like-txt\">Get <span class=\"coins_color_red\">", " coins</span> for viewing this Youtube video</p>", responseBody)), CultureInfo.InvariantCulture);
                videoId = Helpers.Splitter.ElementBetween("<a class=\"campaign_button bg_gray btn btn-black form-group\" onclick=\"record.skip(", ")\">Skip</a>", responseBody);
                id = Helpers.StringTreatement.DeleteWhiteSpace(videoId.Split(',')[0]);
                csrfToken = Helpers.Finder.GetToken(responseBody);

                //var getCountRequest = new HttpRequestMessage
                //{
                //    Method = HttpMethod.Post,
                //    RequestUri = new Uri(Constant.CAMPAIGN + "get-count"),
                //    Headers =
                //    {
                //        {"Host", "likestool.com" },
                //        {"Connection", "keep-alive" },
                //        {"Content-Length", $"id={id}".Length.ToString() },
                //       {"Accept", "application/json, text/javascript, */*; q=0.01" },
                //       {"Sec-Fetch-Dest","empty" },
                //       {"X-CSRF-Token",csrfToken },
                //       {"X-Requested-With","XMLHttpRequest" },
                //       {"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36" },
                //       {HttpRequestHeader.ContentType.ToString(),"application/x-www-form-urlencoded" },
                //       {"Origin","https://likestool.com" },
                //       {"Sec-Fetch-Site","same-origin" },
                //       {"Sec-Fetch-Mode","cors" },
                //       {"Referer",Constant.YOUTUBE_VIEWS },
                //       {"Accept-Encoding","gzip, deflate, br" },
                //       {"Accept-Language","fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7" }
                //    },
                //    Content = new FormUrlEncodedContent(new[]
                //     {
                //new KeyValuePair<string,
                //string>("id",id)
                //      })
                //};


                var content = new FormUrlEncodedContent(new[]{new KeyValuePair<string,string>("id",id)});
                content.Headers.Add("X-CSRF-Token", csrfToken);

                var countRequest = await client.PostAsync(Constant.CAMPAIGN + "get-count", content);

                if (!countRequest.IsSuccessStatusCode)
                {
                    session.YoutubeViews.Error++;
                    return (false, "Bad request");
                }

                await Task.Delay(TimeSpan.FromSeconds(50));


                t = Helpers.StringTreatement.DeleteWhiteSpace(videoId.Split(',')[1]);


                //var checkCampaignRequest = new HttpRequestMessage
                //{
                //    Method = HttpMethod.Post,
                //    RequestUri = new Uri(Constant.CAMPAIGN + "check-campaign"),
                //    Headers =
                //    {
                //        {"Host", "likestool.com" },
                //        {"Connection", "keep-alive" },
                //        {"Content-Length", $"id={id}&t={t}".Length.ToString() },
                //       {"Accept", "text/html, */*; q=0.01" },
                //       {"Sec-Fetch-Dest","empty" },
                //       {"X-CSRF-Token",csrfToken },
                //       {"X-Requested-With","XMLHttpRequest" },
                //       {"User-Agent","Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36" },
                //       {HttpRequestHeader.ContentType.ToString(),"application/x-www-form-urlencoded" },
                //       {"Origin","https://likestool.com" },
                //       {"Sec-Fetch-Site","same-origin" },
                //       {"Sec-Fetch-Mode","cors" },
                //       {"Referer",Constant.YOUTUBE_VIEWS },
                //       {"Accept-Encoding","gzip, deflate, br" },
                //       {"Accept-Language","fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7" }
                //    },
                //    //Content = new FormUrlEncodedContent(new Dictionary<string, string>
                //    //{
                //    //    {"id", id },
                //    //    {"t",t }
                //    //})
                //    Content = new FormUrlEncodedContent(new[]
                //    {
                //             new KeyValuePair<string, string>("id",id),
                //             new KeyValuePair<string, string>("t",t)
                //         })
                //};

                var content2 = new FormUrlEncodedContent(new[] 
                { 
                    new KeyValuePair<string, string>("id", id),
                    new KeyValuePair<string, string>("t", t) 
                });
                content2.Headers.Add("X-CSRF-Token", csrfToken);


                var checkCampaign =  await client.PostAsync(Constant.CAMPAIGN + "check-campaign",content2);

                if (!checkCampaign.IsSuccessStatusCode)
                {
                    session.YoutubeViews.Error++;
                    return (false, "Bad request");
                }

                session.YoutubeViews.Success++;

                session.CoinsEarned = pointToEarn;
                session.Coins += pointToEarn;
            }

            return (true, string.Empty);
        }
    }
}
