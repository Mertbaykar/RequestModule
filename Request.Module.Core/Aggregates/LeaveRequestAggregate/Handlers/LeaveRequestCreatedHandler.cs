using MediatR;
using Microsoft.Extensions.Logging;
using Request.Module.Application.Aggregates.LeaveRequestAggregate.Events;
using Request.Module.Infrastructure.Persistence.Data.Repository;

namespace Request.Module.Application.Aggregates.LeaveRequestAggregate.Handlers
{
    public class LeaveRequestCreatedHandler : INotificationHandler<LeaveRequestCreatedEvent>
    {
        private readonly ICumulativeLeaveRequestRepository _cumulativeLeaveRequestRepository;
        private readonly ILogger<LeaveRequestCreatedHandler> _logger;

        public LeaveRequestCreatedHandler(ICumulativeLeaveRequestRepository cumulativeLeaveRequestRepository, ILogger<LeaveRequestCreatedHandler> logger)
        {
            _cumulativeLeaveRequestRepository = cumulativeLeaveRequestRepository;
            _logger = logger;
        }

        public async Task Handle(LeaveRequestCreatedEvent leaveRequestCreatedEvent, CancellationToken cancellationToken)
        {
            try
            {
                await _cumulativeLeaveRequestRepository.HandleAfterLeaveRequestCreation(leaveRequestCreatedEvent.LeaveRequest);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }
}
