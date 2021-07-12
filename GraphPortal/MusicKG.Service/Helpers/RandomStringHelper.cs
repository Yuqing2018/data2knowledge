using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.Service.Helpers
{
    public class RandomStringHelper
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefjhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomColorString()
        {
            Random randomNum_First = new Random((int)DateTime.Now.Ticks);
            System.Threading.Thread.Sleep(randomNum_First.Next(50));
            Random randomNum_Sencond = new Random((int)DateTime.Now.Ticks);

            int int_Red = randomNum_First.Next(256);
            int int_Green = randomNum_Sencond.Next(256);
            int int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            System.Drawing.Color color = System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue);
            string strColor = "#" + Convert.ToString(color.ToArgb(), 16).PadLeft(8, '0').Substring(2, 6);
            return strColor;
        }

    }
}
