using MasterCategoryAPI.Models;
using MasterCategoryAPI.Models.DTOs;

namespace MasterCategoryAPI.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(long id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createDto);
        Task<CategoryDto?> UpdateCategoryAsync(long id, UpdateCategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(long id);
        Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync();
        Task<IEnumerable<CategoryDto>> GetHierarchicalCategoriesAsync();
    }
}