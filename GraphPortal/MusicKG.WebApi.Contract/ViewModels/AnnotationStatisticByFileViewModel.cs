using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Annotation statistic by file view model.
    /// </summary>
    public class AnnotationStatisticByFileViewModel
    {
        /// <summary>
        /// Annotation statistic legend list.
        /// </summary>
        public IEnumerable<string> legends;

        /// <summary>
        /// Annotation statistic item list.
        /// </summary>
        public IEnumerable<IEnumerable<AnnotationStatisticItemViewModel>> items;
    }
}
