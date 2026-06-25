using BaF_ToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace BaF_ToDo.Repositories
{
    public interface ITaskEntityRepository
    {
        Task<List<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity?> GetTaskByIdAsync(int id);
        Task<TaskEntity> AddTaskAsync(TaskEntity task);
        Task<bool> UpdateTaskAsync(TaskEntity task);
        Task<bool> DeleteTaskAsync(int id);
    }

    // Typically instead of _context.SaveChangesAsync() we would use Unit of Work pattern
    // or other bussiness logic to handle transaction operations
    public class TaskEntityRepository : ITaskEntityRepository
    {
        private readonly TaskEntityDbContext _context;

        public TaskEntityRepository(TaskEntityDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskEntity>> GetAllTasksAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskEntity?> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> AddTaskAsync(TaskEntity task)
        {
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return task;
        }

        public async Task<bool> UpdateTaskAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
