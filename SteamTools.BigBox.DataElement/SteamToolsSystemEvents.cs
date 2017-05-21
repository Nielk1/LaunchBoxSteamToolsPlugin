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
                case SystemEventTypes.BigBoxStartupCompleted:
                case SystemEventTypes.LaunchBoxStartupCompleted:
                    SteamToolsBigBoxContext.Init();
                    break;
            }
        }
    }
}
