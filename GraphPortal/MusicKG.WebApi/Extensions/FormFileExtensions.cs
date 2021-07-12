using Microsoft.AspNetCore.Http;
using MusicKG.Service.Constants;
using MusicKG.Service.Helpers;
using MusicKG.Service.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Extensions
{
    public static class FormFileExtensions
    {
        public static byte[] GetContent(this IFormFile formFile)
        {
            byte[] result;
            using (Stream stream = formFile.OpenReadStream())
            {
                result = new byte[stream.Length];
                stream.Read(result, 0, result.Length);
            }
            return result;
        }

        public static T ToObject<T>(this IFormFile file) where T : class
        {
            T result = null;
            if (file.ContentType.ToLower().Equals(HttpContentTypes.TextPlain) || 
                file.ContentType.ToLower().Equals(HttpContentTypes.ApplicationJson))
            {
                using (Stream stream = file.OpenReadStream())
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                    {
                        string text = reader.ReadToEnd();

                        try
                        {
                            //反序列化测试对象
                            result = JsonConvert.DeserializeObject<T>(text);
                        }
                        catch (JsonException)
                        {
                            ErrorHelper.ThrowException(MusicKGMessages.FileContentWrongMessage, HttpStatusCode.BadRequest);
                        }
                    }
                }
            }
            else
            {
                ErrorHelper.ThrowException(MusicKGMessages.FileFormatWrongMessage, HttpStatusCode.BadRequest);
            }
            return result;
        }

        public static List<string> SplitByLine(this IFormFile file, bool ignoreDuplicate = true)
        {
            List<string> result = new List<string>();
            using (Stream stream = file.OpenReadStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.Default))
                {
                    string line = reader.ReadLine();
                    while (line != null)
                    {
                        if (ignoreDuplicate)
                        {
                            if (!result.Contains(line.Trim()))
                            {
                                result.Add(line.Trim());
                            }
                        }
                        else
                        {
                            result.Add(line.Trim());
                        }
                        line = reader.ReadLine();
                    }
                }
            }
            return result;
        }
    }
}
