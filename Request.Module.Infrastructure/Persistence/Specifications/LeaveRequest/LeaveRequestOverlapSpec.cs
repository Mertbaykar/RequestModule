using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.LeaveRequest
{
    public class LeaveRequestOverlapSpec : Specification<Request.Module.Domain.LeaveRequest>
    {
        public LeaveRequestOverlapSpec(DateTime startDate, DateTime endDate, Guid userId)
        {

            Query
                .Where(x => x.CreatedById == userId
                && (!(x.EndDate < startDate || x.StartDate > endDate))
                )
                .AsNoTracking();
                
        }
    }
}
