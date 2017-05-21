using System;
using Unbroken.LaunchBox.Plugins.Data;
using System.Linq;
using System.Threading;
using Unbroken.LaunchBox.Plugins;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SteamTools
{
    /// <summary>
    /// Thin proxy around SteamTools.Core's SteamToolsContext
    /// This proxy protects against assembly reference errors if SteamTools.Core.dll is not loaded
    /// This protection allows themes to contain SteamTools objects even if the target install does not have SteamTools
    /// </summary>
    public static class SteamToolsBigBoxContext
    {
        /// <summary>
        /// Is SteamTools.Core.dll loaded?
        /// </summary>
        public static bool IsSteamToolsLoaded { get; private set; }

        internal static void Init()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            Assembly[] assems = currentDomain.GetAssemblies();
            IsSteamToolsLoaded = assems.Any(assembly =>
            {
                var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true).FirstOrDefault();
                return attribute != null && attribute.Value == "b0442f11-eb3f-4af0-8b09-ec67d7d8d701";
            });
        }

        private static bool IsSteamGame_External(IGame game) { return SteamToolsContext.IsSteamGame(game); }
        private static ulong? GetSteamGameID_External(IGame game) { return SteamToolsContext.GetSteamGameID(game); }
        private static bool? IsInstalled_External(ulong value, IGame game) { return SteamToolsContext.IsInstalled(value, game); }

        /// <summary>
        /// Is this application Steam game?
        /// </summary>
        /// <param name="game">LaunchBox IGame object</param>
        /// <returns>Nullable Boolean, null if no SteamTools.Core</returns>
        public static bool? IsSteamGame(IGame game)
        {
            if (!IsSteamToolsLoaded) return null;
            return IsSteamGame_External(game);
        }

        /// <summary>
        /// Get Steam GameID from Launchbox IGame object
        /// </summary>
        /// <param name="game">LaunchBox IGame object</param>
        /// <returns>Nullable UInt64, always null if no SteamTools.Core</returns>
        public static UInt64? GetSteamGameID(IGame game)
        {
            if (!IsSteamToolsLoaded) return null;
            return GetSteamGameID_External(game);
        }

        /// <summary>
        /// Is this Steam game Installed?
        /// </summary>
        /// <param name="GameID">Steam GameID</param>
        /// <returns>Nullable Boolean, always null if no SteamTools.Core</returns>
        public static bool? IsInstalled(UInt64 GameID, IGame game)
        {
            if (!IsSteamToolsLoaded) return null;
            return IsInstalled_External(GameID, game);
        }
    }
}