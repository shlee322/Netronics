using System;
using System.Text;
using Netronics.DB;
using Netronics.DB.Where;

namespace Netronics.Mobile.Auth
{
    class AuthProcessor
    {
        private const string CharPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz123456789";
        private static readonly Random Rnd = new Random();

        public static User NewAuthUser(string key)
        {
            var data = new User() {Key = key};
            data.Save();
            return data;
        }

        public static string GenerateKey()
        {
            return GetRandomString(256);
        }

        public static User GetUser(int id)
        {
            return (User)Model.ModelObjects<User>().Find(id);
        }

        private static string GetRandomString(int length)
        {
            var rs = new StringBuilder();

            while (length-- > 0)
            {
                rs.Append(CharPool[(int)(Rnd.NextDouble() * CharPool.Length)]);
            }

            return rs.ToString();
        }
    }
}
