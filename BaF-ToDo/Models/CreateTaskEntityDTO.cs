using System.ComponentModel.DataAnnotations;

namespace BaF_ToDo.Models
{
    public class CreateTaskEntityDTO
    {
        [NotEmptyOrWhitespaceAttribute]
        [Required, MinLength(1), MaxLength(255)]
        public string Title { get; set; }
    }
}
