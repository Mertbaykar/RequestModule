using Request.Module.Domain;
using Request.Module.Domain.Requests.Base;
using Request.Module.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Requests.Concrete
{
    public class CreateLeaveRequest : IRequestModel
    {
        public LeaveType LeaveType { get; set; } = LeaveType.AnnualLeave;
        public string Reason { get; set; } = string.Empty;
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(1);
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(4);
        public ADUserResponse CreatedBy { get; set; }
    }
}
