using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Pagination view model.
    /// </summary>
    public class PaginationViewModel<T>
    {
        /// <summary>
        /// Total item count.
        /// </summary>
        [Display(Name = "Total item count")]
        public long TotalCount;

        /// <summary>
        /// Start item index.
        /// </summary>
        [Display(Name = "Start item index")]
        public int From;

        /// <summary>
        /// Item count.
        /// </summary>
        [Display(Name = "Item count")]
        public int Count;

        /// <summary>
        /// Items.
        /// </summary>
        [Display(Name = "Items")]
        public IEnumerable<T> Items;
    }
}
