using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;

namespace PlayListCreator_FW.Classes
{
    public class UpdateCount : Hub
    {
        private static readonly Dictionary<string, string> CodeToConnectionId = new Dictionary<string, string>();

        // Method to send progress update to all connected clients
        public void SendProgress(int countAdded, int total)
        {
            Clients.Caller.updateCount(countAdded, total);
        }

      


    }
}