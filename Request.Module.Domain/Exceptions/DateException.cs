using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Exceptions
{
    public class DateException : Exception
    {
        public DateException() : base()
        {

        }

        public DateException(string message) : base(message)
        {

        }

        //public DateException(Notification notification) : base(notification.Message)
        //{
        //    Notification = notification;
        //}

        //public DateException(IEnumerable<Notification> notifications) : base()
        //{

        //}

        //public Notification Notification { get; set; }
    }
}
