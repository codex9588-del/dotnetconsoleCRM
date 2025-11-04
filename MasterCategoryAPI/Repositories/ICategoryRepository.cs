using MasterCategoryAPI.Models;

namespace MasterCategoryAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(long id);
        Task<Category> CreateAsync(Category category);
        Task<Category> UpdateAsync(Category category);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesWithParentAsync();
        Task<IEnumerable<Category>> GetChildCategoriesAsync(long parentId);
        Task<IEnumerable<Category>> GetHierarchicalCategoriesAsync();
    }
}