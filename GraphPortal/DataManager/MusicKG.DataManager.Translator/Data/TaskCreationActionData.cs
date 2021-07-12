using MusicKG.DataManager.Models;
using System.Collections.Generic;

namespace MusicKG.DataManager.Translator.Data
{
    public class TaskCreationActionData
    {
        public IEnumerable<SimpleDocumentModel> Documents { get; set; }
    }
}
