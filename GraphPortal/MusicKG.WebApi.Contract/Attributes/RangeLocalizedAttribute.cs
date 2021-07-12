using System.ComponentModel.DataAnnotations;

namespace MusicKG.WebApi.Contract.Attributes
{
    public class RangeLocalizedAttribute : RangeAttribute
    {
        public RangeLocalizedAttribute(int minimum, int maximum) : base(minimum, maximum)
        {
            ErrorMessageResourceType = typeof(Resources.DataAnnotationsResources);
            ErrorMessageResourceName = nameof(Resources.DataAnnotationsResources.RangeAttribute_ValidationError);
        }
    }
}
