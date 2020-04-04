using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Controllers
{
    [Route("api/count")]
    public class CountController : Controller
    {
        private readonly IHubContext<CountHub> countHub;

        public CountController(IHubContext<CountHub> countHub)
        {
            this.countHub = countHub;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string random)
        {
            await countHub.Clients.All.SendAsync("someFunc", new { random = "Start" });

            return Accepted(10);
        }
    }
}
