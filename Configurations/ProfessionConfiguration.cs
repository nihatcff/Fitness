using Fitness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fitness.Configurations
{
    public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
    {
        public void Configure(EntityTypeBuilder<Profession> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        }
    }
}
