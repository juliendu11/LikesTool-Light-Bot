using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikesToolBotAPI.API
{
    public interface ILikesToolBotAPI
    {
        Task<(bool result, string message)> LoginAsync();

        Task<(bool result, string message)> LogoutAsync();

        Task<(bool result, string message, int coins)> GetCoinsAsync();

        Processors.ISpecial Special { get; }

        Classes.Session Session { get; }
    }
}
