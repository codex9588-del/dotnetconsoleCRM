using System.ComponentModel.DataAnnotations;

namespace UserMasterCategory.API.DTOs;

public class UpdateUserMasterCategoryDTO
{
    [StringLength(100, MinimumLength = 1)]
    public string? name { get; set; } = string.Empty;

    [StringLength(250)]
    public string? description { get; set; }

    [EmailAddress]
    public string email { get; set; } = string.Empty;

    public int? role_id { get; set; }

    [Phone]
    public string phone_number { get; set; } = string.Empty;


    public bool? is_active { get; set; }
}