using Ardalis.Result;
using Ardalis.SharedKernel;
using Request.Module.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Requests.Base
{
    public interface ICreator<TEntity, TRequest> where TEntity : EntityBaseCustom<Guid>, IAggregateRoot where TRequest : IRequestModel
    {
        Task<Result<TEntity>> Create(TRequest request); 
    }
}
