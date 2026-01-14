using Fitness.Models.Common;

namespace Fitness.Models
{
    public class Profession : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Trainer> Trainers { get; set; } = [];
    }
}
