using System.ComponentModel.DataAnnotations;

namespace BaF_ToDo.Models
{
    public class NotEmptyOrWhitespaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string str && (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)))
            {
                return new ValidationResult("The field cannot be empty or whitespace.");
            }

            return ValidationResult.Success;
        }
    }

    public record UpdateTaskEntityDTO
    {
        [NotEmptyOrWhitespaceAttribute]
        [Required, MinLength(1), MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public bool IsCompleted { get; set; } = false;
    }


}
