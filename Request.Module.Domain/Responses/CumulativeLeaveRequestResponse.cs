using Request.Module.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Application.Responses
{
    public class CumulativeLeaveRequestResponse
    {
        public LeaveType LeaveType { get;  set; }

        public Guid UserId { get;  set; }
        public string UserFullName { get;  set; }

        public int Year { get;  set; }
        public int TotalHours { get;  set; }
    }
}
