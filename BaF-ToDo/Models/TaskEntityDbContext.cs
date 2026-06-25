using Microsoft.EntityFrameworkCore;

namespace BaF_ToDo.Models
{
    public class TaskEntityDbContext : DbContext
    {
        public TaskEntityDbContext(DbContextOptions<TaskEntityDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
    }
}
