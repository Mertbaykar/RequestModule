using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Specifications.ADUser
{
    public class UserWithManagerSpec : Specification<Request.Module.Domain.ADUser>
    {
        public UserWithManagerSpec()
        {
            Query
                .Include(x => x.Manager)
                .ThenInclude(x => x.Manager);
                
        }
    }
}
