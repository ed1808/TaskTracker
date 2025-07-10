using Microsoft.Data.Sqlite;
using TaskTracker.DTOs;
using TaskTracker.Models;
using TaskTracker.Services;

namespace TaskTracker.Helpers;

internal static class MenuNavigator
{
    private static readonly string[] _menu = [
        "Create task",
        "Get all tasks",
        "Get task detail",
        "Update task",
        "Delete task",
        "Exit"
    ];

    private static readonly ToDoService _toDoService = new();

    public static MenuOptions ShowMainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("||========================== TaskTracker ==========================||\n");

            int index = 0;

            while (true)
            {
                Console.SetCursorPosition(0, 2);
                RenderMenu(index);

                ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                if (pressedKey.Key == ConsoleKey.UpArrow)
                {
                    index = (index - 1 + _menu.Length) % _menu.Length;
                }
                else if (pressedKey.Key == ConsoleKey.DownArrow)
                {
                    index = (index + 1) % _menu.Length;
                }
                else if (pressedKey.Key == ConsoleKey.Enter)
                {
                    return (MenuOptions)index;
                }
            }
        }
    }

    public static void GetAllTasks()
    {
        try
        {
            List<ToDoRead> toDos = _toDoService.List();

            if (toDos.Count == 0)
            {
                Console.WriteLine("There's no tasks yet");
                PauseExecution();
                return;
            }

            string header = string.Format("{0, -5} | {1, -30} | {2, -20} | {3, -20}", "Id", "Title", "Status", "Priority");
            string divider = new('-', header.Length);

            Console.WriteLine($"\n{divider}");
            Console.WriteLine(header);
            Console.WriteLine(divider);

            foreach (ToDoRead toDo in toDos)
            {
                Console.WriteLine("{0, -5} | {1, -30} | {2, -20} | {3, -20}", toDo.Id, toDo.Title, toDo.Status, toDo.Priority);
            }

            Console.WriteLine(divider);
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error getting all tasks: {ex.Message}");
        }

        PauseExecution();
    }

    public static void GetTask()
    {
        Console.Write("Enter the ID of the task: ");
        string? taskId = Console.ReadLine();

        if (string.IsNullOrEmpty(taskId?.Trim()))
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        bool isParsed = int.TryParse(taskId, out int id);

        if (!isParsed)
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        try
        {
            ToDo? toDo = _toDoService.Get(id);

            if (toDo == null)
            {
                Console.WriteLine("The specified task doesn't exists");
            }
            else
            {
                Console.WriteLine($"\nTitle: \t\t{toDo.Title}");
                Console.WriteLine($"Description: \t{toDo.Description}");
                Console.WriteLine($"Status: \t{toDo.Status}");
                Console.WriteLine($"Priority: \t{toDo.Priority}");
                Console.WriteLine($"Created at: \t{toDo.CreatedAt}");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error getting the task: {ex.Message}");
        }

        PauseExecution();
    }

    public static void CreateTask()
    {
        (string title, string? description, Priority priority) = GetTaskData();
        ToDoCreate newToDo = new(title, description, priority);

        try
        {
            bool result = _toDoService.Create(newToDo);

            if (!result)
            {
                Console.WriteLine("Something went wrong, try again");
            }
            else
            {
                Console.WriteLine("New task added successfully");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error creating task: {ex.Message}");
        }

        PauseExecution();
    }

    public static void DeleteTask()
    {
        Console.Write("Enter the ID of the task to be deleted: ");
        string? taskId = Console.ReadLine();

        if (string.IsNullOrEmpty(taskId?.Trim()))
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        bool isParsed = int.TryParse(taskId, out int id);

        if (!isParsed)
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        ToDo? validToDo = _toDoService.Get(id);

        if (validToDo == null)
        {
            Console.WriteLine("The task doesn't exists");
            PauseExecution();
            return;
        }

        try
        {
            bool result = _toDoService.Delete(id);

            if (!result)
            {
                Console.WriteLine("Something went wrong.");
            }
            else
            {
                Console.WriteLine("Task deleted successfully");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error trying to delete the task: {ex.Message}");
        }

        PauseExecution();
    }

    public static void UpdateTask()
    {
        Console.Write("Enter the ID of the task to be updated: ");
        string? taskId = Console.ReadLine();

        if (string.IsNullOrEmpty(taskId?.Trim()))
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        bool isParsed = int.TryParse(taskId, out int id);

        if (!isParsed)
        {
            Console.WriteLine("Invalid ID");
            PauseExecution();
            return;
        }

        ToDo? validToDo = _toDoService.Get(id);

        if (validToDo == null)
        {
            Console.WriteLine("The task doesn't exists");
            PauseExecution();
            return;
        }

        (string? title, string? description, Status? status, Priority? priority) = GetUpdateData();
        ToDoUpdate toDoUpdate = new(title, description, status, priority);

        try
        {
            bool result = _toDoService.Update(id, toDoUpdate);

            if (!result)
            {
                Console.WriteLine("Something went wrong.");
            }
            else
            {
                Console.WriteLine("Task updated successfully");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error trying to update the task: {ex.Message}");
        }

        PauseExecution();
    }

    private static void RenderMenu(int currentIndex)
    {
        for (int i = 0; i < _menu.Length; i++)
        {
            if (i == currentIndex)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[ > ] {_menu[i]}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"[   ] {_menu[i]}");
            }
        }
    }

    private static void PauseExecution()
    {
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey(true);
    }

    private static (string title, string? description, Priority priority) GetTaskData()
    {
        string title;
        string? description;
        bool isParsed;
        int taskPriority = 0;

        do
        {
            Console.WriteLine("\nAdd new task");

            Console.Write("Task title: ");
            title = Console.ReadLine() ?? "";

            Console.Write("Task description: ");
            description = Console.ReadLine();

            Console.WriteLine(string.IsNullOrEmpty(title));

            if (string.IsNullOrEmpty(title.Trim()))
            {
                Console.WriteLine("Task title required");
                PauseExecution();
                Console.Clear();
            }
        } while (string.IsNullOrEmpty(title));

        do
        {
            Console.WriteLine("Task priority (enter the number of the priority):");
            Console.WriteLine("1) Low");
            Console.WriteLine("2) Medium");
            Console.WriteLine("3) High");
            Console.WriteLine("4) Urgent");
            string? priorityOption = Console.ReadLine();

            isParsed = int.TryParse(priorityOption, out int chosenPriority);

            if (!isParsed)
            {
                Console.WriteLine("Invalid task priority");
                PauseExecution();
                Console.Clear();
            }
            else
            {
                taskPriority = chosenPriority - 1;
            }
        } while (!isParsed);

        return (title, description, (Priority)taskPriority);
    }

    private static (string? title, string? description, Status? status, Priority? priority) GetUpdateData()
    {
        string? newTitle;
        string? newDescription;
        Status? newStatus;
        Priority? newPriority;

        Console.Write("New title: ");
        newTitle = Console.ReadLine();

        Console.Write("New description: ");
        newDescription = Console.ReadLine();

        Console.WriteLine("New status (enter the number of the status):");
        Console.WriteLine("1) Pending");
        Console.WriteLine("2) In Process");
        Console.WriteLine("3) Finished");

        string? chosenStatus = Console.ReadLine();
        bool isStatusParsed = int.TryParse(chosenStatus, out int parsedStatus);

        newStatus = isStatusParsed ? (Status)(parsedStatus - 1) : null;

        Console.WriteLine("New priority (enter the number of the priority):");
        Console.WriteLine("1) Low");
        Console.WriteLine("2) Medium");
        Console.WriteLine("3) High");
        Console.WriteLine("4) Urgent");

        string? chosenPriority = Console.ReadLine();
        bool isPriorityParsed = int.TryParse(chosenPriority, out int parsedPriority);

        newPriority = isPriorityParsed ? (Priority)(parsedPriority - 1) : null;

        return (newTitle, newDescription, newStatus, newPriority);
    }
}

internal enum MenuOptions
{
    Add,
    GetAll,
    GetOne,
    Update,
    Delete,
    Exit
}