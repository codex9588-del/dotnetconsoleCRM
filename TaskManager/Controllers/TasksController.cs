using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using TaskManager.DTOs;
using TaskManager.Enums;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks(
            [FromQuery] string? category = null,
            [FromQuery] PriorityLevel? priority = null,
            [FromQuery] TaskManager.Enums.TaskStatus? status = null)
        {
            var tasks = await _taskService.GetAllAsync(category, priority, status);
            var response = tasks.Select(MapToDto);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskResponseDto>> GetTask(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(MapToDto(task));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> CreateTask(CreateTaskDto createDto)
        {
            var task = await _taskService.CreateAsync(createDto);
            var response = MapToDto(task);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, UpdateTaskDto updateDto)
        {
            var task = await _taskService.UpdateAsync(id, updateDto);
            if (task == null) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}/status/{status}")]
        public async Task<IActionResult> UpdateStatus(int id, TaskManager.Enums.TaskStatus status)
        {
            var result = await _taskService.UpdateStatusAsync(id, status);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            var stats = await _taskService.GetStatisticsAsync();
            return Ok(stats);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _taskService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetOverdueTasks()
        {
            var tasks = await _taskService.GetOverdueAsync();
            var response = tasks.Select(MapToDto);
            return Ok(response);
        }

        [HttpGet("parent/{parentTaskId}")]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetSubTasks(int parentTaskId)
        {
            var tasks = await _taskService.GetByParentAsync(parentTaskId);
            var response = tasks.Select(MapToDto);
            return Ok(response);
        }

        private static TaskResponseDto MapToDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                Category = task.Category,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                DueDate = task.DueDate,
                CompletedAt = task.CompletedAt,
                EstimatedEffort = task.EstimatedEffort,
                AssignedTo = task.AssignedTo,
                Tags = task.Tags,
                ParentTaskId = task.ParentTaskId
            };
        }
    }
}