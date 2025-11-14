namespace ToDoAppAPI.Models;

public class ToDoTask
{
    public int Id { get; set; } // PostgreSQL genera ID's automaticamente.
    public string Content { get; set; } = string.Empty;

    private DateTime? _deadline;
    public DateTime? Deadline
    {
        get => _deadline;
        set => _deadline = value?.ToUniversalTime(); // Lo convierte a UTC antes de guardar.
    }

    public bool IsCompleted { get; set; }

    public ToDoTask() { }

    public ToDoTask(string content, DateTime? deadline = null, bool isCompleted = false)
    {
        Content = content;
        Deadline = deadline?.ToUniversalTime();
        IsCompleted = isCompleted;
    }
}