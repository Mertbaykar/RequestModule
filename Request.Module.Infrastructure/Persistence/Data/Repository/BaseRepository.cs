using Ardalis.SharedKernel;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Request.Module.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Infrastructure.Persistence.Data.Repository
{
    public abstract class BaseRepository<TEntity> : RepositoryBase<TEntity>, IBaseRepository<TEntity> where TEntity : EntityBase<Guid>, IAggregateRoot
    {
        protected readonly RequestContext MasterDataDb;
        protected readonly IMapper Mapper;

        public BaseRepository(RequestContext dbContext, IMapper mapper) : base(dbContext)
        {
            this.MasterDataDb = dbContext;
            this.Mapper = mapper;
        }

        public virtual Task<List<TEntity>> GetAll()
        {
            return base.ListAsync();
        }
    }

    public interface IBaseRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase<Guid>, IAggregateRoot
    {
        Task<List<TEntity>> GetAll();
    }
}
