using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Implementation is based on the two articles
/// https://sandervandevelde.wordpress.com/2018/01/05/getting-signalr-running-on-asp-net-mvc-core/
/// https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio
/// </summary>
//https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-2.2
//https://docs.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-2.2&tabs=visual-studio
namespace ASPNETCoreWebApp.SignalRHub
{
    //The ChatHub class inherits from the SignalR Hub class. The Hub class manages connections, groups, and messaging.
    public class ChatHub : Hub
    {

        //The SendMessage method can be called by a connected client to send a message to all clients.
        //SignalR code is asynchronous to provide maximum scalability.
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
