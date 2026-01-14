using System.ComponentModel.DataAnnotations;

namespace Fitness.ViewModels.TrainerViewModels
{
    public class TrainerUpdateVM
    {
        public int Id { get; set; }
        [Required, MaxLength(256), MinLength(3)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(1024), MinLength(3)]
        public string Description { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
        [Required]
        public int ProfessionId { get; set; }
    }
}
