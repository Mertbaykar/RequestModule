using Ardalis.SharedKernel;

namespace Request.Module.Domain.Events
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
