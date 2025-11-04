using TaskManager.Enums;

namespace TaskManager.DTOs
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskManager.Enums.TaskStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TimeSpan? EstimatedEffort { get; set; }
        public string? AssignedTo { get; set; }
        public string? Tags { get; set; }
        public int? ParentTaskId { get; set; }
        public bool IsOverdue => DueDate.HasValue && DueDate < DateTime.UtcNow && Status != TaskManager.Enums.TaskStatus.Completed;
        public List<string> TagList => Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
    }
}