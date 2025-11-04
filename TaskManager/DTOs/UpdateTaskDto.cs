using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TaskManager.Enums;

namespace TaskManager.DTOs
{
    public class UpdateTaskDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskManager.Enums.TaskStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityLevel Priority { get; set; }

        [StringLength(50)]
        public string Category { get; set; } = "General";

        public DateTime? DueDate { get; set; }

        public TimeSpan? EstimatedEffort { get; set; }

        [StringLength(100)]
        public string? AssignedTo { get; set; }

        [StringLength(200)]
        public string? Tags { get; set; }

        public int? ParentTaskId { get; set; }
    }
}