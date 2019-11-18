using System;
using Unbroken.LaunchBox.Plugins.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using SteamVent;

namespace SteamTools
{
    public static class SteamToolsContext
    {
        private static object contextLock = new object();
        private static SteamContext context;
        private static CancellationTokenSource SteamSentinalToken;
        private static Thread SteamSentinalThread;

        internal static void Remap()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveThirdPartyLibrary;
        }

        private static Assembly ResolveThirdPartyLibrary(object sender, ResolveEventArgs args)
        {
            // Check that CLR is loading the version of ThirdPartyLibrary referenced by MyLibrary
            /*if (args.Name.Equals("BigBox, Version=7.10.0.7, Culture=neutral, PublicKeyToken=97d6238f04304129"))
            {
                try
                {
                    // Load from application's base directory. Alternative logic might be needed if you need to 
                    // load from GAC etc. However, note that calling certain overloads of Assembly.Load will result
                    // in the AssemblyResolve event from firing recursively - see recommendations in
                    // http://msdn.microsoft.com/en-us/library/ff527268.aspx for further info
                    var assembly = Assembly.LoadFrom("BigBox.exe");
                    return assembly;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }*/
            if (Regex.IsMatch(args.Name, @"BigBox, Version=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+, Culture=neutral, PublicKeyToken=97d6238f04304129")) { return Assembly.LoadFrom("BigBox.exe"); }
            if (Regex.IsMatch(args.Name, @"LaunchBox, Version=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+, Culture=neutral, PublicKeyToken=97d6238f04304129")) { return Assembly.LoadFrom("LaunchBox.exe"); }
            if (Regex.IsMatch(args.Name, @"Unbroken.LaunchBox, Version=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+, Culture=neutral, PublicKeyToken=97d6238f04304129")) { return Assembly.LoadFrom("Unbroken.LaunchBox.dll"); }
            if (Regex.IsMatch(args.Name, @"Unbroken.LaunchBox.Wpf, Version=[0-9]+\.[0-9]+\.[0-9]+\.[0-9]+, Culture=neutral, PublicKeyToken=97d6238f04304129")) { return Assembly.LoadFrom("Unbroken.LaunchBox.Wpf.dll"); }
            return null;
        }

        /// <summary>
        /// Not for use by anything but the Steam context connector, use (context?.SteamIsRunning ?? false)
        /// </summary>
        private static bool SteamActiveCheck = false;
        private static CancellationTokenSource SteamUpdateSentinalToken;
        private static Thread SteamUpdateSentinalThread;

        #region System Control
        internal static void Init()
        {
            SteamToolsOptions.LoadConfig();
            lock (contextLock)
            {
                if (context == null)
                {
                    context = SteamContext.GetInstance();

                    SteamSentinalToken = new CancellationTokenSource();
                    SteamSentinalThread = new Thread(new ThreadStart(CheckSteam));
                    SteamSentinalThread.Start();

                    SteamUpdateSentinalToken = new CancellationTokenSource();
                    SteamUpdateSentinalThread = new Thread(new ThreadStart(CheckSteamGames));
                    SteamUpdateSentinalThread.Start();
                }
            }
        }

        internal static void Shutdown()
        {
            lock (contextLock)
            {
                SteamSentinalToken.Cancel();
                SteamUpdateSentinalToken.Cancel();

                while(SteamSentinalThread.IsAlive || SteamUpdateSentinalThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }

                if (context != null)
                {
                    if (SteamActiveCheck)
                    {
                        context.Shutdown();
                        SteamActiveCheck = false;
                    }
                }
            }
        }

        private static void CheckSteam()
        {
            for (;;)
            {
                if (SteamSentinalToken.IsCancellationRequested) break;
                lock(contextLock)
                {
                    if (!SteamActiveCheck && context.SteamIsRunning)
                    {
                        //context.Init(ProxyServerPath: "Plugins", SearchSubfolders: true);
                        SteamActiveCheck = true;
                    }
                    if (SteamActiveCheck && !context.SteamIsRunning)
                    {
                        context.Shutdown();
                        SteamActiveCheck = false;
                    }
                }
                if (SteamSentinalToken.IsCancellationRequested) break;
                Thread.Sleep(1000);
                if (SteamSentinalToken.IsCancellationRequested) break;
            }
        }

