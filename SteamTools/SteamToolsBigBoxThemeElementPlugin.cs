using CarbyneSteamContextWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    public class SteamToolsBigBoxThemeElementPlugin : Panel,IBigBoxThemeElementPlugin
    {
        public static readonly DependencyProperty IsSteamGameProperty = DependencyProperty.Register("IsSteamGame", typeof(Boolean?), typeof(SteamToolsBigBoxThemeElementPlugin));
        public bool? IsSteamGame
        {
            get { return (bool)GetValue(IsSteamGameProperty); }
            set { SetValue(IsSteamGameProperty, value); }
        }

        public static readonly DependencyProperty IsInstalledProperty = DependencyProperty.Register("IsInstalled", typeof(Boolean?), typeof(SteamToolsBigBoxThemeElementPlugin));
        public bool? IsInstalled
        {
            get { return (bool?)GetValue(IsInstalledProperty); }
            set { SetValue(IsInstalledProperty, value); }
        }

        public static readonly DependencyProperty ManualProperty = DependencyProperty.Register("Manual", typeof(Boolean), typeof(SteamToolsBigBoxThemeElementPlugin));
        public bool Manual
        {
            get { return (bool)GetValue(ManualProperty); }
            set { SetValue(ManualProperty, value); }
        }

        public static readonly DependencyProperty GameProperty = DependencyProperty.Register("Game", typeof(IGame), typeof(SteamToolsBigBoxThemeElementPlugin));
        public IGame Game
        {
            get { return (IGame)GetValue(GameProperty); }
            set
            {
                SetValue(GameProperty, value);
                UpdateSelectedGame(Game);
            }
        }

        public bool OnDown(bool held) { return false; }
        public bool OnEnter() { return false; }
        public bool OnEscape() { return false; }
        public bool OnLeft(bool held) { return false; }
        public bool OnPageDown() { return false; }
        public bool OnPageUp() { return false; }
        public bool OnRight(bool held) { return false; }
        public bool OnUp(bool held) { return false; }

        public SteamToolsBigBoxThemeElementPlugin()
        {
            
        }

        private Thread UpdateDataThread;
        private object UpdateDataLock = new object();
        private CancellationTokenSource UpdateDataThreadToken;
        private class UpdateSelectionData
        {
            public CancellationTokenSource cts;
            public IGame game;
        }
        public void OnSelectionChanged(FilterType filterType, string filterValue, IPlatform platform, IPlatformCategory category, IPlaylist playlist, IGame game)
        {
            if (!Manual)
            {
                UpdateSelectedGame(game);
            }
        }

        private void UpdateSelectedGame(IGame game)
        {
            lock (UpdateDataLock)
            {
                if (UpdateDataThreadToken != null) UpdateDataThreadToken.Cancel();

                IsSteamGame = null;
                IsInstalled = null;

                UpdateDataThreadToken = new CancellationTokenSource();
                UpdateDataThread = new Thread(new ParameterizedThreadStart(UpdateSelected));
                UpdateDataThread.Start(new UpdateSelectionData { cts = UpdateDataThreadToken, game = game }); // copy the cancelation ref so we keep it safe
            }
        }

        private void UpdateSelected(object data)
        {
            UpdateSelectionData data2 = (UpdateSelectionData)data;
            if (data2.game.ApplicationPath?.StartsWith("steam://") ?? false)
            {
                if (data2.cts.IsCancellationRequested) return;
                this.Dispatcher.Invoke(() =>
                {
                    if (data2.cts.IsCancellationRequested) return;
                    IsSteamGame = true;
                });

                string GameID = data2.game.ApplicationPath.Split('/').Last();
                UInt64 GameIDNumber = 0;
                if (UInt64.TryParse(GameID, out GameIDNumber))
                {
                    if (data2.cts.IsCancellationRequested) return;
                    SteamContext context = SteamContext.GetInstance();
                    if (data2.cts.IsCancellationRequested) return;
                    if (context.IsInstalled(GameIDNumber))
                    {
                        if (data2.cts.IsCancellationRequested) return;
                        this.Dispatcher.Invoke(() =>
                        {
                            if (data2.cts.IsCancellationRequested) return;
                            IsInstalled = true;
                        });
                    }
                    else
                    {
                        if (data2.cts.IsCancellationRequested) return;
                        this.Dispatcher.Invoke(() =>
                        {
                            if (data2.cts.IsCancellationRequested) return;
                            IsInstalled = false;
                        });
                    }
                }
            }
            else {
                if (data2.cts.IsCancellationRequested) return;
                this.Dispatcher.Invoke(() =>
                {
                    if (data2.cts.IsCancellationRequested) return;
                    IsSteamGame = false;
                });
            }
        }
    }
}
