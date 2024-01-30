using Request.Module.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Application.Responses
{
    public class LeaveRequestResponse
    {
        public string RequestFormNumber { get; set; }
        public LeaveType LeaveType { get; set; }
        public string Reason { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalHours { get; set; }

        public Workflow Workflow { get; set; }
        public DateTime CreateDate { get; set; }


        public Guid CreatedById { get; set; }
        public string CreatedByFullName { get; set; }

        public Guid? AssignedUserId { get; set; }
        public string AssignedUserFullName { get; set; }
    }
}
