using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Ardalis.SharedKernel;
using Request.Module.Domain;
using Request.Module.Domain.Base;

namespace Request.Module.Infrastructure.Persistence.Data
{
    public class RequestContext : DbContext
    {
        private readonly IDomainEventDispatcher? _dispatcher;

        public RequestContext(DbContextOptions<RequestContext> options,
          IDomainEventDispatcher? dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        //public RequestContext(DbContextOptions<RequestContext> options)
        //  : base(options)
        //{
        //}

        public DbSet<ADUser> Users { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<CumulativeLeaveRequest> CumulativeLeaveRequests { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Notification gruplama için işe yarar
            var now = DateTime.Now;

            ChangeTracker.Entries<EntityBase<Guid>>()
               .ToList().ForEach(entry =>
               {
                   if (entry.Entity is EntityModifyDate entityModifyDate)
                   {
                       if (entry.State == EntityState.Added)
                       {
                           entityModifyDate.ChangeModifyDate(null);
                           Entry(entityModifyDate).Property(x => x.LastModifiedAt).IsModified = true;

                           entityModifyDate.SetCreateDate(now);
                           Entry(entityModifyDate).Property(x => x.CreateDate).IsModified = true;
                       }
                       else if (entry.State == EntityState.Modified)
                       {
                           Entry(entityModifyDate).Property(x => x.CreateDate).IsModified = false;

                           entityModifyDate.ChangeModifyDate(now);
                           Entry(entityModifyDate).Property(x => x.LastModifiedAt).IsModified = true;

                       }
                   }
                   else if (entry.Entity is EntityCreateDate entityCreateDate)
                   {
                       if (entry.State == EntityState.Added)
                       {
                           entityCreateDate.SetCreateDate(now);
                           Entry(entityCreateDate).Property(x => x.CreateDate).IsModified = true;
                       }
                       else
                           Entry(entityCreateDate).Property(x => x.CreateDate).IsModified = false;
                   }
               });

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


            // ignore events if no dispatcher provided
            if (_dispatcher == null) return result;

            // dispatch events only if save was successful
            var entitiesWithEvents = ChangeTracker.Entries<EntityBase>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToArray();

            await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

            return result;
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}
