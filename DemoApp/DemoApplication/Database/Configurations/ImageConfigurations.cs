using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database.Configurations
{
    public class ImageConfigurations : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder
               .ToTable("Images");
            builder
           .HasOne(b => b.Book)
           .WithMany(i => i.Images)
           .HasForeignKey(b => b.BookId);
        }
    }
}
