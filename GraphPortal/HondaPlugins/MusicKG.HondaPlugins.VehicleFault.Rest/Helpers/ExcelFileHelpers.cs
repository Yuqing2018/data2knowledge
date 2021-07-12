using MusicKG.Scheduler.Service.Helpers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Net;

namespace MusicKG.HondaPlugins.VehicleFault.Rest.Helpers
{
    public class ExcelFileHelpers
    {
        public static List<Dictionary<string, object>> ProcessFile(System.IO.Stream stream, int titleIndex)
        {
            var keys = new List<string>();
            var result = new List<Dictionary<string, object>>();

            Utility.ExcelHelper.ReadExcel(stream, (rowIndex, row) =>
            {
                if (rowIndex < titleIndex)
                {
                    return null;
                }
                if (rowIndex == titleIndex)
                {
                    keys.AddRange(row);
                }
                else
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (row.Count <= i)
                        {
                            dictionary.Add(keys[i].Trim(), "");
                        }
                        else
                        {
                            dictionary.Add(keys[i].Trim(), row[i]);
                        }
                    }

                    result.Add(dictionary);
                }
                return null;
            });

            return result;
        }

        public static List<Dictionary<string, object>> ProcessFile(IFormFile file, int titleIndex)
        {
            var contentType = file.ContentType;

            if (contentType != "application/octet-stream" && contentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                ErrorHelper.ThrowException("You can only upload excel file.", HttpStatusCode.BadRequest);
            }

            var keys = new List<string>();
            var result = new List<Dictionary<string, object>>();

            Utility.ExcelHelper.ReadExcel(file.OpenReadStream(), (rowIndex, row) =>
            {
                if (rowIndex < titleIndex)
                {
                    return null;
                }
                if (rowIndex == titleIndex)
                {
                    keys.AddRange(row);
                }
                else
                {
                    var dictionary = new Dictionary<string, object>();

                    for (int i = 0; i < keys.Count; i++)
                    {
                        if (row.Count <= i)
                        {
                            dictionary.Add(keys[i].Trim(), "");
                        }
                        else
                        {
                            dictionary.Add(keys[i].Trim(), row[i]);
                        }
                    }

                    result.Add(dictionary);
                }
                return null;
            });

            return result;
        }
    }
}
