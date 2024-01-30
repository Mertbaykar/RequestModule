using Ardalis.Result;
using Ardalis.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Requests.Base
{
    public interface ICreator<TEntity, TRequest> where TEntity : EntityBase<Guid>, IAggregateRoot where TRequest : IRequestModel
    {
        Task<Result<TEntity>> Create(TRequest request); 
    }
}
