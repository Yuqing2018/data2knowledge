using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Annotation entity statistic view model.
    /// </summary>
    public class AnnotationEntityStatisticViewModel
    {
        /// <summary>
        /// Enity count.
        /// </summary>
        public int EntityCount;

        /// <summary>
        /// Relationship count.
        /// </summary>
        public int RelationshipCount;

        /// <summary>
        /// Property count.
        /// </summary>
        public int PropertyCount;
    }
}
