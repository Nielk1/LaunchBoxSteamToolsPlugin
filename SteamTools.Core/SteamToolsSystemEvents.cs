using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    class SteamToolsSystemEvents : ISystemEventsPlugin
    {
        public void OnEventRaised(string eventType)
        {
            switch(eventType)
            {
                case SystemEventTypes.PluginInitialized:
                    SteamToolsContext.Remap();
                    break;
                case SystemEventTypes.BigBoxStartupCompleted:
                case SystemEventTypes.LaunchBoxStartupCompleted:
                    SteamToolsContext.Init();
                    break;
                case SystemEventTypes.BigBoxShutdownBeginning:
                case SystemEventTypes.LaunchBoxShutdownBeginning:
                    SteamToolsContext.Shutdown();
                    break;
            }
        }
    }
}
