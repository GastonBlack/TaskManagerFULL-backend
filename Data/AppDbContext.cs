using Microsoft.EntityFrameworkCore;
using ToDoAppAPI.Models;

namespace ToDoAppAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Tabla "Tasks"
    public DbSet<ToDoTask> Tasks { get; set; }
}