using Microsoft.Data.Sqlite;
using TaskTracker.DTOs;
using TaskTracker.Interfaces;
using TaskTracker.Models;

namespace TaskTracker.Services;

internal class ToDoService : IToDoService
{
    private readonly string _connectionString = "Data Source=todos.db";

    public ToDoService()
    {
        Initialize();
    }

    private SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    private void Initialize()
    {
        try
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();

            string createTableCmd =
            @"
                CREATE TABLE IF NOT EXISTS ToDos(
                    Id INTEGER PRIMARY KEY,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status INTEGER,
                    Priority INTEGER,
                    CreatedAt TEXT NOT NULL
                )
            ";

            using SqliteCommand cmd = new(createTableCmd, connection);
            cmd.ExecuteNonQuery();
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error during initialization: {ex.Message}");
        }
    }

    public bool Create(ToDoCreate toDoCreate)
    {
        using SqliteConnection connection = GetConnection();
        connection.Open();

        string insertCmd =
        @"
            INSERT INTO ToDos(
                Title,
                Description,
                Status,
                Priority,
                CreatedAt
            ) VALUES (
                :title,
                :description,
                :status,
                :priority,
                :createdAt
            )
        ";
        using SqliteCommand cmd = new(insertCmd, connection);

        cmd.Parameters.AddWithValue(":title", toDoCreate.Title);
        cmd.Parameters.AddWithValue(":description", toDoCreate.Description);
        cmd.Parameters.AddWithValue(":status", toDoCreate.Status);
        cmd.Parameters.AddWithValue(":priority", toDoCreate.Priority);
        cmd.Parameters.AddWithValue(":createdAt", toDoCreate.CreatedAt);

        int rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public bool Delete(int id)
    {
        using SqliteConnection connection = GetConnection();
        connection.Open();

        string deleteCmd = "DELETE FROM ToDos WHERE Id = :id";

        using SqliteCommand cmd = new(deleteCmd, connection);
        cmd.Parameters.AddWithValue(":id", id);
        
        int rowsAffected = cmd.ExecuteNonQuery();

        return rowsAffected > 0;
    }

    public ToDo? Get(int id)
    {
        using SqliteConnection connection = GetConnection();
        connection.Open();

        ToDo? toDo = null;

        string selectCmd =
        @"
            SELECT
                Title,
                Description,
                Status,
                Priority,
                CreatedAt
            FROM
                ToDos
            WHERE
                Id = :id
        ";

        using SqliteCommand cmd = new(selectCmd, connection);
        cmd.Parameters.AddWithValue(":id", id);

        using SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                toDo = new(
                    reader.GetString(0),
                    reader.GetString(1),
                    (Status)reader.GetInt32(2),
                    (Priority)reader.GetInt32(3),
                    reader.GetDateTime(4)
                )
                {
                    Id = id
                };
            }
        }

        return toDo;
    }

    public List<ToDoRead> List()
    {
        using SqliteConnection connection = GetConnection();
        connection.Open();

        List<ToDoRead> toDos = [];

        string selectCmd =
        @"
            SELECT
                Id,
                Title,
                Status,
                Priority
            FROM
                ToDos
            WHERE
                Status = 0
                OR Status = 1
        ";

        using SqliteCommand cmd = new(selectCmd, connection);
        using SqliteDataReader reader = cmd.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                toDos.Add(
                    new(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        (Status)reader.GetInt32(2),
                        (Priority)reader.GetInt32(3)
                    )
                );
            }
        }

        return toDos;
    }

    public bool Update(int id, ToDoUpdate toDoUpdate)
    {
        ToDo? toDo = Get(id);

        if (toDo != null)
        {
            using SqliteConnection connection = GetConnection();
            connection.Open();

            string updateCmd =
            @"
                UPDATE
                    ToDos
                SET
                    Title = :title,
                    Description = :description,
                    Status = :status,
                    Priority = :priority
                WHERE
                    Id = :id
            ";

            using SqliteCommand cmd = new(updateCmd, connection);

            string newTitle = toDoUpdate?.Title != null && !string.IsNullOrEmpty(toDoUpdate.Title.Trim()) ? toDoUpdate.Title : toDo.Title;
            string? newDescription = toDoUpdate?.Description != null && !string.IsNullOrEmpty(toDoUpdate.Description.Trim()) ? toDoUpdate.Description : toDo.Description;
            int newStatus = (int)(toDoUpdate?.Status != null ? toDoUpdate.Status : toDo.Status!);
            int newPriority = (int)(toDoUpdate?.Priority != null ? toDoUpdate.Priority : toDo.Priority!);

            cmd.Parameters.AddWithValue(":title", newTitle);
            cmd.Parameters.AddWithValue(":description", newDescription);
            cmd.Parameters.AddWithValue(":status", newStatus);
            cmd.Parameters.AddWithValue(":priority", newPriority);
            cmd.Parameters.AddWithValue(":id", id);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        return false;
    }
}