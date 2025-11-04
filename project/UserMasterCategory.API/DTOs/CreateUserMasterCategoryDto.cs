using System.ComponentModel.DataAnnotations;

namespace UserMasterCategory.API.DTOs;

public class CreateUserMasterCategoryDTO
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
    public string name { get; set; } = string.Empty;  // ✅ Initialize with empty string

    [Required(ErrorMessage = "Description is required")]
    [StringLength(250, MinimumLength = 1, ErrorMessage = "Description must be between 1 and 250 characters")]  // ✅ MinimumLength 20 se 1 kiya
    public string description { get; set; } = string.Empty;  // ✅ Initialize with empty string

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]  // ✅ Better than [Phone] attribute
    public string phone_number { get; set; } = string.Empty;  // ✅ Initialize with empty string
}