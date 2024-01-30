using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Request.Module.Domain;
using Request.Module.Domain.Requests.Concrete;
using Request.Module.Domain.Responses;


namespace Request.Module.Web.Views.Shared.Components
{
    public partial class CreateRequestPage : ComponentBase
    {
        [Parameter]
        public List<ADUserResponse> Users { get; set; } = new List<ADUserResponse>();
        public ADUserResponse SelectedUser { get; set; }

        private Guid selectedUserId;
        public Guid SelectedUserId
        {
            get { return selectedUserId; }
            set
            {
                selectedUserId = value;
                SelectedUser = Users.FirstOrDefault(x => x.Id == value);
                CreateLeaveRequest.CreatedBy = SelectedUser;
            }
        }

        private List<string> ErrorMessages { get; set; } = new List<string>();

        public CreateLeaveRequest CreateLeaveRequest { get; set; } = new CreateLeaveRequest();

        protected override void OnInitialized()
        {
            SelectedUserId = Users.FirstOrDefault().Id;
        }

        public async Task MakeLeaveRequest()
        {
            try
            {
                ErrorMessages.Clear();

                if (CreateLeaveRequest.StartDate <= DateTime.Today)
                    ErrorMessages.Add("Geçmiş tarih için izin alınamaz.");
                if (CreateLeaveRequest.StartDate > CreateLeaveRequest.EndDate)
                    ErrorMessages.Add("Başlangıç tarihi Bitiş tarihinden sonra olamaz.");
                if (CreateLeaveRequest.StartDate.Year != CreateLeaveRequest.EndDate.Year)
                    ErrorMessages.Add("İzinler aynı yıl içindeki tarihlerde alınmalıdır.");

                if (!ErrorMessages.Any())
                {
                    await CreateLeaveRequestService.Create(CreateLeaveRequest);
                    await OpenSuccessModal();
                }
            }
            catch (Exception ex)
            {
                ErrorMessages.Add($"{ex.Message}");
            }
        }

        public async Task OpenSuccessModal()
        {
            await JS.InvokeVoidAsync("openModal", "successModal");
        }

        public async Task CloseSuccessModal()
        {
            await JS.InvokeVoidAsync("closeModal", "successModal");
        }
    }
}
