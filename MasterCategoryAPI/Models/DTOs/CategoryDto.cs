namespace MasterCategoryAPI.Models.DTOs
{
    public class CategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public long? ParentId { get; set; }
        public string? ParentName { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CategoryDto> Children { get; set; } = new();
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Slug { get; set; }
        public long? ParentId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string? Description { get; set; }
    }

    public class UpdateCategoryDto
    {
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public long? ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}