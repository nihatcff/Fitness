using Fitness.Models;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Profession> Professions { get; set; }

    }
}
