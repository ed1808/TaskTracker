using TaskTracker.Models;

namespace TaskTracker.DTOs;

internal class ToDoCreate(string title, string? description, Priority priority)
{
    public string Title { get; set; } = title;
    public string? Description { get; set; } = description;
    public Status Status { get; set; } = Status.Pending;
    public Priority Priority { get; set; } = priority;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}