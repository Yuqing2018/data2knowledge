using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.Resources;
using MusicKG.WebApi.Contract.Constants;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MusicKG.WebApi.Contract.BindingModels
{
    public class TaskCreationRuleUpdateBindingModel
    {
        [Display(Name = nameof(DisplayNameResources.TaskCreationUser), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string CreateUser { get; set; }

        [Display(Name = nameof(DisplayNameResources.AutoTaskCreationRules), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        [MinLengthLocalized(1)]
        public List<AutoTaskRuleUpdateBindingModel> Rules { get; set; }
    }

    public class AutoTaskRuleUpdateBindingModel
    {
        [RequiredLocalized]
        [StringLengthLocalized(ModelValidationConstant.MaxNameLength)]
        public string Name { get; set; }

        [Display(Name = nameof(DisplayNameResources.DocumentTags), ResourceType = typeof(DisplayNameResources))]
        public List<string> DocumentTags { get; set; }

        [Display(Name = nameof(DisplayNameResources.AnnotatorIDlist), ResourceType = typeof(DisplayNameResources))]
        [MinLengthLocalized(1)]
        public List<string> Annotators { get; set; }

        [Display(Name = nameof(DisplayNameResources.Overlap), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        [RangeLocalized(0, 100)]
        public int Overlap { get; set; } = 0;

        [Display(Name = nameof(DisplayNameResources.Isannotationresultautomaticallyapproved), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        public bool IsAutoApproved { get; set; } = true;

        [Display(Name = nameof(DisplayNameResources.Isknowledgeautomaticallymerged), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        public bool IsAutoMerged { get; set; } = true;

        [Display(Name = nameof(DisplayNameResources.MaxFinishDays), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        [RangeLocalized(1, int.MaxValue)]
        public int MaxFinishDays { get; set; } = 365;

        [Display(Name = nameof(DisplayNameResources.DocumentCount), ResourceType = typeof(DisplayNameResources))]
        [RequiredLocalized]
        [RangeLocalized(1, int.MaxValue)]
        public int DocumentCount { get; set; } = 1;
    }
}
