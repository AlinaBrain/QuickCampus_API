using System.ComponentModel.DataAnnotations;

namespace QuickCampus_Core.ViewModel
{
    public class RoleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "RoleName is required.")]
        [MaxLength(30, ErrorMessage = "Name must be at most 30 characters long.")]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z\s]*$", ErrorMessage = "Only alphabetic characters are allowed in the name.")]
        public string? RoleName { get; set; }

        public List<int> RolePermission { get; set; }
    }
}
