using DemoApplication.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DemoApplication.Database.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
               .ToTable("Users");

            builder
               .HasOne(u => u.Basket)
               
               .WithOne(b => b.User)
               .HasForeignKey<Basket>(u => u.UserId);
            builder
                .HasOne(u => u.Adress)
                .WithOne(a => a.User)
                .HasForeignKey<UserAdress>(u => u.UserId);
        }
    }
}
