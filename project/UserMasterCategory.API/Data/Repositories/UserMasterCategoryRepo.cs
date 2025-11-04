using Microsoft.EntityFrameworkCore;
using UserMasterCategory.API.Data;
using UserMasterCategory.API.Models;

namespace UserMasterCategory.API.Data.Repositories;

public class UserMasterCategoryRepo : IUserMasterCategoryRepo
{
    private readonly ApplicationDbContext _context;

    public UserMasterCategoryRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserMaster?> GetByIdAsync(int id)  // ✅ Updated to UserMaster
    {
        return await _context.UserMasters  // ✅ Updated to UserMasters
            .FirstOrDefaultAsync(u => u.id == id);
    }

    public async Task<IEnumerable<UserMaster>> GetAllAsync()  // ✅ Updated to UserMaster
    {
        return await _context.UserMasters  // ✅ Updated to UserMasters
            .OrderBy(u => u.id)
            .ToListAsync();
    }

    public async Task<UserMaster?> GetByMobileAsync(string phone_number)  // ✅ Updated to UserMaster
    {
        return await _context.UserMasters  // ✅ Updated to UserMasters
            .FirstOrDefaultAsync(u => u.phone_number == phone_number);
    }

    public async Task<UserMaster> AddAsync(UserMaster userMaster)  // ✅ Updated to UserMaster
    {
        _context.UserMasters.Add(userMaster);  // ✅ Updated to UserMasters
        await _context.SaveChangesAsync();
        return userMaster;
    }

    public async Task<UserMaster> UpdateAsync(UserMaster userMaster)  // ✅ Updated to UserMaster
    {
        _context.UserMasters.Update(userMaster);  // ✅ Updated to UserMasters
        await _context.SaveChangesAsync();
        return userMaster;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null) return false;

        _context.UserMasters.Remove(user);  // ✅ Updated to UserMasters
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<UserMaster>> GetActiveUsersAsync()  // ✅ Updated to UserMaster
    {
        return await _context.UserMasters  // ✅ Updated to UserMasters
            .Where(u => u.is_active)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserMaster>> GetUsersByNameAsync(string name)  // ✅ Updated to UserMaster
    {
        return await _context.UserMasters  // ✅ Updated to UserMasters
            .Where(u => u.name.Contains(name))
            .ToListAsync();
    }
}