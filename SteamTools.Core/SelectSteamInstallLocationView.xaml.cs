using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Markup;
using Unbroken.LaunchBox.Wpf.BigBox;
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    /// <summary>
    /// Interaction logic for SelectSteamInstallLocationView.xaml
    /// </summary>
    public partial class SelectSteamInstallLocationView : UserControl, IBigBoxThemeElementPlugin
    {
        public bool Accepted { get; set; }

        public SelectSteamInstallLocationView()
        {
            this.InitializeComponent();
            double screenHeight = App.ScreenHeight;
            this.Items.FontSize = screenHeight / 28.5;
            this.Accepted = false;
        }

        public bool OnUp(bool held) {
            if (this.IsVisible)
            {
                int currentIndex = Items.SelectedIndex;
                currentIndex--;
                if (currentIndex < 0)
                {
                    if (held)
                    {
                        currentIndex = 0;
                    }
                    else
                    {
                        currentIndex = Items.Items.Count - 1;
                    }
                }
                Items.SelectedIndex = currentIndex;
                Items.ScrollIntoView(Items.SelectedItem);
                return true;
            }
            return false;
        }
        public bool OnDown(bool held) {
            if (this.IsVisible)
            {
                int currentIndex = Items.SelectedIndex;
                currentIndex++;
                if (currentIndex >= Items.Items.Count)
                {
                    if (held)
                    {
                        currentIndex = Items.Items.Count - 1;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                }
                Items.SelectedIndex = currentIndex;
                Items.ScrollIntoView(Items.SelectedItem);
                return true;
            }
            return false;
        }
        public bool OnLeft(bool held) { return this.IsVisible; }
        public bool OnRight(bool held) { return this.IsVisible; }

        public bool OnPageUp() { return this.IsVisible; }
        public bool OnPageDown() { return this.IsVisible; }

        private void OnEnter(object sender, MouseButtonEventArgs e)
        {
            OnEnter();
        }
        public bool OnEnter() {
            if (this.IsVisible)
            {
                Accepted = true;
                this.Visibility = Visibility.Hidden;
                return true;
            }
            return false;
        }
        public bool OnEscape()
        {
            if(this.IsVisible)
            {
                this.Visibility = Visibility.Hidden;
                return true;
            }
            return false;
        }
        
        public void OnSelectionChanged(FilterType filterType, string filterValue, IPlatform platform, IPlatformCategory category, IPlaylist playlist, IGame game)
        {
            
        }

        public void SetLibraries(string[] libs)
        {
            foreach (string lib in libs)
            {
                Items.Items.Add(lib);
            }
            Items.SelectedIndex = -1;
        }

        public void SetHeader(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                HeaderText.Text = name;
                HeaderText.Visibility = Visibility.Visible;
            }
            else
            {
                HeaderText.Visibility = Visibility.Collapsed;
                HeaderText.Text = null;
            }
        }

        public int SelectedIndex()
        {
            return Items.SelectedIndex;
        }
    }
}
