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
using Request.Module.Domain.Specifications.LeaveRequest;
using AutoMapper;
using Request.Module.Application.Responses;

namespace Request.Module.Infrastructure.Persistence.Data.Repository
{
    public class LeaveRequestRepository : BaseRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(RequestContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<bool> HasOverlapOverDatesAsync(DateTime startDate, DateTime endDate, Guid createdById)
        {
            var overlapSpec = new LeaveRequestOverlapSpec(startDate, endDate, createdById);
            return await AnyAsync(overlapSpec);
        }

        public async Task<List<LeaveRequestResponse>> GetLeaveRequests()
        {
            var viewSpec = new LeaveRequestViewSpec();
            var leaveRequests = await ListAsync(viewSpec);
            return Mapper.Map<List<LeaveRequestResponse>>(leaveRequests);
        }
    }

    public interface ILeaveRequestRepository : IBaseRepository<LeaveRequest>
    {
        Task<bool> HasOverlapOverDatesAsync(DateTime startDate, DateTime endDate, Guid createdById);
        Task<List<LeaveRequestResponse>> GetLeaveRequests();
    }
}
