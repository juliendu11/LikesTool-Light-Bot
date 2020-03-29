using HtmlAgilityPack;
using LikesToolBotAPI.API.Processors;
using LikesToolBotAPI.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LikesToolBotAPI.API
{
    public class LikesToolBotAPI : ILikesToolBotAPI
    {
        private readonly string email;
        private readonly string password;
        private readonly HttpClient client;

        private readonly CookieContainer cookies;

        public LikesToolBotAPI(string email, string password)
        {
            this.email = email;
            this.password = password;

            cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler
            {
                CookieContainer = cookies
            };


            this.client = new HttpClient(handler);
           this. client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
            this.client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.132 Safari/537.36");
            this. client.BaseAddress = new Uri(Constant.BASE_URI);
        }

        public ISpecial Special { get; private set; }

        public Session Session { get; private set; }

        public Task<(bool result, string message, int coins)> GetCoinsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<(bool result, string message)> LoginAsync()
        {
           var getToken = await FirstRequestAsync();

            if (string.IsNullOrEmpty(getToken))
            {
                return (false, "Empty token");
            }
           

            var content = new FormUrlEncodedContent(new[]
          {
                new KeyValuePair<string, string>("_csrf",getToken),
                new KeyValuePair<string, string>("LoginForm[username]",this.email),
                new KeyValuePair<string, string>("LoginForm[password]",this.password),
                new KeyValuePair<string, string>("LoginForm[rememberMe]","0")
            });


            HttpResponseMessage response = await client.PostAsync("site/login", content);

            string responseBody = await response.Content.ReadAsStringAsync();


            if (responseBody.Contains("Username or email cannot be blank.") || responseBody.Contains("Password cannot be blank."))
            {
                return (false, responseBody.Contains("Username or email cannot be blank.") ? "Username or email is empty": "Password is empty");
            }

            if (!response.IsSuccessStatusCode)
            {
                return (false, response.StatusCode.ToString());
            }

            Session = new Classes.Session(this.email, this.password, double.Parse(Helpers.Finder.GetCoins(responseBody), CultureInfo.InvariantCulture));

            ActiveProcessor();


            return (true, Helpers.Finder.GetCoins(responseBody));
        }

        public async Task<(bool result, string message)> LogoutAsync()
        {
            HttpResponseMessage response = await client.GetAsync("site/logout");
            if (!response.IsSuccessStatusCode)
            {
                return (false, response.StatusCode.ToString());
            }
            return (true, string.Empty);
        }

        /// <summary>
        /// Active all processor if the account is well connected
        /// </summary>
        private void ActiveProcessor()
        {
            this.Special = new Special(this.client,this.Session);
        }

        /// <summary>
        /// To fill cookies
        /// </summary>
        /// <returns></returns>
        private async Task<string> FirstRequestAsync()
        {
            string token = "";

            HttpResponseMessage response = await client.GetAsync("site/login");
            string responseBody = await response.Content.ReadAsStringAsync();

            token = Helpers.Finder.GetToken(responseBody);

            return token;
        }
    }
}
