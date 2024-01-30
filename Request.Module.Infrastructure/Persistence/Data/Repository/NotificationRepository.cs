using Request.Module.Domain;
using Request.Module.Domain.Specifications.CumulativeLeaveRequest;
using AutoMapper;
using Request.Module.Domain.Responses;

namespace Request.Module.Infrastructure.Persistence.Data.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(RequestContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<NotificationResponse>> GetNotifications()
        {
            var notificationsSpec = new NotificationViewSpec();
            var notifications = await ListAsync(notificationsSpec);
            return Mapper.Map<List<NotificationResponse>>(notifications);
        }
    }

    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<List<NotificationResponse>> GetNotifications();

    }
}
