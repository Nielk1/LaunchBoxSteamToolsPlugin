using SteamVent;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    class SteamToolsGetBoxGameMenuItemPlugin : IGameMenuItemPlugin
    {
        SteamContext context;
        public bool SupportsMultipleGames => true;

        public string Caption => "Steam Tools / Get Boxes";

        public Image IconImage { get; private set; }

        public bool ShowInLaunchBox => true;

        public bool ShowInBigBox => false;

        public bool GetIsValidForGame(IGame selectedGame)
        {
            return GetIsValidForGames(new IGame[] { selectedGame });
        }

        public void OnSelected(IGame selectedGame)
        {
            OnSelected(new IGame[] { selectedGame });
        }

        public bool GetIsValidForGames(IGame[] selectedGames)
        {
            return selectedGames.Any(game => SteamToolsContext.IsSteamGame(game) && SteamToolsContext.GetSteamGameID(game) > 0);
        }

        public void OnSelected(IGame[] selectedGames)
        {

            UInt64[] AppIDs = selectedGames.Where(game => SteamToolsContext.IsSteamGame(game)).Select(game => SteamToolsContext.GetSteamGameID(game)).Where(dr => dr.HasValue && dr.Value > 0).Select(dr => dr.Value).ToArray();
            if (AppIDs.Length == 0)
                return;
            List<SteamLaunchableApp> apps = context.GetAppsWithMetadata(AppIDs).Where(dr => new[] { "game", "demo", "application" }.Contains(dr.appType) && AppIDs.Contains(dr.AppID)).ToList();
            WebClient webClient = new WebClient();

            foreach (IGame game in selectedGames)
            {
                if (SteamToolsContext.IsSteamGame(game))
                {
                    UInt64? GameIDNumber = SteamToolsContext.GetSteamGameID(game);
                    if (GameIDNumber.HasValue)
                    {
                        SteamLaunchableApp app = apps.Where(dr => dr.AppID == GameIDNumber.Value).FirstOrDefault();

                        if (app == null)
                            continue;

                        if (app.extra_has_library_hero)
                        {
                            if (game.GetAllImagesWithDetails("Box - Front").Length == 0)
                            {
                                string SavePath = game.GetNextAvailableImageFilePath(".jpg", "Box - Front", null);
                                try
                                {
                                    webClient.DownloadFile($@"https://steamcdn-a.akamaihd.net/steam/apps/{GameIDNumber.Value}/library_600x900_2x.jpg?t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}", SavePath);
                                }
                                catch (Exception ex)
                                {
                                    if (File.Exists(SavePath))
                                        File.Delete(SavePath);
                                }
                            }
                        }
                    }
                }
            }
        }

        public SteamToolsGetBoxGameMenuItemPlugin()
        {
            IconImage = Resources.steam.ToBitmap();
            context = SteamContext.GetInstance();
        }
    }
}
