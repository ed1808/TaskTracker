using TaskTracker.DTOs;
using TaskTracker.Models;

namespace TaskTracker.Interfaces;

internal interface IToDoService
{
    bool Create(ToDoCreate toDoCreate);
    List<ToDoRead> List();
    ToDo? Get(int id);
    bool Update(int id, ToDoUpdate toDoUpdate);
    bool Delete(int id);
}