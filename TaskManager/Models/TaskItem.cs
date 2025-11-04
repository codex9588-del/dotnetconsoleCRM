using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManager.Enums;

namespace TaskManager.Models
{
    [Table("Tasks", Schema = "taskmanager")]
    public class TaskItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; } = string.Empty;

        [Column(TypeName = "text")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(20)")]
        public TaskManager.Enums.TaskStatus Status { get; set; } = TaskManager.Enums.TaskStatus.Pending;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Category { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public DateTime? DueDate { get; set; }

         public TimeSpan? EstimatedEffort { get; set; } // in hours


        [Column(TypeName = "varchar(100)")]
        public string? AssignedTo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Tags { get; set; }

        public int? ParentTaskId { get; set; }

        public virtual TaskItem? ParentTask { get; set; }
        public virtual ICollection<TaskItem>? SubTasks { get; set; }

    }
}