using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;

namespace SteamTools
{
    class AdvancedSteamImportSystemMenuItemPlugin : ISystemMenuItemPlugin
    {
        public string Caption => "Advanced Steam Import";

        private System.Drawing.Image _IconImage;
        public Image IconImage => _IconImage;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool AllowInBigBoxWhenLocked => false;


        public void OnSelected()
        {
            AdvancedSteamImportDialog Dlg = new AdvancedSteamImportDialog();
            Dlg.SetPlatforms(PluginHelper.DataManager.GetAllPlatforms());
            Dlg.ShowDialog();

        }

        public AdvancedSteamImportSystemMenuItemPlugin()
        {
            _IconImage = Resources.steam.ToBitmap();
        }
    }
}
