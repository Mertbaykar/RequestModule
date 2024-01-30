using Ardalis.SharedKernel;
using Ardalis.Specification.EntityFrameworkCore;
using System;

namespace Request.Module.Infrastructure.Infrastructure.Data
{
    // inherit from Ardalis.Specification type
    public class EfRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(RequestContext dbContext) : base(dbContext)
        {
        }
    }
}
