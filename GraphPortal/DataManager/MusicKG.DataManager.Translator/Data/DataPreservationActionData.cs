using MusicKG.DataManager.Translator.Models;
using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Data
{
    public class DataPreservationActionData
    {
        /// <summary>
        /// Dictionary
        /// key: tags split by comma. 
        /// Value: document dictionary.
        ///     key: document file name.
        ///     Value: document file content.
        /// </summary>
        public IEnumerable<DataPreservationModel> Documents { get; set; }
    }
}
