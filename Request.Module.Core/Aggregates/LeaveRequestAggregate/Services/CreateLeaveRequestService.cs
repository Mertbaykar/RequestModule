using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Request.Module.Application.Base;
using Request.Module.Domain;
using Request.Module.Domain.Exceptions;
using Request.Module.Domain.Requests.Base;
using Request.Module.Domain.Requests.Concrete;
using Request.Module.Infrastructure.Persistence.Data.Repository;

namespace Request.Module.Application.Aggregates.LeaveRequestAggregate.Services
{
    public class CreateLeaveRequestService : ICreateLeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILogger<CreateLeaveRequestService> _logger;

        public CreateLeaveRequestService(ILeaveRequestRepository leaveRequestRepository,
           ILogger<CreateLeaveRequestService> logger)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _logger = logger;
        }

        public async Task<Result<LeaveRequest>> Create(CreateLeaveRequest createLeaveRequest)
        {
            try
            {

                bool hasOverlap = await _leaveRequestRepository.HasOverlapOverDatesAsync(createLeaveRequest.StartDate, createLeaveRequest.EndDate, createLeaveRequest.CreatedBy.Id);

                if (hasOverlap)
                    throw new DateException($"{createLeaveRequest.StartDate.ToShortDateString()} - {createLeaveRequest.EndDate.ToShortDateString()} tarihleri arasında daha önce izin talebi yapmışsınız");

                var leaveRequest =  LeaveRequest.Create(createLeaveRequest.LeaveType, createLeaveRequest.Reason, createLeaveRequest.StartDate, createLeaveRequest.EndDate, createLeaveRequest.CreatedBy);

                await _leaveRequestRepository.AddAsync(leaveRequest);
                //// event publish savechanges'e taşındı
                //var domainEvent = new LeaveRequestCreatedEvent(leaveRequest);
                //await _mediator.Publish(domainEvent);
                return Result.Success(leaveRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw ex;
            }

        }
    }

    public interface ICreateLeaveRequestService : ICreator<LeaveRequest, CreateLeaveRequest>, ServiceBase
    {
    }
}
