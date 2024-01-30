using Microsoft.AspNetCore.Mvc;
using Request.Module.Application.Responses;
using Request.Module.Domain;
using Request.Module.Domain.Responses;
using Request.Module.Infrastructure.Persistence.Data.Repository;
using Request.Module.Web.Models;
using Request.Module.Web.Models.ViewModel;
using System.Diagnostics;

namespace Request.Module.Web.Controllers
{
    public class RequestController : Controller
    {
        private readonly ILogger<RequestController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ICumulativeLeaveRequestRepository _cumulativeLeaveRequestRepository;

        public RequestController(ILogger<RequestController> logger, IUserRepository userRepository, ILeaveRequestRepository leaveRequestRepository, INotificationRepository notificationRepository, ICumulativeLeaveRequestRepository cumulativeLeaveRequestRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _notificationRepository = notificationRepository;
            _cumulativeLeaveRequestRepository = cumulativeLeaveRequestRepository;
        }

        public async Task<IActionResult> Create()
        {
            var data = await _userRepository.GetUsers();
            var vm = new LayoutModel<List<ADUserResponse>>(data);
            return View(vm);
        }

        public async Task<IActionResult> LeaveRequests()
        {
            var data = await _leaveRequestRepository.GetLeaveRequests();
            var vm = new LayoutModel<List<LeaveRequestResponse>>(data);
            return View(vm);
        }

        public async Task<IActionResult> CumulativeLeaveRequests()
        {
            var data = await _cumulativeLeaveRequestRepository.GetCumulativeLeaveRequests();
            var vm = new LayoutModel<List<CumulativeLeaveRequestResponse>>(data);
            return View(vm);
        }

        public async Task<IActionResult> Notifications()
        {
            var data = await _notificationRepository.GetNotifications();
            var vm = new LayoutModel<List<NotificationResponse>>(data);
            return View(vm);
        }
    }
}
