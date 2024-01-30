using Ardalis.SharedKernel;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Request.Module.Infrastructure.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Request.Module.Domain;
using Request.Module.Domain.Specifications.ADUser;
using AutoMapper;
using Request.Module.Domain.Responses;
using Request.Module.Domain.Specifications.CumulativeLeaveRequest;

namespace Request.Module.Infrastructure.Persistence.Data.Repository
{
    public class UserRepository : BaseRepository<ADUser>, IUserRepository 
    {
        public UserRepository(RequestContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<ADUserResponse>> GetUsers()
        {
            var userwithManagerSpec = new UserWithManagerSpec();
            var users = await base.ListAsync(userwithManagerSpec);
            return Mapper.Map<List<ADUserResponse>>(users);
        }
    }

    public interface IUserRepository: IBaseRepository<ADUser>
    {
        Task<List<ADUserResponse>> GetUsers();
    }
}