        private static void CheckSteamGames()
        {
            /*for (int i = 0; i < 60; i++)
            {
                if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                Thread.Sleep(1000);
            }
            if (SteamUpdateSentinalToken.IsCancellationRequested) return;
            for (;;)
            {
                if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                bool steamRunning = false;
                lock (contextLock)
                {
                    steamRunning = context?.SteamIsRunning ?? false;
                }
                if (steamRunning)
                {
                    if (PluginHelper.StateManager.IsPremium)// || SteamGameAutoHideShow)
                    {
                        IGame[] games = PluginHelper.DataManager.GetAllGames();
                        if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                        bool dataDirty = false;
                        foreach (IGame game in games)
                        {
                            if (SteamUpdateSentinalToken.IsCancellationRequested) break;
                            if (IsSteamGame(game))
                            {
                                if (SteamUpdateSentinalToken.IsCancellationRequested) break;
                                UInt64? GameID = GetSteamGameID(game);
                                if (SteamUpdateSentinalToken.IsCancellationRequested) break;
                                if (GameID.HasValue)
                                {
                                    bool? _isInstalled = null;
                                    //Thread.Sleep(1000);
                                    lock (contextLock)
                                    {
                                        _isInstalled = context?.IsInstalled(GameID.Value);
                                    }
                                    if (_isInstalled.HasValue)
                                    {
                                        if (PluginHelper.StateManager.IsPremium)
                                        {
                                            ICustomField installedField = game.GetAllCustomFields().Where(field => field.Name == "Steam Game Installed").FirstOrDefault();
                                            if (installedField == null)
                                            {
                                                installedField = game.AddNewCustomField();
                                                installedField.Name = "Steam Game Installed";
                                                installedField.Value = _isInstalled.ToString();
                                                dataDirty |= dataDirty; // PluginHelper.DataManager.Save(true);
                                            }
                                            else if (installedField.Value != _isInstalled.ToString())
                                            {
                                                installedField.Value = _isInstalled.ToString();
                                                dataDirty |= dataDirty; // PluginHelper.DataManager.Save(true);
                                            }
                                        }
                                        if (SteamToolsOptions.HideUninstalled)
                                        {
                                            if(!game.Hide && !_isInstalled.Value)
                                            {
                                                game.Hide = true;
                                                dataDirty |= true;
                                            }
                                        }
                                        if (SteamToolsOptions.ShowInstalled)
                                        {
                                            if (game.Hide && _isInstalled.Value)
                                            {
                                                game.Hide = false;
                                                dataDirty |= true;
                                            }
                                        }
                                    }
                                    if (SteamUpdateSentinalToken.IsCancellationRequested) break;
                                }
                            }
                        }
                        if(dataDirty)
                        {
                            PluginHelper.DataManager.Save(false);
                        }
                    }
                    if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                    for (int i = 0; i < SteamToolsOptions.PollingSteamRate; i++)
                    {
                        if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    for (int i = 0; i < 60; i++)
                    {
                        if (SteamUpdateSentinalToken.IsCancellationRequested) return;
                        Thread.Sleep(1000);
                    }
                }
            }*/
        }
        #endregion System Control

        /// <summary>
        /// Is this application a Steam game?
        /// </summary>
        /// <param name="game">LaunchBox IGame object</param>
        /// <returns>Boolean</returns>
        public static bool IsSteamGame(IGame game)
        {
            return game.ApplicationPath?.StartsWith("steam://") ?? false;
        }

        /// <summary>
        /// Get Steam GameID from Launchbox IGame object
        /// </summary>
        /// <param name="game">LaunchBox IGame object</param>
        /// <returns>Nullable UInt64 GameID</returns>
        public static UInt64? GetSteamGameID(IGame game)
        {
            if (!IsSteamGame(game)) return null;
            string GameID = game.ApplicationPath.Split('/').Last();
            UInt64 GameIDNumber = 0;
            if (UInt64.TryParse(GameID, out GameIDNumber))
                return GameIDNumber;
            return null;
        }

        /// <summary>
        /// Is this Steam game Installed?
        /// </summary>
        /// <param name="GameID">Steam GameID</param>
        /// <returns>Nullable Boolean</returns>
        public static bool? IsInstalled(UInt64 GameID, IGame game)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                {
                    if(game == null) return null;
                    if (!PluginHelper.StateManager.IsPremium) return null;

                    ICustomField installedFieldX = game.GetAllCustomFields().Where(field => field.Name == "Steam Game Installed").FirstOrDefault();
                    if (installedFieldX == null)
                    {
                        return null;
                    }
                    bool retVal;
                    if(bool.TryParse(installedFieldX.Value, out retVal))
                    {
                        return retVal;
                    }
                    return null;
                }

                bool _isInstalled = context.IsInstalled(GameID);
                if (game == null)
                    return _isInstalled;// context.IsInstalled(GameID);

                //if(SteamGameAutoHideShow)
                //{
                //    //show or hide game
                //}

                bool dataDirty = false;
                if (PluginHelper.StateManager.IsPremium)
                {
                    ICustomField installedField = game.GetAllCustomFields().Where(field => field.Name == "Steam Game Installed").FirstOrDefault();
                    if (installedField == null)
                    {
                        installedField = game.AddNewCustomField();
                        installedField.Name = "Steam Game Installed";
                        installedField.Value = _isInstalled.ToString();
                        dataDirty |= true; // PluginHelper.DataManager.Save(false);
                    }
                    else if (installedField.Value != _isInstalled.ToString())
                    {
                        installedField.Value = _isInstalled.ToString();
                        dataDirty |= true; // PluginHelper.DataManager.Save(false);
                    }
                }
                if (SteamToolsOptions.HideUninstalled)
                {
                    if (!game.Hide && !_isInstalled)
                    {
                        game.Hide = true;
                        dataDirty |= true;
                    }
                }
                if (SteamToolsOptions.ShowInstalled)
                {
                    if (game.Hide && _isInstalled)
                    {
                        game.Hide = false;
                        dataDirty |= true;
                    }
                }

                if (dataDirty) PluginHelper.DataManager.Save(false);

                return _isInstalled;
            }
        }

        /// <summary>
        /// Get array of Steam libraries
        /// </summary>
        /// <returns></returns>
        /*public static string[] GetGameLibraries()
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return null;
                return context.GetGameLibraries();
            }
        }*/

        /// <summary>
        /// Install a game in Steam
        /// </summary>
        /// <param name="GameID">Steam GameID</param>
        /*public static void InstallGame(ulong GameID)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return;
                context.InstallGame(GameID);
            }
        }*/

        /// <summary>
        /// Install a game in Steam
        /// </summary>
        /// <param name="GameID">Steam GameID</param>
        /// <param name="index">Steam library index</param>
        /// <returns></returns>
        /*public static Steam4NET.EAppUpdateError? InstallGame(ulong GameID, int index)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return null;
                return context.InstallGame(GameID, index);
            }
        }*/
    }
}