using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain
{
    public class ADUser : EntityBase<Guid>, IAggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        [NotMapped]
        public string FullName => FirstName + " " + LastName;

        public string Email { get; private set; }
        public UserType UserType { get; private set; }

        public Guid? ManagerId { get; private set; }
        public ADUser Manager { get; private set; }

        public virtual ICollection<LeaveRequest> LeaveRequests { get; private set; }
        public virtual ICollection<CumulativeLeaveRequest> CumulativeLeaveRequests { get; private set; }
        public virtual ICollection<Notification> Notifications { get; private set; }

        public ADUser(string firstName, string lastName, string email, UserType userType, Guid? managerId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserType = userType;
            ManagerId = managerId;
        }

    }

    public enum UserType
    {
        WhiteCollarEmployee = 10,
        BlueCollarEmployee = 20,
        Manager = 30
    }
}
