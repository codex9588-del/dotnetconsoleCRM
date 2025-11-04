using UserMasterCategory.API.Data.Repositories;
using UserMasterCategory.API.DTOs;
using UserMasterCategory.API.Models;

namespace UserMasterCategory.API.Services;

public class UserMasterCategoryService : IUserMasterCategoryService
{
    private readonly IUserMasterCategoryRepo _userMasterCategoryRepo;

    public UserMasterCategoryService(IUserMasterCategoryRepo userMasterCategoryRepo)
    {
        _userMasterCategoryRepo = userMasterCategoryRepo;
    }

    public async Task<IEnumerable<UserMasterCategoryDTO>> GetAllUserMasterCategoryAsync()
    {
        try
        {
            var usersMasterCategory = await _userMasterCategoryRepo.GetAllAsync();
            return usersMasterCategory.Select(MapToDto);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error retrieving users from database.", ex);
        }
    }

    public async Task<UserMasterCategoryDTO> GetUserMasterCategoryByIdAsync(int id)
    {
        var userMasterCategory = await _userMasterCategoryRepo.GetByIdAsync(id);
        if (userMasterCategory == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        
        return MapToDto(userMasterCategory);
    }

    public async Task<UserMasterCategoryDTO> CreateUserMasterCategoryAsync(CreateUserMasterCategoryDTO createUserMasterCategoryDTO)
    {
        try
        {
            // Check if phone number already exists
            var existingUser = await _userMasterCategoryRepo.GetByMobileAsync(createUserMasterCategoryDTO.phone_number);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"User with phone number '{createUserMasterCategoryDTO.phone_number}' already exists.");
            }

            var userMasterCategory = new UserMaster
            {
                name = createUserMasterCategoryDTO.name,
                description = createUserMasterCategoryDTO.description,
                phone_number = createUserMasterCategoryDTO.phone_number,
                is_active = true,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow
            };

            var createdUserMasterCategory = await _userMasterCategoryRepo.AddAsync(userMasterCategory);
            return MapToDto(createdUserMasterCategory);
        }
        catch (InvalidOperationException)
        {
            throw; // Re-throw business exceptions
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error creating user in database.", ex);
        }
    }

    public async Task<UserMasterCategoryDTO> UpdateUserMasterCategoryAsync(int id, UpdateUserMasterCategoryDTO updateUserMasterCategoryDTO)
    {
        try
        {
            var existingUser = await _userMasterCategoryRepo.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            // Check if new phone number already exists (if changed)
            if (existingUser.phone_number != updateUserMasterCategoryDTO.phone_number)
            {
                var userWithSamePhone = await _userMasterCategoryRepo.GetByMobileAsync(updateUserMasterCategoryDTO.phone_number);
                if (userWithSamePhone != null && userWithSamePhone.id != id)
                {
                    throw new InvalidOperationException($"Another user with phone number '{updateUserMasterCategoryDTO.phone_number}' already exists.");
                }
            }

            // Update properties with null safety
            existingUser.name = updateUserMasterCategoryDTO.name ?? existingUser.name;
            existingUser.description = updateUserMasterCategoryDTO.description ?? existingUser.description;
            existingUser.phone_number = updateUserMasterCategoryDTO.phone_number ?? existingUser.phone_number;
            existingUser.is_active = updateUserMasterCategoryDTO.is_active ?? existingUser.is_active;
            existingUser.updated_at = DateTime.UtcNow;

            var updatedUser = await _userMasterCategoryRepo.UpdateAsync(existingUser);
            return MapToDto(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            throw; // Re-throw not found exceptions
        }
        catch (InvalidOperationException ex)
        {
            throw; // Re-throw business exceptions
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error updating user with ID {id}.", ex);
        }
    }

    public async Task<bool> DeleteUserMasterCategoryAsync(int id)
    {
        try
        {
            var existingUser = await _userMasterCategoryRepo.GetByIdAsync(id);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }

            return await _userMasterCategoryRepo.DeleteAsync(id);
        }
        catch (KeyNotFoundException)
        {
            throw; // Re-throw not found exceptions
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Error deleting user with ID {id}.", ex);
        }
    }

    private static UserMasterCategoryDTO MapToDto(UserMaster userMasterCategory)
    {
        return new UserMasterCategoryDTO
        {
            id = userMasterCategory.id,
            name = userMasterCategory.name,
            description = userMasterCategory.description,
            phone_number = userMasterCategory.phone_number,
            is_active = userMasterCategory.is_active,
            created_at = userMasterCategory.created_at,
            updated_at = userMasterCategory.updated_at
        };
    }
}