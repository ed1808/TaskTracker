using TaskTracker.Models;

namespace TaskTracker.DTOs;

internal class ToDoUpdate(string? title, string? description, Status? status, Priority? priority)
{
    public string? Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public Status? Status { get; set; } = status;
    public Priority? Priority { get; set; } = priority;
}