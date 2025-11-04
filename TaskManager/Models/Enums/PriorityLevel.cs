using System.Text.Json.Serialization;

namespace TaskManager.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PriorityLevel
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Critical = 4
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TaskStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
}