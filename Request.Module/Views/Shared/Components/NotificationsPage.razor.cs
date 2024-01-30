using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Request.Module.Domain;
using Request.Module.Application.Responses;
using Request.Module.Domain.Responses;


namespace Request.Module.Web.Views.Shared.Components
{
    public partial class NotificationsPage : ComponentBase
    {
        [Parameter]
        public List<NotificationResponse> NotificationList { get; set; } = new List<NotificationResponse>();


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                JS.InvokeVoidAsync("InitDataTable", "notifications");
            }
        }
    }
}
