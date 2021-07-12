using MongoDB.Bson;
using MusicKG.HondaPlugins.DataAccess.Enums;

namespace MusicKG.HondaPlugins.Services.Models
{
    public class SyndromeServiceModel
    {
        public ObjectId MongoId
        {
            get
            {
                return new ObjectId(Id);
            }
            set
            {
                Id = value.ToString();
            }
        }

        public string Id { get; private set; }

        public string Name { get; set; }

        public BadGrade BadGrade { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SyndromeServiceModel)
                return (obj as SyndromeServiceModel).Name == Name;

            return false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
