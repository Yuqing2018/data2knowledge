using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Annotation statistic item view model.
    /// </summary>
    public class AnnotationStatisticItemViewModel
    {
        /// <summary>
        /// Date.
        /// </summary>
        public DateTime date;


        /// <summary>
        /// The count of files.
        /// </summary>
        public int fileCount;
    }
}
