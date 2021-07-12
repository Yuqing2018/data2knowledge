using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace MusicKG.HondaPlugins.DataAccess.Extensions
{
    public static class ResourcesExtentions
    {
        public static IEnumerable<KeyValuePair<string, string>> GetAllResources(ResourceManager resourceManager)
        {
            ResourceSet resourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);

            IDictionaryEnumerator dictNumerator = resourceSet.GetEnumerator();

            // Get all string resources
            while (dictNumerator.MoveNext())
            {
                // Only string resources
                if (dictNumerator.Value is string)
                {
                    var key = (string)dictNumerator.Key;
                    var value = (string)dictNumerator.Value;
                    yield return new KeyValuePair<string, string>(key, value);
                }
            }
        }
    }
}
