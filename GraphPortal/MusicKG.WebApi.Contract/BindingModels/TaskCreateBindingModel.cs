using MusicKG.WebApi.Contract.Constants;
using MusicKG.WebApi.Contract.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicKG.WebApi.Contract.BindingModels
{
    /// <summary>
    /// Task create binding model.
    /// </summary>
    public class TaskCreateBindingModel
    {
        /// <summary>
        /// Task name.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TaskName), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Overlap.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Overlap), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [RangeLocalized(0, 100)]
        public int Overlap { get; set; }

        /// <summary>
        /// Expected due at.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.ExpectedDueAt), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [DataType(DataType.DateTime)]
        public DateTime ExpectedDueAt { get; set; }

        /// <summary>
        /// Annotator ID list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.AnnotatorIDlist), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<string> AnnotatorIds { get; set; }

        /// <summary>
        /// Document ID list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DocumentIDlist), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<string> DocumentIds { get; set; }

        /// <summary>
        /// Is annotation result automatically approved.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Isannotationresultautomaticallyapproved), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        public bool IsAutoApproved { get; set; }

        /// <summary>
        /// Is knowledge automatically merged.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.Isknowledgeautomaticallymerged), ResourceType = typeof(Resources.DisplayNameResources))]
        [RequiredLocalized]
        public bool IsAutoMerged { get; set; }

        /// <summary>
        /// Dictionary id list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.DictionaryIDlist), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> DictionaryIds { get; set; }

        /// <summary>
        /// Task type.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.TaskType), ResourceType = typeof(Resources.DisplayNameResources))]
        public string TaskType { get; set; }

        /// <summary>
        /// Inspector id list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.InspectorIds), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> InspectorIds { get; set; }

        /// <summary>
        /// Acceptor id list.
        /// </summary>
        [Display(Name = nameof(Resources.DisplayNameResources.AcceptorIds), ResourceType = typeof(Resources.DisplayNameResources))]
        public List<string> AcceptorIds { get; set; }
    }
}
