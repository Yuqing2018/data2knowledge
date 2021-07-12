using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.Attributes
{
    public class RequiredLocalizedAttribute : RequiredAttribute
    {
        public RequiredLocalizedAttribute()
        {
            ErrorMessageResourceType = typeof(Resources.DataAnnotationsResources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotationsResources.RequiredAttribute_ValidationError);
        }
    }
}
