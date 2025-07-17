using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PlayListCreator_FW.App_Start.Startup))]

namespace PlayListCreator_FW.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Map SignalR hubs to "/signalr"
            app.MapSignalR();
        }
    }
}