using SteamVent;
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
                    string platform = Dlg.SelectedPlatform;
                    if (platform == @"-----NEW-----")
                    {
                        TextInputDialog textDlg = new TextInputDialog() { Title = "Platform" };
                        textDlg.ShowDialog();
                        platform = textDlg.Text;
                    }

                    Dlg.CheckedItems.ForEach(steamGame =>
                    {
                        IGame game = PluginHelper.DataManager.AddNewGame(steamGame.Title);
                        game.ApplicationPath = $@"steam://rungameid/{steamGame.GetShortcutID()}";
                        game.Platform = platform;
                        game.Status = "Imported from Steam with Steam Tools";
                        game.Source = "Steam Tools";
                        game.DateAdded = Now;
                        game.DateModified = Now;

                        ICustomField customField = game.AddNewCustomField();
                        customField.Name = "Steam AppType";
                        switch (steamGame.AppType)
                        {
                            case "game":
                                customField.Value = "Game";
                                break;
                            case "application":
                                customField.Value = "Application";
                                break;
                            case "demo":
                                customField.Value = "Demo";
                                break;
                            default:
                                customField.Value = steamGame.AppType;
                                break;
                        }
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
