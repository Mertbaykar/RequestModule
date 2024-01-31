using Request.Module.Domain;
using Request.Module.Domain.Specifications.CumulativeLeaveRequest;
using AutoMapper;
using Request.Module.Application.Responses;

namespace Request.Module.Infrastructure.Persistence.Data.Repository
{
    public class CumulativeLeaveRequestRepository : BaseRepository<CumulativeLeaveRequest>, ICumulativeLeaveRequestRepository
    {
        public CumulativeLeaveRequestRepository(RequestContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<List<CumulativeLeaveRequestResponse>> GetCumulativeLeaveRequests()
        {
            var cumulativeLeaveRequestViewSpec = new CumulativeLeaveRequestViewSpec();
            var cumulativeLeaveRequests = await ListAsync(cumulativeLeaveRequestViewSpec);
            return Mapper.Map<List<CumulativeLeaveRequestResponse>>(cumulativeLeaveRequests);
        }

        public async Task HandleAfterLeaveRequestCreation(LeaveRequest createdLeaveRequest)
        {
            var spec = new CumulativeLeaveRequestByLeaveRequestSpec(createdLeaveRequest);
            var cumulativeLeaveRequest = await this.FirstOrDefaultAsync(spec);

            // create
            if (cumulativeLeaveRequest == null)
            {
                try
                {
                    var createdCumulativeLeaveRequest = CumulativeLeaveRequest.Create(createdLeaveRequest.LeaveType, createdLeaveRequest.CreatedById, createdLeaveRequest.StartDate, createdLeaveRequest.EndDate);
                    await AddAsync(createdCumulativeLeaveRequest);
                }
                catch (Exception ex)
                {
                    // dbcontext track edebiliyor
                    createdLeaveRequest.ChangeWorkFlow(Workflow.Exception);
                    await MasterDataDb.SaveChangesAsync();
                }
            }
            // update
            else
            {
                cumulativeLeaveRequest.UpdateTotalHours(createdLeaveRequest);
                await MasterDataDb.SaveChangesAsync();
            }
        }
    }

    public interface ICumulativeLeaveRequestRepository : IBaseRepository<CumulativeLeaveRequest>
    {
        Task HandleAfterLeaveRequestCreation(LeaveRequest createdLeaveRequest);
        Task<List<CumulativeLeaveRequestResponse>> GetCumulativeLeaveRequests();
    }
}
