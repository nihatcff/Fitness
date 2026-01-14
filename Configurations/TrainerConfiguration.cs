using Fitness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitness.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> builder)
        {
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(256);
            builder.Property(x=>x.Description).IsRequired().HasMaxLength(1024);
            builder.Property(x=>x.ImagePath).IsRequired();

            builder.HasOne(x=>x.Profession).WithMany(x=>x.Trainers).HasForeignKey(x=>x.ProfessionId).HasPrincipalKey(x=>x.Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
