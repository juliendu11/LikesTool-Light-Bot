using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace LikesToolBotAPI.Classes
{
    public class Session
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double coins = 0;
        private double coinsEarned = 0;
        private bool logged = false;

        public string Email { get; set; }

        public string Password { get; set; }

        public bool Logged
        {
            get => logged;
            set
            {
                if (value!= logged)
                {
                    logged = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Logged"));
                }
            }
        }

        public double Coins 
        {
            get => coins;
            set
            {
                if (value != coins)
                {
                    coins = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Coins"));
                }
            }
        }

        public double CoinsEarned
        {
            get => coinsEarned;
            set
            {
                if (value != coinsEarned)
                {
                    coinsEarned = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CoinsEarned"));
                }
            }
        }

        public ProcessCount YoutubeViews { get; set; }

        public Session(string email, string password, double launchedCoint)
        {
            this.Email = email;
            this.Password = password;
            this.Coins = launchedCoint;
            this.Logged = true;
        }
    }
}
