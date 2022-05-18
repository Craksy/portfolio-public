using System.ComponentModel.DataAnnotations;

namespace Frontend.Models{
    public class SubmitPostValidator 
    {
        [Required]
        [MaxLength(80)]
        public string? Title { get; set; }

        [Required]
        public string? Content { get; set; }
    }
}