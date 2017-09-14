using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MusicSearch.Models;
using System.Threading.Tasks;

namespace MusicSearch.Hubs
{
    public class MyHub : Hub
    {
        public void HelloServer() => Clients.All.hello();
        public MyHub()
        {           
            var taskTimer = Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    string timeNow = DateTime.Now.ToString();
                    Clients.All.SendServerTime(timeNow);
                    await Task.Delay(3000);
                }
            }, TaskCreationOptions.LongRunning
                );
        }
    }
}