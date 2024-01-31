using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using MediatR;
using Request.Module.Domain.Base;
using Request.Module.Domain.Exceptions;
using System;
using System.Runtime.CompilerServices;

namespace Request.Module.Domain
{
    public class CumulativeLeaveRequest : EntityBaseCustom<Guid>, IAggregateRoot
    {
        private const int hoursperDay = 8;

        private const int AnnualLeaveMaxDay = 14;
        private const int ExcusedAbsenceMaxDay = 5;

        private const double AnnualLeaveToleranceRate = 0.1;
        private const double ExcusedAbsenceToleranceRate = 0.2;

        private const double MaxAnnualLeaveHoursWithTolerance = AnnualLeaveMaxDay * hoursperDay * (1 + AnnualLeaveToleranceRate);
        private const double MaxExcusedAbsenceHoursWithTolerance = ExcusedAbsenceMaxDay * hoursperDay * (1 + ExcusedAbsenceToleranceRate);

        // % 80 notification limiti
        private const double NotificationLimitRate = 0.8;

        private const double NotificationLimit4UserAnnualLeave = hoursperDay * AnnualLeaveMaxDay * NotificationLimitRate;
        private const double NotificationLimit4UserExcusedAbsence = hoursperDay * ExcusedAbsenceMaxDay * NotificationLimitRate;


        private CumulativeLeaveRequest()
        {

        }

        public static CumulativeLeaveRequest Create(LeaveType leaveType, Guid createdById, DateTime startDate, DateTime endDate)
        {
            
            var cumulativeLeaveRequest = new CumulativeLeaveRequest();
            cumulativeLeaveRequest.LeaveType = leaveType;
            cumulativeLeaveRequest.UserId = Guard.Against.Default(createdById, nameof(createdById));

            ValidateDates(startDate, endDate);
            int totalHours = 0;

            ValidateTotalHoursAfterCumulativeLeaveRequestCreation(startDate, endDate, ref totalHours, cumulativeLeaveRequest);
            cumulativeLeaveRequest.Year = startDate.Year;
            cumulativeLeaveRequest.TotalHours = totalHours;
            return cumulativeLeaveRequest;
        }

        private static void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate <= DateTime.Today)
                throw new DateException("Geçmiş tarihlerde izin alınamaz.");
            if (startDate > endDate)
                throw new DateException("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");
            if (startDate.Year != endDate.Year)
                throw new DateException("Başlangıç ile bitiş tarihi aynı yıl içinde olmalıdır.");

        }

        /// <summary>
        /// Create esnasında kullanılıyor, DateException'lar için notification gerekmez
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="totalHours"></param>
        /// <returns></returns>
        /// <exception cref="DateException"></exception>
        private static void ValidateTotalHoursAfterCumulativeLeaveRequestCreation(DateTime startDate, DateTime endDate, ref int totalHours, CumulativeLeaveRequest cumulativeLeaveRequest)
        {
            totalHours = GetTotalHours(startDate, endDate);

            if (cumulativeLeaveRequest.LeaveType == LeaveType.AnnualLeave)
            {
                if (totalHours >= MaxAnnualLeaveHoursWithTolerance)
                {
                    var diff = totalHours - MaxAnnualLeaveHoursWithTolerance;
                    string diffExplanation = diff == 0 ? "limite ulaşan talep yaptı" : $"limiti {diff / hoursperDay} gün aşan talep yaptı";

                    throw new DateException($"{cumulativeLeaveRequest.User.FullName} {cumulativeLeaveRequest.LeaveType} izin çeşidi için  {diffExplanation}");
                }
            }
            else
            {
                if (totalHours >= MaxExcusedAbsenceHoursWithTolerance)
                {
                    var diff = totalHours - MaxExcusedAbsenceHoursWithTolerance;
                    string diffExplanation = diff == 0 ? "limite ulaşan talep yaptı" : $"limiti {diff / hoursperDay} gün aşan talep yaptı";

                    // exception fırlat ve eklenen LeaveRequest'in workflow'unu exception yap.
                    throw new DateException($"{cumulativeLeaveRequest.User.FullName} {cumulativeLeaveRequest.LeaveType} izin çeşidi için  {diffExplanation}");
                }
            }

        }

        private static int GetTotalHours(DateTime startDate, DateTime endDate)
        {
            int totalHours = 0;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                    totalHours += hoursperDay;
            }

            return totalHours;
        }

        private void AddNotifications4UserAndManagerIfExists(string message)
        {
            var userNotification = new Notification(User, message, Id);
            AddNotification(userNotification);

            if (User.Manager != null)
            {
                var managerNotification = new Notification(User.Manager, message, Id);
                AddNotification(managerNotification);
            }
        }

        public void AddNotifications(IEnumerable<Notification> notifications)
        {
            foreach (var notification in notifications)
                AddNotification(notification);
        }
        public void AddNotification(Notification notification)
        {
            if (!Notifications.Contains(notification))
                this.Notifications.Add(notification);
        }

        public void UpdateTotalHours(LeaveRequest leaveRequest)
        {
            var addingTotalHours = GetTotalHours(leaveRequest.StartDate, leaveRequest.EndDate);
            var originalTotalHours = this.TotalHours;
            this.TotalHours += addingTotalHours;

            if (this.LeaveType == LeaveType.AnnualLeave)
            {
                if (TotalHours >= MaxAnnualLeaveHoursWithTolerance)
                {
                    // LeaveRequest'in workflow'unu exception yap.
                    // kullanıcı ile varsa manager'a notification gidecek

                    TotalHours = originalTotalHours;
                    string message = $"{User.FullName} {LeaveType} izin çeşidi için limiti {addingTotalHours / hoursperDay} gün aşan talep yaptı";
                    AddNotifications4UserAndManagerIfExists(message);
                    leaveRequest.ChangeWorkFlow(Workflow.Exception);
                }
                // % 80 aşımı
                else if (TotalHours >= NotificationLimit4UserAnnualLeave)
                {
                    string message = $"{User.FullName} {LeaveType} izin çeşidi için limit % {NotificationLimitRate * 100} oranına ulaştı.";
                    AddNotification(new Notification(User, message, Id));
                }

            }
            else
            {
                if (TotalHours >= MaxExcusedAbsenceHoursWithTolerance)
                {
                    // LeaveRequest'in workflow'unu exception yap.
                    // kullanıcı ile varsa manager'a notification gidecek
                    TotalHours = originalTotalHours;
                    string message = $"{User.FullName} {LeaveType} izin çeşidi için limiti {addingTotalHours / hoursperDay} gün aşan talep yaptı";
                    AddNotifications4UserAndManagerIfExists(message);
                    leaveRequest.ChangeWorkFlow(Workflow.Exception);
                }
                // % 80 aşımı
                else if (TotalHours >= NotificationLimit4UserExcusedAbsence)
                {
                    string message = $"{User.FullName} {LeaveType} izin çeşidi için limit % {NotificationLimitRate * 100} oranına ulaştı.";
                    AddNotification(new Notification(User, message, Id));
                }
            }
        }

        public LeaveType LeaveType { get; private set; }

        public Guid UserId { get; private set; }
        public ADUser User { get; private set; }

        public virtual ICollection<Notification> Notifications { get; private set; }

        public int Year { get; private set; }
        public int TotalHours { get; private set; }
    }
}
