using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unbroken.LaunchBox.Plugins;

namespace SteamTools
{
    class SteamToolsOptionsSystemMenuItem : ISystemMenuItemPlugin
    {
        public string Caption => "Steam Tools Options";

        private System.Drawing.Image _IconImage;
        public Image IconImage => _IconImage;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool AllowInBigBoxWhenLocked => false;

        public string CleanerSystemMenuItems_ParentMenuItem => "optionsToolStripMenuItem";

        public void OnSelected()
        {
            MessageBox.Show("No Options yet in this version");
        }

        public SteamToolsOptionsSystemMenuItem()
        {
            _IconImage = Resources.steam.ToBitmap();
        }
    }
}
