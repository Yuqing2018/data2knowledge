using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MusicKG.Service.Helpers
{
    public class HashHelper
    {
        public static string GetMD5Hash(byte[] content)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(content);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public static string GetMD5HashBase64(byte[] content)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(content);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
