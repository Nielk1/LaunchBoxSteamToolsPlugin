using CarbyneSteamContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    class AdvancedSteamImportSystemMenuItem : ISystemMenuItemPlugin
    {
        public string Caption => "Advanced Steam Import";

        private System.Drawing.Image _IconImage;
        public Image IconImage => _IconImage;

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool AllowInBigBoxWhenLocked => false;

        public string CleanerSystemMenuItems_ParentMenuItem => "importToolStripMenuItem";

        public void OnSelected()
        {
            AdvancedSteamImportDialog Dlg = new AdvancedSteamImportDialog();
            if (Dlg.ShowDialog() == DialogResult.OK)
            {
                DateTime Now = DateTime.Now;
                List<SteamLaunchable> SteamGames = Dlg.CheckedItems;
                if (SteamGames.Count > 0)
                {
                    Dlg.CheckedItems.ForEach(steamGame =>
                    {
                        IGame game = PluginHelper.DataManager.AddNewGame(steamGame.Title);
                        game.ApplicationPath = $@"steam://rungameid/{steamGame.GetShortcutID()}";
                        game.Platform = Dlg.SelectedPlatform;
                        game.Status = "Imported from Steam with Steam Tools";
                        game.Source = "Steam Tools";
                        game.DateAdded = Now;
                        game.DateModified = Now;
                    });
                    PluginHelper.DataManager.Save();
                }
            }
        }

        public AdvancedSteamImportSystemMenuItem()
        {
            _IconImage = Resources.steam.ToBitmap();
        }
    }
}
