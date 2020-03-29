using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace LikesToolBotAPI.API.Builder
{
    public  class LikesToolBotAPIBuilder : ILikesToolBotAPIBuilder
    {
        private string email, password;

        private LikesToolBotAPIBuilder() { }



        public static ILikesToolBotAPIBuilder CreateBuilder()
        {
            return new LikesToolBotAPIBuilder();
        }

        public ILikesToolBotAPI Build()
        {
          
            return new LikesToolBotAPI(this.email, this.password);
        }

        public ILikesToolBotAPIBuilder SetAccount(string email, string password)
        {
            this.email = email;
            this.password = password;
            return this;
        }
    }
}
