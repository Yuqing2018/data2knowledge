using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.ViewModels
{
    /// <summary>
    /// Category list item view model.
    /// </summary>
    public class CategoryListItemViewModel
    {
        /// <summary>
        /// Category ID.
        /// </summary>
        [Display(Name = "Cateory ID")]
        public string Id { get; set; }
        /// <summary>
        /// Workspace ID.
        /// </summary>
        [Display(Name = "Workspace ID")]
        public string WorkspaceId { get; set; }
        /// <summary>
        /// Name.
        /// </summary>
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
