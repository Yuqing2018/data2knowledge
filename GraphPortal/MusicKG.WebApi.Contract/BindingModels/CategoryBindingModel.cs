using MusicKG.WebApi.Contract.Attributes;
using MusicKG.WebApi.Contract.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MusicKG.WebApi.Contract.BindingModels
{
    public class CategoryBindingModel
    {
        /// <summary>
        /// ID.
        /// </summary>
        [Display(Name = "Category ID")]
        [StringLengthLocalized(ModelValidationConstant.ObjectIdLength, MinimumLength = ModelValidationConstant.ObjectIdLength)]
        public string Id { get; set; }

        /// <summary>
        /// Category name.
        /// </summary>
        [Display(Name = "Category name")]
        [Required]
        public string Name { get; set; }
    }
}
