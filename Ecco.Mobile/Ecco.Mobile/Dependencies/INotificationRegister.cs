using Ecco.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Dependencies
{
    public interface INotificationRegister
    {
        void RegisterForRemoteNotifications(string username, IDatabaseManager db);
    }
}