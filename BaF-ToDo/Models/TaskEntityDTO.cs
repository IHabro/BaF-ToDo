namespace BaF_ToDo.Models
{
    public record TaskEntityDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
