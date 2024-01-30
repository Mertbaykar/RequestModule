using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Request.Module.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain
{
    public class Notification : EntityCreateDate
    {
        private Notification()
        {
            
        }

        public Notification(ADUser user, string message, Guid cumulativeLeaveRequestId)
        {
            UserId = Guard.Against.Default(user.Id, nameof(user.Id));
            User = user;
            Message = message;
            CumulativeLeaveRequestId = cumulativeLeaveRequestId;
        }

        public Guid UserId { get; private set; }
        public ADUser User { get; private set; }

        public string Message { get; private set; }

        public CumulativeLeaveRequest CumulativeLeaveRequest { get; private set; }
        public Guid CumulativeLeaveRequestId { get; private set; }
    }
}
