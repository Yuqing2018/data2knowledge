using MusicKG.DataAccess.Enums;
using MusicKG.WebApi.Contract.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Task result create binding model.
    /// </summary>
    public class TaskResultCreateBindingModel
    {
        /// <summary>
        /// Task document result.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Taskdocumentresult), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        public string Result { get; set; }

        /// <summary>
        /// Task document result type.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Taskdocumentresulttype), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        public TaskDocumentResultTypeEnum ResultType { get; set; }

        /// <summary>
        /// Task document status.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Taskdocumentstatus), ResourceType = typeof(Resources.DisplayNameResources))]
        public TaskDocumentStatusEnum Status { get; set; }
    }
}
