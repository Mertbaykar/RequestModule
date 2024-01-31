using Ardalis.GuardClauses;
using Request.Module.Domain.Base;
using Request.Module.Domain.Events;
using Request.Module.Domain.Exceptions;
using Request.Module.Domain.Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain
{
    public class LeaveRequest : EntityModifyDate
    {
        private const int hoursperDay = 8;

        private LeaveRequest()
        {

        }

        public static LeaveRequest Create(LeaveType leaveType, string reason, DateTime startDate, DateTime endDate, ADUserResponse createdBy)
        {
            var leaveRequest = new LeaveRequest();
            leaveRequest.LeaveType = leaveType;
            leaveRequest.Reason = reason;
            leaveRequest.StartDate = Guard.Against.Default(startDate, nameof(startDate));
            leaveRequest.EndDate = Guard.Against.Default(endDate, nameof(endDate));

            ValidateDates(startDate, endDate);
            leaveRequest.CreatedById = Guard.Against.Default(createdBy.Id, nameof(createdBy.Id));

            HandleBusinessLogic(leaveRequest, createdBy);
            leaveRequest.RegisterDomainEvent(new LeaveRequestCreatedEvent(leaveRequest));
            return leaveRequest;
        }

        private static void ValidateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate <= DateTime.Today)
                throw new DateException("Geçmiş tarih için izin alınamaz.");
            if (startDate > endDate)
                throw new DateException("Başlangıç tarihi Bitiş tarihinden sonra olamaz.");
            if (startDate.Year != endDate.Year)
                throw new DateException("İzinler aynı yıl içindeki tarihlerde alınmalıdır.");
            if (GetTotalHours(startDate, endDate) == 0)
                throw new DateException("Resmi tatiller için izin talep edilemez.");
        }

        private static void HandleBusinessLogic(LeaveRequest leaveRequest, ADUserResponse createdBy)
        {
            #region Business Logic

            if (createdBy.UserType == UserType.WhiteCollarEmployee)
            {
                leaveRequest.Workflow = Workflow.Pending;
                leaveRequest.AssignedUserId = Guard.Against.Default(createdBy.Manager.Id, nameof(createdBy.Manager.Id));
            }
            else if (createdBy.UserType == UserType.BlueCollarEmployee)
            {
                if (leaveRequest.LeaveType == LeaveType.AnnualLeave)
                {
                    leaveRequest.Workflow = Workflow.Pending;
                    leaveRequest.AssignedUserId = Guard.Against.Default(createdBy.Manager.Manager.Id, nameof(createdBy.Manager.Manager.Id));
                }
                else if (leaveRequest.LeaveType == LeaveType.ExcusedAbsence)
                {
                    leaveRequest.Workflow = Workflow.Pending;
                    leaveRequest.AssignedUserId = Guard.Against.Default(createdBy.Manager.Id, nameof(createdBy.Manager.Id));
                }
            }
            else if (createdBy.UserType == UserType.Manager)
            {
                leaveRequest.Workflow = Workflow.None;
                leaveRequest.AssignedUserId = null;
            }

            #endregion
        }

        public void ChangeWorkFlow(Workflow workflow)
        {
            this.Workflow = workflow;
        }

        public int FormNumber { get; private set; }
        public string RequestFormNumber { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string Reason { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Workflow Workflow { get; private set; }

        /// <summary>
        /// CreatedBy'ın; Manager'ı, TopManager'ı veya null
        /// </summary>
        public Guid? AssignedUserId { get; private set; }
        public ADUser AssignedUser { get; private set; }

        public Guid CreatedById { get; private set; }
        public ADUser CreatedBy { get; private set; }

        public Guid? LastModifiedById { get; private set; }
        public ADUser LastModifiedBy { get; private set; }

        public static int GetTotalHours(DateTime startDate, DateTime endDate)
        {
            int totalHours = 0;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Sunday)
                    totalHours += hoursperDay;
            }

            return totalHours;
        }
    }

    public enum LeaveType
    {
        AnnualLeave = 10,
        ExcusedAbsence = 20,
    }

    public enum Workflow
    {
        None = 0,
        Pending = 10,
        Approved = 20,
        Rejected = 30,
        Exception = 100
    }
}
