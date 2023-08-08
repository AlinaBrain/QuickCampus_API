
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickCampus_Core.Common
{
    public class PercentageRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (value is decimal percentage)
            {
                if (percentage < 0 || percentage > 100)
                {
                    return new ValidationResult("Percentage must be between 0 and 100.");
                }
            }

            return ValidationResult.Success;
        }

    }
}