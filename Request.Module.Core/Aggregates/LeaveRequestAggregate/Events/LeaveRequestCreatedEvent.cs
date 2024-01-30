using Ardalis.SharedKernel;
using Request.Module.Domain;

namespace Request.Module.Application.Aggregates.LeaveRequestAggregate.Events
{
    public class LeaveRequestCreatedEvent : DomainEventBase
    {
        public LeaveRequest LeaveRequest { get; set; }

        public LeaveRequestCreatedEvent(LeaveRequest leaveRequest)
        {
            LeaveRequest = leaveRequest;
        }
    }
}
