using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.LeaveRequest
{
    public class LeaveRequestViewSpec : Specification<Request.Module.Domain.LeaveRequest>
    {
        public LeaveRequestViewSpec()
        {

            Query
                .OrderByDescending(x => x.CreateDate)
                .ThenBy(x => x.StartDate)
                .Include(x => x.CreatedBy)
                .Include(x => x.AssignedUser)
                .AsNoTracking();
                
        }
    }
}
