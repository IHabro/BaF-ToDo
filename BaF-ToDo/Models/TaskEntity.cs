namespace BaF_ToDo.Models
{
    public class TaskEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        public void MarkCompleted()
        { 
            IsCompleted = true;
        }

        public TaskEntityDTO ToDTO()
        {
            return new TaskEntityDTO
            {
                Id = Id,
                Title = Title,
                IsCompleted = IsCompleted
            };
        }
    }
}
