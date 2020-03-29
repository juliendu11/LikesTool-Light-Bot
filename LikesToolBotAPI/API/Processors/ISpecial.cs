using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LikesToolBotAPI.API.Processors
{
    /// <summary>
    /// Contains processes that do not need to have an account on a connected social network
    /// </summary>
    public interface ISpecial
    {
        /// <summary>
        /// Watch youtube videos to earn points with 45 second wait time, no account needed
        /// </summary>
        /// <param name="videoCount">Parameter to make the process in loop, you can also put 1 and make a loop outside</param>
        /// <returns>If there is an error the whole process is stopped and an error message is transmitted otherwise it waits for the end of the loop</returns>
        Task<(bool result, string message)> ViewYoutubeVideo(int videoCount);
    }
}
