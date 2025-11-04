using MasterCategoryAPI.Models;
using MasterCategoryAPI.Models.DTOs;
using MasterCategoryAPI.Repositories;

namespace MasterCategoryAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(MapToDto);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(long id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null ? MapToDto(category) : null;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto)
        {
            if (createDto == null)
                throw new ArgumentNullException(nameof(createDto));

            var category = new Category
            {
                Name = createDto.Name ?? string.Empty,
                Slug = createDto.Slug,
                ParentId = createDto.ParentId,
                DisplayOrder = createDto.DisplayOrder,
                Description = createDto.Description,
                IsActive = true,
            };

            var created = await _categoryRepository.CreateAsync(category);
            return MapToDto(created);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(long id, UpdateCategoryDto updateDto)
        {
            if (updateDto == null)
                throw new ArgumentNullException(nameof(updateDto));

            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return null;

            existingCategory.Name = updateDto.Name ?? string.Empty;
            existingCategory.Slug = updateDto.Slug;
            existingCategory.ParentId = updateDto.ParentId;
            existingCategory.DisplayOrder = updateDto.DisplayOrder;
            existingCategory.Description = updateDto.Description;
            existingCategory.IsActive = updateDto.IsActive;

            var updated = await _categoryRepository.UpdateAsync(existingCategory);
            if (updated == null) return null;
            return MapToDto(updated);
        }

        public async Task<bool> DeleteCategoryAsync(long id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
        {
            var categories = await _categoryRepository.GetActiveCategoriesAsync();
            return categories.Select(MapToDto);
        }

        public async Task<IEnumerable<CategoryDto>> GetHierarchicalCategoriesAsync()
        {
            var categories = await _categoryRepository.GetHierarchicalCategoriesAsync();
            return categories.Select(MapToDtoWithChildren);
        }

        private CategoryDto MapToDto(Category category)
        {
            if (category == null) return new CategoryDto();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name ?? string.Empty,
                Slug = category.Slug,
                ParentId = category.ParentId,
                ParentName = category.Parent?.Name,
                DisplayOrder = category.DisplayOrder,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Children = new List<CategoryDto>(),
            };
        }
        private CategoryDto MapToDtoWithChildren(Category category)
        {
            if (category == null) return new CategoryDto();
            var dto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name ?? string.Empty,
                Slug = category.Slug,
                ParentId = category.ParentId,
                ParentName = category.Parent?.Name,
                DisplayOrder = category.DisplayOrder,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                Children = new List<CategoryDto>()
            };
            if (category.Children != null && category.Children.Any())
            {
                dto.Children = category.Children
                .Where(child => child.IsActive)
                .Select(MapToDtoWithChildren)
                .OrderBy(child => child.DisplayOrder)
                .ThenBy(child => child.Name)
                .ToList();
            }

            return dto;
        }
    }
}