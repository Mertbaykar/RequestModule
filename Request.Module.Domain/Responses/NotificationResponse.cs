using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Responses
{
    public class NotificationResponse
    {
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }

        public DateTime CreateDate { get; set; }
        public string Message { get; set; }
        /// <summary>
        /// Kümülatif talep yılı
        /// </summary>
        public int Year { get; set; }
    }
}
