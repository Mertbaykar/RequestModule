using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.CumulativeLeaveRequest
{
    public class CumulativeLeaveRequestViewSpec : Specification<Domain.CumulativeLeaveRequest>
    {
        public CumulativeLeaveRequestViewSpec()
        {

            Query
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.TotalHours)
                .ThenBy(x => x.LeaveType)
                .Include(x => x.User)
                .AsNoTracking();
                
        }
    }
}
