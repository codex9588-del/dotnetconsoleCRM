using Microsoft.AspNetCore.Mvc;
using UserMasterCategory.API.DTOs;
using UserMasterCategory.API.Services;

namespace UserMasterCategory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserMasterCategoryController : ControllerBase
{
    private readonly IUserMasterCategoryService _service;

    public UserMasterCategoryController(IUserMasterCategoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserMasterCategoryDTO>>>> GetAll()
    {
        try
        {
            var users = await _service.GetAllUserMasterCategoryAsync();
            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.SuccessResponse(users, "Users retrieved successfully");
            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.ErrorResponse("Database error: " + ex.Message);
            return StatusCode(500, response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.ErrorResponse("Internal server error: " + ex.Message);
            return StatusCode(500, response);
        }
    }

    [HttpPost("multiple")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserMasterCategoryDTO>>>> CreateMultiple(IEnumerable<CreateUserMasterCategoryDTO> dtos)
    {
        try
        {
            var createdUsers = new List<UserMasterCategoryDTO>();

            foreach (var dto in dtos)
            {
                // Manual validation
                if (string.IsNullOrWhiteSpace(dto.name))
                {
                    var validationResponse = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Name is required");
                    return BadRequest(validationResponse);
                }

                if (string.IsNullOrWhiteSpace(dto.phone_number) || dto.phone_number.Length != 10)
                {
                    var validationResponse = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Valid 10-digit phone number is required");
                    return BadRequest(validationResponse);
                }

                var created = await _service.CreateUserMasterCategoryAsync(dto);
                createdUsers.Add(created);
            }

            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.SuccessResponse(createdUsers, "Users created successfully");
            return CreatedAtAction(nameof(GetAll), response);
        }
        catch (InvalidOperationException ex)
        {
            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.ErrorResponse(ex.Message);
            return Conflict(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<IEnumerable<UserMasterCategoryDTO>>.ErrorResponse("Failed to create users: " + ex.Message);
            return StatusCode(500, response);
        }
    }

       // MULTIPLE DELETE ENDPOINT - ADDED HERE
    [HttpDelete("multiple")]
    public async Task<ActionResult<ApiResponse<DeleteMultipleResponse>>> DeleteMultiple([FromBody] DeleteMultipleRequest request)
    {
        try
        {
            if (request?.Ids == null || !request.Ids.Any())
            {
                return BadRequest(ApiResponse<DeleteMultipleResponse>.ErrorResponse("No IDs provided for deletion"));
            }

            var deletedIds = new List<int>();
            var notFoundIds = new List<int>();
            var failedIds = new List<int>();

            foreach (var id in request.Ids)
            {
                try
                {
                    var deleted = await _service.DeleteUserMasterCategoryAsync(id);
                    if (deleted)
                    {
                        deletedIds.Add(id);
                    }
                    else
                    {
                        failedIds.Add(id);
                    }
                }
                catch (KeyNotFoundException)
                {
                    notFoundIds.Add(id);
                }
            }

            var responseData = new DeleteMultipleResponse
            {
                DeletedIds = deletedIds,
                NotFoundIds = notFoundIds,
                FailedIds = failedIds
            };

            string message;
            if (deletedIds.Count == request.Ids.Count())
            {
                message = "All users deleted successfully";
            }
            else if (deletedIds.Any())
            {
                message = $"Partial deletion completed. Deleted: {deletedIds.Count}, Not Found: {notFoundIds.Count}, Failed: {failedIds.Count}";
            }
            else
            {
                message = "No users were deleted";
            }

            var response = ApiResponse<DeleteMultipleResponse>.SuccessResponse(responseData, message);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<DeleteMultipleResponse>.ErrorResponse($"Failed to delete users: {ex.Message}");
            return StatusCode(500, response);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserMasterCategoryDTO>>> GetById(int id)
    {
        try
        {
            var user = await _service.GetUserMasterCategoryByIdAsync(id);
            var response = ApiResponse<UserMasterCategoryDTO>.SuccessResponse(user, "User retrieved successfully");
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse(ex.Message);
            return NotFound(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Failed to retrieve user: " + ex.Message);
            return StatusCode(500, response);
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<UserMasterCategoryDTO>>> Create(CreateUserMasterCategoryDTO dto)
    {
        try
        {
            // Manual validation
            if (string.IsNullOrWhiteSpace(dto.name))
            {
                var validationResponse = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Name is required");
                return BadRequest(validationResponse);
            }
                
            if (string.IsNullOrWhiteSpace(dto.phone_number) || dto.phone_number.Length != 10)
            {
                var validationResponse = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Valid 10-digit phone number is required");
                return BadRequest(validationResponse);
            }

            var created = await _service.CreateUserMasterCategoryAsync(dto);
            var response = ApiResponse<UserMasterCategoryDTO>.SuccessResponse(created, "User created successfully");
            return CreatedAtAction(nameof(GetById), new { id = created.id }, response);
        }
        catch (InvalidOperationException ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse(ex.Message);
            return Conflict(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Failed to create user: " + ex.Message);
            return StatusCode(500, response);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<UserMasterCategoryDTO>>> Update(int id, UpdateUserMasterCategoryDTO dto)
    {
        try
        {
            var updated = await _service.UpdateUserMasterCategoryAsync(id, dto);
            var response = ApiResponse<UserMasterCategoryDTO>.SuccessResponse(updated, "User updated successfully");
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse(ex.Message);
            return NotFound(response);
        }
        catch (InvalidOperationException ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse(ex.Message);
            return Conflict(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse<UserMasterCategoryDTO>.ErrorResponse("Failed to update user: " + ex.Message);
            return StatusCode(500, response);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        try
        {
            await _service.DeleteUserMasterCategoryAsync(id);
            var response = ApiResponse.SuccessResponse("User deleted successfully");
            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            var response = ApiResponse.ErrorResponse(ex.Message);
            return NotFound(response);
        }
        catch (Exception ex)
        {
            var response = ApiResponse.ErrorResponse("Failed to delete user: " + ex.Message);
            return StatusCode(500, response);
        }
    }
}

// ADD THESE DTO CLASSES TO YOUR DTOs FOLDER OR IN THE SAME FILE
public class DeleteMultipleRequest
{
    public IEnumerable<int> Ids { get; set; } = new List<int>();
}

public class DeleteMultipleResponse
{
    public List<int> DeletedIds { get; set; } = new List<int>();
    public List<int> NotFoundIds { get; set; } = new List<int>();
    public List<int> FailedIds { get; set; } = new List<int>();
}