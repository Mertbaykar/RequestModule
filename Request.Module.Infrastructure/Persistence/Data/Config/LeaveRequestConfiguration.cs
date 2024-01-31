using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Request.Module.Domain;

namespace Request.Module.Infrastructure.Persistence.Data.Config
{

    public class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder
                .HasKey(x => x.Id);


            builder
               .Property(x => x.FormNumber)
               .HasDefaultValueSql("NEXT VALUE FOR RequestFormNumberSequence");

            builder
                .Property(x => x.RequestFormNumber)
                 .HasComputedColumnSql("('LRF-000' + CONVERT(VARCHAR(50), [FormNumber]))", false);

            #region Enum

            builder.
                Property(x => x.LeaveType)
                .HasConversion<int>();

            builder.
                Property(x => x.Workflow)
                .HasConversion<int>();

            #endregion

            #region Foreign Key

            builder
                  .HasOne(x => x.AssignedUser)
                  .WithMany()
                  .HasForeignKey(x => x.AssignedUserId)
                  .IsRequired(false);

            builder
                .HasOne(x => x.CreatedBy)
                .WithMany(x => x.LeaveRequests)
                .HasForeignKey(x => x.CreatedById)
                .IsRequired(true);

            builder
                .HasOne(x => x.LastModifiedBy)
                .WithMany()
                .HasForeignKey(x => x.LastModifiedById)
                .IsRequired(false);

            #endregion

        }
    }
}
