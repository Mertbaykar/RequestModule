using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.CumulativeLeaveRequest
{
    public class NotificationViewSpec : Specification<Domain.Notification>
    {
        public NotificationViewSpec()
        {

            Query
                .Include(x => x.User)
                .Include(x => x.CumulativeLeaveRequest)
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.CumulativeLeaveRequest.Year)
                .AsNoTracking();

        }
    }
}
