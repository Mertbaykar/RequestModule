using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Base
{
    public abstract class EntityBaseCustom<TId> : HasDomainEventsBaseCustom where TId : struct, IEquatable<TId>
    {
        public TId Id { get; set; }
    }
}
