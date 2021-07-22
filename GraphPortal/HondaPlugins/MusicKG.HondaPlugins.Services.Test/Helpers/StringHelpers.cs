﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicKG.HondaPlugins.Services.Test.Helpers
{
    public class StringHelpers
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefjhijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
