using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.Attributes
{
    public class MinLengthLocalizedAttribute : MinLengthAttribute
    {
        public MinLengthLocalizedAttribute(int length) : base(length)
        {
            ErrorMessageResourceType = typeof(Resources.DataAnnotationsResources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotationsResources.MinLengthAttribute_ValidationError);
        }
    }
}
