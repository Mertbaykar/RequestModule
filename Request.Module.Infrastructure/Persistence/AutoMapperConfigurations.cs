using AutoMapper;
using Request.Module.Application.Responses;
using Request.Module.Domain;
using Request.Module.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Infrastructure.Persistence
{
    public class AutoMapperConfigurations : Profile
    {
        public AutoMapperConfigurations()
        {

            CreateMap<LeaveRequest, LeaveRequestResponse>()
           .ForMember(dest => dest.CreatedByFullName, opt => opt.MapFrom(src => src.CreatedBy.FullName))
           .ForMember(dest => dest.AssignedUserFullName, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.FullName : string.Empty))
           .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => src.GetTotalHours(src.StartDate, src.EndDate)));


            CreateMap<CumulativeLeaveRequest, CumulativeLeaveRequestResponse>()
           .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
           .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName));


            CreateMap<Notification, NotificationResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.CumulativeLeaveRequest.Year));


            CreateMap<ADUser, ADUserResponse>()
           .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager != null ? src.Manager : null));

        }
    }
}
