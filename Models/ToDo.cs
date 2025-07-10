namespace TaskTracker.Models;

internal class ToDo(string title, string? description, Status? status, Priority? priority, DateTime? createdAt)
{
    public int Id { get; set; }
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public Status? Status { get; set; } = status;
    public Priority? Priority { get; set; } = priority;
    public DateTime? CreatedAt { get; set; } = createdAt;
}

internal enum Status
{
    Pending,
    InProcess,
    Finished
}

internal enum Priority
{
    Low,
    Medium,
    High,
    Urgent
}