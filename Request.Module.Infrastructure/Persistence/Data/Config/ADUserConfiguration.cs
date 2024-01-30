using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Request.Module.Domain;

namespace Request.Module.Infrastructure.Persistence.Data.Config
{

    public class ADUserConfiguration : IEntityTypeConfiguration<ADUser>
    {
        public void Configure(EntityTypeBuilder<ADUser> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder.
                HasOne(x => x.Manager)
                .WithMany()
                .HasForeignKey(x => x.ManagerId)
                .IsRequired(false);

            builder.
                Property(x => x.UserType)
                .HasConversion<int>();


            var user1 = new ADUser("Münir", "Özkul", "munir.ozkul@negzel.net", UserType.Manager, null) { 
                Id = Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37")
            };

            var user2 = new ADUser("Şener", "Şen", "sener.sen@negzel.net", UserType.WhiteCollarEmployee, Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37"))
            {
                Id = Guid.Parse("59fb152a-2d59-435d-8fc1-cbc35c0f1d82")
            };

            var user3 = new ADUser("Kemal","Sunal","kemal.sunal@negzel.net", UserType.BlueCollarEmployee,Guid.Parse("59fb152a-2d59-435d-8fc1-cbc35c0f1d82"))
            {
                Id = Guid.Parse("23591451-1cf1-46a5-907a-ee3e52abe394"),
            };

            var seed = new List<ADUser> { user1, user2, user3 };
            builder.HasData(seed);
        }
    }
}
