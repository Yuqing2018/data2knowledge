using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.Attributes
{
    public class StringLengthLocalizedAttribute : StringLengthAttribute
    {
        public StringLengthLocalizedAttribute(int maximumLength) : base(maximumLength)
        {
            ErrorMessageResourceType = typeof(Resources.DataAnnotationsResources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotationsResources.StringLengthAttribute_ValidationError);
        }
    }
}
