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
    class SteamToolsGetSteamBannerGameMenuItemPlugin : IGameMenuItemPlugin
    {
        SteamContext context;
        public bool SupportsMultipleGames => true;

        public string Caption => "Steam Tools / Get Steam Banners";

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
            WebClient webClient = new WebClient();

            foreach (IGame game in selectedGames)
            {
                if (SteamToolsContext.IsSteamGame(game))
                {
                    UInt64? GameIDNumber = SteamToolsContext.GetSteamGameID(game);
                    if (GameIDNumber.HasValue)
                    {
                        if (game.GetAllImagesWithDetails("Steam Banner").Length == 0)
                        {
                            string SavePath = game.GetNextAvailableImageFilePath(".png", "Steam Banner", null);
                            try
                            {
                                webClient.DownloadFile($@"https://steamcdn-a.akamaihd.net/steam/apps/{GameIDNumber.Value}/header.jpg?t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}", SavePath);
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

        public SteamToolsGetSteamBannerGameMenuItemPlugin()
        {
            IconImage = Resources.steam.ToBitmap();
            context = SteamContext.GetInstance();
        }
    }
}
