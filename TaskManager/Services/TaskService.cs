using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.DTOs;
using TaskManager.Enums;

namespace TaskManager.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(string? category, PriorityLevel? priority, TaskManager.Enums.TaskStatus? status)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(t => t.Category == category);

            if (priority.HasValue)
                query = query.Where(t => t.Priority == priority.Value);

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            return await query
                .Include(t => t.SubTasks)
                .OrderByDescending(t => t.Priority)
                .ThenBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.SubTasks)
                .Include(t => t.ParentTask)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TaskItem> CreateAsync(CreateTaskDto createDto)
        {
            var task = new TaskItem
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Priority = createDto.Priority,
                Category = createDto.Category,
                DueDate = createDto.DueDate,
                EstimatedEffort = createDto.EstimatedEffort,
                AssignedTo = createDto.AssignedTo,
                Tags = createDto.Tags,
                ParentTaskId = createDto.ParentTaskId,
                Status = TaskManager.Enums.TaskStatus.Pending
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItem?> UpdateAsync(int id, UpdateTaskDto updateDto)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.Status = updateDto.Status;
            task.Priority = updateDto.Priority;
            task.Category = updateDto.Category;
            task.DueDate = updateDto.DueDate;
            task.EstimatedEffort = updateDto.EstimatedEffort;
            task.AssignedTo = updateDto.AssignedTo;
            task.Tags = updateDto.Tags;
            task.ParentTaskId = updateDto.ParentTaskId;

            if (updateDto.Status == TaskManager.Enums.TaskStatus.Completed)
                task.CompletedAt = DateTime.UtcNow;
            else if (task.Status == TaskManager.Enums.TaskStatus.Completed && updateDto.Status != TaskManager.Enums.TaskStatus.Completed)
                task.CompletedAt = null;

            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, TaskManager.Enums.TaskStatus status)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return false;

            task.Status = status;
            
            if (status == TaskManager.Enums.TaskStatus.Completed)
                task.CompletedAt = DateTime.UtcNow;
            else
                task.CompletedAt = null;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetOverdueAsync()
        {
            return await _context.Tasks
                .Where(t => t.Status != TaskManager.Enums.TaskStatus.Completed && 
                           t.DueDate.HasValue && 
                           t.DueDate < DateTime.UtcNow)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<object> GetStatisticsAsync()
        {
            var total = await _context.Tasks.CountAsync();
            var completed = await _context.Tasks.CountAsync(t => t.Status == TaskManager.Enums.TaskStatus.Completed);
            var inProgress = await _context.Tasks.CountAsync(t => t.Status == TaskManager.Enums.TaskStatus.InProgress);
            var pending = await _context.Tasks.CountAsync(t => t.Status == TaskManager.Enums.TaskStatus.Pending);
            var overdue = await _context.Tasks.CountAsync(t => t.Status != TaskManager.Enums.TaskStatus.Completed && 
                                                              t.DueDate.HasValue && 
                                                              t.DueDate < DateTime.UtcNow);

            var priorityStats = await _context.Tasks
                .GroupBy(t => t.Priority)
                .Select(g => new { Priority = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            var categoryStats = await _context.Tasks
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToListAsync();

            var statusStats = await _context.Tasks
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            return new
            {
                Total = total,
                Completed = completed,
                InProgress = inProgress,
                Pending = pending,
                Overdue = overdue,
                CompletionRate = total > 0 ? Math.Round((double)completed / total * 100, 2) : 0,
                PriorityStats = priorityStats,
                CategoryStats = categoryStats,
                StatusStats = statusStats
            };
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Tasks
                .Select(t => t.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetByParentAsync(int parentTaskId)
        {
            return await _context.Tasks
                .Where(t => t.ParentTaskId == parentTaskId)
                .Include(t => t.SubTasks)
                .ToListAsync();
        }
    }
}