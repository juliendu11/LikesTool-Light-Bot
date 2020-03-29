using System;
using System.Collections.Generic;
using System.Text;

namespace LikesToolBotAPI.API.Builder
{
    public interface ILikesToolBotAPIBuilder
    {
        ILikesToolBotAPIBuilder SetAccount(string email, string password);

        ILikesToolBotAPI Build();
    }
}
