using System.Collections.Generic;

namespace MusicKG.Service.Models
{
    public class DictionaryUpdateServiceModel
    {
        public string Name { get; set; }

        public bool IsNameAssigned { get; set; }
        
        public string EntityId { get; set; }

        public bool IsEntityIdAssigned { get; set; }

        public List<string> Vocabularies { get; set; }
        
        public bool IsVocabulariesAssigned { get; set; }
    }
}
