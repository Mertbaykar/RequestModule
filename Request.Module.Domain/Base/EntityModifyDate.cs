using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Base
{
    public class EntityModifyDate : EntityCreateDate
    {
        public DateTime? LastModifiedAt { get; private set; }

        public void ChangeModifyDate(DateTime? date)
        {
            LastModifiedAt = date;
        }
    }
}
