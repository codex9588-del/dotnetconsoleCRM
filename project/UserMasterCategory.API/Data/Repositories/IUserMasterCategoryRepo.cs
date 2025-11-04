using UserMasterCategory.API.Models;

namespace UserMasterCategory.API.Data.Repositories;

public interface IUserMasterCategoryRepo
{
    // Basic CRUD operations
    Task<UserMaster?> GetByIdAsync(int id);  // ✅ Updated to UserMaster
    Task<IEnumerable<UserMaster>> GetAllAsync();  // ✅ Updated to UserMaster
    Task<UserMaster> AddAsync(UserMaster userMaster);  // ✅ Updated to UserMaster
    Task<UserMaster> UpdateAsync(UserMaster userMaster);  // ✅ Updated to UserMaster
    Task<bool> DeleteAsync(int id);
    
    // Custom operations
    Task<UserMaster?> GetByMobileAsync(string phone_number);  // ✅ Updated to UserMaster
    Task<IEnumerable<UserMaster>> GetActiveUsersAsync();  // ✅ Updated to UserMaster
    Task<IEnumerable<UserMaster>> GetUsersByNameAsync(string name);  // ✅ Updated to UserMaster
}