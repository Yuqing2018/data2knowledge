using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MusicKG.HondaPlugins.OperationTool.CommandActions
{
    public class Helpers
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefjhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Sha256Hash(string text)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);
            }
        }
    }
}
