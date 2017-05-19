using System;
using CarbyneSteamContextWrapper;
using Unbroken.LaunchBox.Plugins.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins;

namespace SteamTools
{
    public static class SteamToolsContext
    {
        private static object contextLock = new object();
        private static SteamContext context;
        private static CancellationTokenSource SteamSentinalToken;
        private static Thread SteamSentinalThread;
        private static bool SteamActive = false;

        #region System Control
        public static void Init()
        {
            lock (contextLock)
            {
                if (context == null)
                {
                    context = SteamContext.GetInstance();

                    SteamSentinalToken = new CancellationTokenSource();
                    SteamSentinalThread = new Thread(new ThreadStart(CheckSteam));
                    SteamSentinalThread.Start();
                }
            }
        }

        public static void Shutdown()
        {
            lock (contextLock)
            {
                if (context != null)
                {
                    if (SteamActive)
                    {
                        SteamSentinalToken.Cancel();
                        context.Shutdown();
                        SteamActive = false;
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
                    if (!SteamActive && context.SteamIsRunning)
                    {
                        context.Init(ProxyServerPath: "Plugins");
                        SteamActive = true;
                    }
                    if (SteamActive && !context.SteamIsRunning)
                    {
                        context.Shutdown();
                        SteamActive = false;
                    }
                }
                if (SteamSentinalToken.IsCancellationRequested) break;
                Thread.Sleep(1000);
                if (SteamSentinalToken.IsCancellationRequested) break;
            }
        }
        #endregion System Control





        public static bool IsSteamGame(IGame game)
        {
            return game.ApplicationPath?.StartsWith("steam://") ?? false;
        }

        public static UInt64? GetSteamGameID(IGame game)
        {
            if (!IsSteamGame(game)) return null;
            string GameID = game.ApplicationPath.Split('/').Last();
            UInt64 GameIDNumber = 0;
            if (UInt64.TryParse(GameID, out GameIDNumber))
                return GameIDNumber;
            return null;
        }

        public static bool? IsInstalled(UInt64 GameID)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return null;
                return context.IsInstalled(GameID);
            }
        }

        public static string[] GetGameLibraries()
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return null;
                return context.GetGameLibraries();
            }
        }

        public static void InstallGame(ulong GameID)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return;
                context.InstallGame(GameID);
            }
        }

        public static Steam4NET.EAppUpdateError? InstallGame(ulong GameID, int index)
        {
            lock (contextLock)
            {
                if (!context.SteamIsRunning)
                    return null;
                return context.InstallGame(GameID, index);
            }
        }
    }
}