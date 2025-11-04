using UserMasterCategory.API.DTOs;

namespace UserMasterCategory.API.Services;

public interface IUserMasterCategoryService
{
    Task<IEnumerable<UserMasterCategoryDTO>> GetAllUserMasterCategoryAsync();
    Task<UserMasterCategoryDTO> GetUserMasterCategoryByIdAsync(int id);
    Task<UserMasterCategoryDTO> CreateUserMasterCategoryAsync(CreateUserMasterCategoryDTO createUserMasterCategoryDTO);
    Task<UserMasterCategoryDTO> UpdateUserMasterCategoryAsync(int id, UpdateUserMasterCategoryDTO updateUserMasterCategoryDTO);
    Task<bool> DeleteUserMasterCategoryAsync(int id);
}