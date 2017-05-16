using CarbyneSteamContextWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    class SteamToolsSystemEventsPlugin : ISystemEventsPlugin
    {
        public void OnEventRaised(string eventType)
        {
            switch(eventType)
            {
                case SystemEventTypes.BigBoxStartupCompleted:
                case SystemEventTypes.LaunchBoxStartupCompleted:
                    {
                        SteamContext.GetInstance().Init(ProxyServerPath: "Plugins");
                    }
                    break;
                case SystemEventTypes.BigBoxShutdownBeginning:
                case SystemEventTypes.LaunchBoxShutdownBeginning:
                    {
                        SteamContext.GetInstance().Shutdown();
                    }
                    break;
            }
        }
    }
}
