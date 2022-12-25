using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DemoApplication.Database.Configurations
{
    public class AdressConfigurations : IEntityTypeConfiguration<UserAdress>
    {
        public void Configure(EntityTypeBuilder<UserAdress> builder)
        {
            builder
              .ToTable("Addresses");
        }
    }
}
