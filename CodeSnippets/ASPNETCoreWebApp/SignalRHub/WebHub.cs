using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreWebApp.SignalRHub
{
    public class WebHub : Hub
    {
        public void Hello(string message)
        {
            Clients.All.SendAsync(message);
        }

    }
}
