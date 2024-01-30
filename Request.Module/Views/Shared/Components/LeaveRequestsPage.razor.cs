using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Request.Module.Domain;
using Request.Module.Application.Responses;


namespace Request.Module.Web.Views.Shared.Components
{
    public partial class LeaveRequestsPage : ComponentBase
    {
        [Parameter]
        public List<LeaveRequestResponse> LeaveRequestList { get; set; } = new List<LeaveRequestResponse>();


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                JS.InvokeVoidAsync("InitDataTable", "leaveRequests");
            }
        }

    }
}
