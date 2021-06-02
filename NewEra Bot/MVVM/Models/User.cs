using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace NewEra_Bot.Models
{
    public class User
    {
        private readonly string name;

        private readonly string ReferalID;

        private readonly long ID_user;

        private readonly string API_Key;

        private readonly string Secret_Key;
        public decimal Balance { get; set; } = 0;
        public int CountSuccessOrders { get; set; } = 0;

        public decimal CountBNB { get; set; } = 0;

        public ObservableCollection<Order> ListNotCompleted { get; set; } = new ObservableCollection<Order>();

        public ObservableCollection<Order> ListCompleted { get; set; }  = new ObservableCollection<Order>();


        public User(string name, string ReferalID, long ID_user, string API_Key, string Secret_Key)
        {
            this.name = name;
            this.ReferalID = ReferalID;
            this.ID_user = ID_user;
            this.API_Key = API_Key;
            this.Secret_Key = Secret_Key;
        }

        public User(string path) // Извлечение данных из файла
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string Text = sr.ReadToEnd();
                string[] Lines = Text.Split('\n');
                name = Lines[0];
                ReferalID = Lines[1];
                ID_user = long.Parse(Lines[2]);
                API_Key = Lines[3];
                Secret_Key = Lines[4];
            }
        }

        public string GetName()
        {
            return name;
        }
        public string GetReferalID()
        {
            return ReferalID;
        }
        public long GetUserID()
        {
            return ID_user;
        }

        public string GetAPIKey()
        {
            return API_Key;
        }
        public string GetSecretKey()
        {
            return Secret_Key;
        }

    }
}
