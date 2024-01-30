using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.CumulativeLeaveRequest
{
    public class CumulativeLeaveRequestByLeaveRequestSpec : Specification<Request.Module.Domain.CumulativeLeaveRequest>
    {
        public CumulativeLeaveRequestByLeaveRequestSpec(Request.Module.Domain.LeaveRequest leaveRequest)
        {
            Query
                .Where(x => x.UserId == leaveRequest.CreatedById
                && x.LeaveType == leaveRequest.LeaveType
                && x.Year == leaveRequest.StartDate.Year)
                .Include(x => x.User)
                .Include(x => x.User.Manager)
                .Include(x => x.Notifications)
                //.AsNoTracking()
                ;

        }
    }
}
