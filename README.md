# TaskTracker

A simple yet powerful console-based task management application built with C# and .NET 8.0. TaskTracker helps you organize your tasks with priorities, statuses, and detailed descriptions, all stored in a local SQLite database.

## Features

- ✅ **Create Tasks**: Add new tasks with title, description, and priority
- 📋 **View Tasks**: List all active tasks or view detailed information about specific tasks
- ✏️ **Update Tasks**: Modify task details including title, description, status, and priority
- 🗑️ **Delete Tasks**: Remove completed or unnecessary tasks
- 📊 **Task Status Tracking**: Track tasks through different stages (Pending, In Process, Finished)
- 🎯 **Priority Management**: Set task priorities (Low, Medium, High, Urgent)
- 🗄️ **Local Storage**: All data is stored locally using SQLite database

## Tech Stack

- **Framework**: .NET 8.0
- **Language**: C# 12.0
- **Database**: SQLite
- **Architecture**: Clean Architecture with Repository Pattern
- **ORM**: ADO.NET with Microsoft.Data.Sqlite

## Project Structure

```
TaskTracker/
├── Models/
│   └── ToDo.cs               # Main task model and enums
├── DTOs/
│   ├── ToDoCreate.cs         # DTO for creating tasks
│   ├── ToDoRead.cs           # DTO for reading task lists
│   └── ToDoUpdate.cs         # DTO for updating tasks
├── Services/
│   └── ToDoService.cs        # Data access layer
├── Interfaces/
│   └── IToDoService.cs       # Service interface
├── Helpers/
│   └── MenuNavigator.cs      # Console UI navigation
├── Program.cs                # Application entry point
├── TaskTracker.csproj        # Project configuration
└── TaskTracker.sln          # Solution file
```

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Any IDE that supports C# (Visual Studio, VS Code, JetBrains Rider)

## Installation

1. **Clone the repository** (if using version control):
   ```bash
   git clone https://github.com/ed1808/TaskTracker.git
   cd TaskTracker
   ```

2. **Navigate to the project directory**:
   ```bash
   cd TaskTracker
   ```

3. **Restore dependencies**:
   ```bash
   dotnet restore
   ```

4. **Build the project**:
   ```bash
   dotnet build
   ```

## Usage

### Running the Application

```bash
dotnet run
```

### Navigation

The application uses an intuitive console interface:

- **Arrow Keys**: Navigate up/down through menu options
- **Enter**: Select a menu option
- **Text Input**: Follow on-screen prompts for data entry

### Menu Options

1. **Create Task**
   - Enter task title (required)
   - Enter task description (optional)
   - Select priority level (1-4)
   - Task is automatically set to "Pending" status

2. **Get All Tasks**
   - Displays a formatted table of all active tasks (Pending and In Process)
   - Shows ID, Title, Status, and Priority

3. **Get Task Detail**
   - Enter task ID to view complete details
   - Shows all fields including creation date

4. **Update Task**
   - Enter task ID to update
   - Modify any field (leave blank to keep current value)
   - Update title, description, status, or priority

5. **Delete Task**
   - Enter task ID to delete
   - Permanently removes the task from database

6. **Exit**
   - Closes the application

## Task Properties

### Status Options
- **Pending**: Task is created but not started
- **In Process**: Task is currently being worked on
- **Finished**: Task is completed

### Priority Levels
- **Low**: Non-urgent tasks
- **Medium**: Standard priority tasks
- **High**: Important tasks requiring attention
- **Urgent**: Critical tasks needing immediate action

## Database Schema

The application automatically creates a SQLite database (`todos.db`) with the following structure:

```sql
CREATE TABLE ToDos (
    Id INTEGER PRIMARY KEY,
    Title TEXT NOT NULL,
    Description TEXT,
    Status INTEGER,
    Priority INTEGER,
    CreatedAt TEXT NOT NULL
);
```

## Configuration

The application uses the following default configuration:

- **Database File**: `todos.db` (created automatically in the project directory)
- **Connection String**: `Data Source=todos.db`

## Development

### Adding New Features

1. **Models**: Add new models in the `Models/` directory
2. **DTOs**: Create data transfer objects in `DTOs/` directory
3. **Services**: Implement business logic in `Services/` directory
4. **UI**: Extend the console interface in `Helpers/MenuNavigator.cs`

### Code Style

- Uses C# 12.0 features including primary constructors
- Follows clean architecture principles
- Implements the repository pattern for data access
- Uses nullable reference types for better null safety

## Error Handling

The application includes comprehensive error handling:

- **Database Errors**: SQLite exceptions are caught and user-friendly messages are displayed
- **Input Validation**: Invalid inputs are validated with appropriate error messages
- **Data Integrity**: Checks for existing records before operations

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is open source and available under the [MIT License](LICENSE).

## Roadmap

Future enhancements could include:

- [ ] Due date functionality
- [ ] Task categories/tags
- [ ] Search and filter capabilities
- [ ] Export functionality (CSV, JSON)
- [ ] Task reminders
- [ ] Recurring tasks
- [ ] Multi-user support
- [ ] Web interface
- [ ] Mobile app companion

## Support

If you encounter any issues or have questions:

1. Check the [Issues](../../issues) section
2. Create a new issue with detailed description
3. Provide steps to reproduce any bugs

## Author

Created with ❤️ by Edward

---

*TaskTracker - Simple. Effective. Task Management.*
