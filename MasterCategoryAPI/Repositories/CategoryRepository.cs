using Microsoft.EntityFrameworkCore;
using MasterCategoryAPI.Models;
using MasterCategoryAPI.Data;

namespace MasterCategoryAPI.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(c => c.Parent)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(long id)
        {
            return await _context.Categories
                    .Include(c => c.Parent)
                    .Include(c => c.Children)
                    .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = GenerateSlug(category.Name ?? "category");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrEmpty(category.Slug))
            {
                category.Slug = GenerateSlug(category.Name ?? "category");
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;
            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(long id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Parent)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithParentAsync()
        {
            return await _context.Categories
                .Include(c => c.Parent)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetChildCategoriesAsync(long parentId)
        {
            return await _context.Categories
                .Where(c => c.ParentId == parentId && c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetHierarchicalCategoriesAsync()
        {
            var allCategories = await _context.Categories
                .Include(c => c.Children.Where(child => child.IsActive))
                    .ThenInclude(child => child.Children.Where(grandChild => grandChild.IsActive)) // ✅ GRANDCHILDREN
                .Where(c => c.IsActive)
                .ToListAsync();
            foreach (var category in allCategories)
            {
                Console.WriteLine($"Category: {category.Name}, Children: {category.Children.Count}");
                foreach (var child in category.Children)
                {
                    Console.WriteLine($"  - Child: {child.Name}, Grandchildren: {child.Children.Count}");
                }
            }
            return allCategories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.DisplayOrder)
                .ToList();
        }
        private string GenerateSlug(string name)
        {
            if (string.IsNullOrEmpty(name))
                return "category";

            return name.ToLower()
                      .Replace(" ", "-")
                      .Replace("--", "-")
                      .Trim('-');
        }
    }
}