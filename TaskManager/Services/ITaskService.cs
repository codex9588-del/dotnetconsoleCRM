using TaskManager.Models;
using TaskManager.DTOs;
using TaskManager.Enums;

namespace TaskManager.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(string? category, PriorityLevel? priority, TaskManager.Enums.TaskStatus? status);
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(CreateTaskDto createDto);
        Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, TaskManager.Enums.TaskStatus status);
        Task<IEnumerable<TaskItem>> GetOverdueAsync();
        Task<object> GetStatisticsAsync();
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<TaskItem>> GetByParentAsync(int parentTaskId);
    }
}