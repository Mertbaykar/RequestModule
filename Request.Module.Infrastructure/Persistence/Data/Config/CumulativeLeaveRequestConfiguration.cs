using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Request.Module.Domain;


namespace Request.Module.Infrastructure.Persistence.Data.Config
{

    public class CumulativeLeaveRequestConfiguration : IEntityTypeConfiguration<CumulativeLeaveRequest>
    {
        public void Configure(EntityTypeBuilder<CumulativeLeaveRequest> builder)
        {
            builder
                .HasKey(x => x.Id);

            #region Enum

            builder.
                Property(x => x.LeaveType)
                .HasConversion<int>();

            #endregion

            #region Foreign Key

            builder
                  .HasOne(x => x.User)
                  .WithMany(x => x.CumulativeLeaveRequests)
                  .HasForeignKey(x => x.UserId)
                  .IsRequired(true);

            #endregion

        }
    }
}
