using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamTools
{
    class SteamToolsOptions
    {
        public static int PollingSteamRate { get; set; }
        public static bool ShowInstalled { get; set; }
        public static bool HideUninstalled { get; set; }

        public static void LoadConfig()
        {
            // defaults
            PollingSteamRate = 60 * 60;
            ShowInstalled = false;
            HideUninstalled = false;

            if (File.Exists("steamtools.config"))
            {
                string[] lines = File.ReadAllLines("steamtools.config");
                if (lines.Length > 0) try { PollingSteamRate = int.Parse(lines[0]); } catch { }
                if (lines.Length > 1) try { ShowInstalled = bool.Parse(lines[1]); } catch { }
                if (lines.Length > 2) try { HideUninstalled = bool.Parse(lines[2]); } catch { }
            }
        }

        public static void SaveConfig()
        {
            File.WriteAllLines("steamtools.config", new string[]
            {
                PollingSteamRate.ToString(),
                ShowInstalled.ToString(),
                HideUninstalled.ToString()
            });
        }
    }
}
