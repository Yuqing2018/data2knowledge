using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MusicKG.Service.Helpers
{
    public class SecurityKeyHelper
    {
        private const string RSA_PRIVATE_KEY = "<RSAKeyValue><Modulus>yUYN466Se6Frvk04dnUy/TK5qPFo1LftX4XaPLAnjr9vA0LVLSQpAi0AbplB+7MO/GPmCEb3nxXH6hErZaIOlIA3BM6lBcDOIInRsGzK5jDCd1ZGTvx8HeFVMnY6CHF7Xk0ZsotYB2loQSjCkZbvBth02bpYMhZik/S1AKVkH8AnglBXrB7GlgV6UNjedNd56iLG0qMK8IlQSyvX8T5iKBQsYBCQ5pBigr2DACPeuwbWlAYkbJWBN6GzDk9HyJWyOzqtTCV+/fJ/XF2xRtiRMnNAjhS49bRJp3dVKyZnjm+X/6e/G8wck2nFKrzfXKqoLSupAb7GYYht4arU8CiGSw==</Modulus><Exponent>AQAB</Exponent><D>YjirooaYuXhmyfGJMlWzUlikvctBaz1uu4TooWEdjM3i8OZ2fZb95C/t3AsyZhBVqdlecn+fa6UHcDR8xxY0o0eGkfP+GDak4MAg8kXFJlx5Kh+SJA2WzEYge4WygFhQ6g4aEkVp6gQIenPXr5DGiMKtE2uaiqbd6HIsumn72HnhjUZkMw5GR5pi5DIwGG8uPQdbUdLlkyUE0zzJMcnf4YmL192tRFmh9wTBLutiYrsi6sT8J41nrYy+jiFiE64WONxcdeFTodznBnp+rhmq8UPBVJNrZZQniPlMemzp1nnRXFQ7WUjAV6hIo8jDbPjsg1KxdE34i2oygxUdCzdXUQ==</D><P>7PKEt+A0sEBk/oVWzZoFYkaku2gsCiZkRzGSPpxIqIlN73TN+vwzLRk+1ysIYgf8tcRBM++AzoHzou5RXT6+ofPgmtiGR+6erZZpJHn0yw4Gl9EV1MXZflEJ+hARg0oABIn1ba+bdhns7ZDPAuzD7e5YixaXclBoFgutT4baXX8=</P><Q>2XU0dJOfeX7H3Jzdfa3TPVaShG872siu35ISJK+HvvCd8sEEJDQL3Ub9WU8C6zmAxhLEjDxkWVsU41MkIMS4S4lWF11OnRd83AIKeRrBN9zf5zgAu4xPbU7EqEmTnemisa7+/N3u53/3X2vP3dT+zya0mPSu/3bXdGbXwUAYVTU=</Q><DP>m+CnoveicdjLGA3MHxIWPKLQuGE9p6e+DH/1/658tjMtzDwBTf4gBKk5CABNUYCDpWhopKwJBtHC4Ar8B+XBL3Xq6rNLE4NNbFEo9lAs1HBzsehg/kryXAEgGRv9CWvjOxkvFmuSUE2Z6md7aCcudhZ4tXe1RgTTlL011L2YZi0=</DP><DQ>ScIEYco0nEwRVFXslpVgAf0cQ0lLXwEUt4ZfGwximbQZjt5DHlh88bgDdMhk8A4YDJcPDZXqX5iSNCzeIzM2Y4CYr+IZKRVvZ1xV2w/lG6KSW2XWcca8phHCiGY0aKp3YZc+q9xHDUSNTlNONSrqpq61MPBFbycLG1NKuSSZXEU=</DQ><InverseQ>ZNCWI5MJf62iJkEAK3ac7wE5msvmIdA0iOiraHOZeGR4Iqq1FXZGUv6pKNpRSVWi+f7D8vO5zo9shDVEOBOYUcUU8PCcE9htNlQEcxfJ+3txJgWgBAg1hvRQCDhB3rhrpzOhi9kCuH6vw7Bn14PqRIvwmLq7r4yOEV7vFFqthCc=</InverseQ></RSAKeyValue>";
        private const string RSA_PUBLIC_KEY = "<RSAKeyValue><Modulus>yUYN466Se6Frvk04dnUy/TK5qPFo1LftX4XaPLAnjr9vA0LVLSQpAi0AbplB+7MO/GPmCEb3nxXH6hErZaIOlIA3BM6lBcDOIInRsGzK5jDCd1ZGTvx8HeFVMnY6CHF7Xk0ZsotYB2loQSjCkZbvBth02bpYMhZik/S1AKVkH8AnglBXrB7GlgV6UNjedNd56iLG0qMK8IlQSyvX8T5iKBQsYBCQ5pBigr2DACPeuwbWlAYkbJWBN6GzDk9HyJWyOzqtTCV+/fJ/XF2xRtiRMnNAjhS49bRJp3dVKyZnjm+X/6e/G8wck2nFKrzfXKqoLSupAb7GYYht4arU8CiGSw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// RSA private key
        /// </summary>
        public static SecurityKey RsaPrivateKey => new RsaSecurityKey(XmlStringToRsaParameter(RSA_PRIVATE_KEY));

        /// <summary>
        /// RSA public key
        /// </summary>
        public static SecurityKey RsaPublicKey => new RsaSecurityKey(XmlStringToRsaParameter(RSA_PUBLIC_KEY));

        /// <summary>
        /// Symmetric security key
        /// </summary>
        public static SecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes("diDIOHYUD=fdfji9208"));

        private static RSAParameters XmlStringToRsaParameter(string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            return parameters;
        }
    }
}
