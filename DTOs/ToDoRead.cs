using TaskTracker.Models;

namespace TaskTracker.DTOs;

internal class ToDoRead(int id, string title, Status status, Priority priority)
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public Status Status { get; set; } = status;
    public Priority Priority { get; set; } = priority;
}