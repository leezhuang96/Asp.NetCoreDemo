using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRDemo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRDemo
{
    //[Authorize]
    public class CountHub : Hub
    {
        private readonly ICountService countService;

        public CountHub(ICountService countService)
        {
            this.countService = countService;
        }

        public async Task GetLatestCount(string maxValue)
        {
            //var userName = Context.User.Identity.Name;

            int count;
            do
            {
                count = await countService.GetCount();
                Thread.Sleep(1000);

                //incoke all clients method.
                await Clients.All.SendAsync("ReciveUpdate", count);
            }
            while (count < int.Parse(maxValue));

            //incoke all clients method.
            await Clients.All.SendAsync("Finsihed");
        }

        public async override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var client = Clients.Client(connectionId);

            //incoke client<connectionId> method.
            await client.SendAsync("someFunc", new { random= "Init" });
            //incoke except client<connectionId> method.
            await Clients.AllExcept(connectionId).SendAsync("someFunc");

            await Groups.AddToGroupAsync(connectionId, "MyGroup");
            await Groups.RemoveFromGroupAsync(connectionId, "MyGroup");

            await Clients.Group("MyGroup").SendAsync("someFunc");
        }
    }
}
