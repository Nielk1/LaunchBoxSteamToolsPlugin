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
    public class SteamToolsDataBigBoxElement : Panel, IBigBoxThemeElementPlugin
    {
        public static readonly DependencyProperty IsSteamGameProperty = DependencyProperty.Register("IsSteamGame", typeof(Boolean?), typeof(SteamToolsDataBigBoxElement));
        public bool? IsSteamGame
        {
            get { return (bool)GetValue(IsSteamGameProperty); }
            set { SetValue(IsSteamGameProperty, value); }
        }

        public static readonly DependencyProperty IsInstalledProperty = DependencyProperty.Register("IsInstalled", typeof(Boolean?), typeof(SteamToolsDataBigBoxElement));
        public bool? IsInstalled
        {
            get { return (bool?)GetValue(IsInstalledProperty); }
            set { SetValue(IsInstalledProperty, value); }
        }

        public static readonly DependencyProperty SetOnceProperty = DependencyProperty.Register("SetOnce", typeof(Boolean), typeof(SteamToolsDataBigBoxElement));
        public bool SetOnce
        {
            get { return (bool)GetValue(SetOnceProperty); }
            set { SetValue(SetOnceProperty, value); }
        }
        private bool SetAtLeastOnce = false;

        public bool OnDown(bool held) { return false; }
        public bool OnEnter() { return false; }
        public bool OnEscape() { return false; }
        public bool OnLeft(bool held) { return false; }
        public bool OnPageDown() { return false; }
        public bool OnPageUp() { return false; }
        public bool OnRight(bool held) { return false; }
        public bool OnUp(bool held) { return false; }

        public SteamToolsDataBigBoxElement() { }

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
            if (!SetOnce || !SetAtLeastOnce)
            {
                UpdateSelectedGame(game);
                SetAtLeastOnce = true;
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
            if (SteamToolsContext.IsSteamGame(data2.game))
            {
                SetIsSteamGameSafely(true, data2.cts);

                UInt64? GameIDNumber = SteamToolsContext.GetSteamGameID(data2.game);
                if (GameIDNumber.HasValue)
                {
                    if (data2.cts.IsCancellationRequested) return;

                    bool? l_IsInstalled = SteamToolsContext.IsInstalled(GameIDNumber.Value);
                    if (l_IsInstalled.HasValue)
                    {
                        if (l_IsInstalled.Value)
                        {
                            SetIsInstalledSafely(true, data2.cts);
                        }
                        else
                        {
                            SetIsInstalledSafely(false, data2.cts);
                        }
                    }
                }
            }
            else
            {
                SetIsSteamGameSafely(false, data2.cts);
            }
        }

        /// <summary>
        /// Set the IsInstalled property safely from any thread
        /// </summary>
        /// <param name="Value">New Value</param>
        /// <param name="CancellationToken">Thread Cancellation</param>
        private void SetIsInstalledSafely(bool Value, CancellationTokenSource CancellationToken = null)
        {
            if (CancellationToken != null && CancellationToken.IsCancellationRequested) return;
            this.Dispatcher.Invoke(() =>
            {
                if (CancellationToken != null && CancellationToken.IsCancellationRequested) return;
                IsInstalled = Value;
            });
        }

        /// <summary>
        /// Set the IsSteamGame property safely from any thread
        /// </summary>
        /// <param name="Value">New Value</param>
        /// <param name="CancellationToken">Thread Cancellation</param>
        private void SetIsSteamGameSafely(bool Value, CancellationTokenSource CancellationToken = null)
        {
            if (CancellationToken != null && CancellationToken.IsCancellationRequested) return;
            this.Dispatcher.Invoke(() =>
            {
                if (CancellationToken != null && CancellationToken.IsCancellationRequested) return;
                IsSteamGame = Value;
            });
        }
    }
}
