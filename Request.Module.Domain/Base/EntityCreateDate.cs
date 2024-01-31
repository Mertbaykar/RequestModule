using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Base
{
    public class EntityCreateDate : EntityBaseCustom<Guid>, IAggregateRoot
    {
        public DateTime CreateDate { get; private set; }

        public void SetCreateDate(DateTime date)
        {
            CreateDate = date;
        }
    }
}
