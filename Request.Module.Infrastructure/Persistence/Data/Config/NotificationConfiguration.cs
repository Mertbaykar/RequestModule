using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Request.Module.Domain;

namespace Request.Module.Infrastructure.Persistence.Data.Config
{

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder
                .HasKey(x => x.Id);

            #region Foreign Key

            builder
                  .HasOne(x => x.User)
                  .WithMany(x => x.Notifications)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired(true);


            builder
              .HasOne(x => x.CumulativeLeaveRequest)
              .WithMany(x => x.Notifications)
              .HasForeignKey(x => x.CumulativeLeaveRequestId)
              .IsRequired(true);

            #endregion

        }
    }
}
