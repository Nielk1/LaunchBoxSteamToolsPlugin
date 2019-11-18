using SteamVent;
using SteamVent.Common;
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
    class SteamToolsGetMetadataGameMenuItemPlugin : IGameMenuItemPlugin
    {
        SteamContext context;
        public bool SupportsMultipleGames => true;

        public string Caption => "Steam Tools / Get Metadata";

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
            bool dataDirty = false;

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
                        CGameID gameID = new CGameID(GameIDNumber.Value);

                        if (gameID.ModID == 0)
                        {
                            SteamLaunchableApp app = apps.Where(dr => dr.AppID == gameID.AppID).FirstOrDefault();

                            if (app == null)
                                continue;

                            if (app.extra_developer.Length > 0 && string.IsNullOrWhiteSpace(game.Developer))
                            {
                                game.Developer = string.Join(";", app.extra_developer);
                                dataDirty |= true; // PluginHelper.DataManager.Save(true);
                            }

                            if (app.extra_publisher.Length > 0 && string.IsNullOrWhiteSpace(game.Publisher))
                            {
                                game.Publisher = string.Join(";", app.extra_publisher);
                                dataDirty |= true; // PluginHelper.DataManager.Save(true);
                            }

                            ICustomField customField = game.GetAllCustomFields().Where(field => field.Name == "Steam Release Date").FirstOrDefault();
                            {
                                if (app.extra_original_release_date.HasValue)
                                {
                                    if (!game.ReleaseDate.HasValue)
                                    {
                                        game.ReleaseDate = app.extra_original_release_date;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }
                                    else if (customField != null && customField.Value == game.ReleaseDate.Value.ToString("yyyy-MM-dd"))
                                    {
                                        // if the release date is an exact copy of the steam release date,
                                        // assume we're actually using the steam release date as the normal
                                        // release date and null it for the code to pick it up
                                        game.ReleaseDate = null;
                                    }
                                }

                                if (app.extra_steam_release_date.HasValue)
                                {
                                    if (!game.ReleaseDate.HasValue)
                                    {
                                        game.ReleaseDate = app.extra_steam_release_date;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }

                                    string StreamReleaseDate = app.extra_steam_release_date.Value.ToString("yyyy-MM-dd");
                                    if (customField == null)
                                    {
                                        customField = game.AddNewCustomField();
                                        customField.Name = "Steam Release Date";
                                        customField.Value = StreamReleaseDate;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }
                                    else if (customField.Value != StreamReleaseDate)
                                    {
                                        customField.Value = StreamReleaseDate;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }

                                    ICustomField customField2 = game.GetAllCustomFields().Where(field => field.Name == "Steam Release Year").FirstOrDefault();
                                    string StreamReleaseYear = app.extra_steam_release_date.Value.ToString("yyyy");
                                    if (customField2 == null)
                                    {
                                        customField2 = game.AddNewCustomField();
                                        customField2.Name = "Steam Release Year";
                                        customField2.Value = StreamReleaseYear;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }
                                    else if (customField2.Value != StreamReleaseYear)
                                    {
                                        customField2.Value = StreamReleaseYear;
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }

                                    /*
                                    ICustomField customField3 = game.GetAllCustomFields().Where(field => field.Name == "Steam AppType").FirstOrDefault();
                                    if (customField3 == null)
                                    {
                                        customField3 = game.AddNewCustomField();
                                        customField3.Name = "Steam AppType";
                                        switch (app.AppType)
                                        {
                                            case "game":
                                                customField3.Value = "Game";
                                                break;
                                            case "application":
                                                customField3.Value = "Application";
                                                break;
                                            case "demo":
                                                customField3.Value = "Demo";
                                                break;
                                            default:
                                                customField3.Value = app.AppType;
                                                break;
                                        }
                                        dataDirty |= true; // PluginHelper.DataManager.Save(true);
                                    }
                                    */
                                }
                            }
                        }
                    }
                }
            }

            if (dataDirty)
                PluginHelper.DataManager.Save(true);
        }

        public SteamToolsGetMetadataGameMenuItemPlugin()
        {
            IconImage = Resources.steam.ToBitmap();
            context = SteamContext.GetInstance();
        }
    }
}
