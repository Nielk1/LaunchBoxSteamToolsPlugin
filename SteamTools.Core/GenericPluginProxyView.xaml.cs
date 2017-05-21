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
using Unbroken.LaunchBox.Plugins;
using Unbroken.LaunchBox.Plugins.Data;

namespace SteamTools
{
    /// <summary>
    /// Interaction logic for GenericPluginProxyView.xaml
    /// </summary>
    public partial class GenericPluginProxyView : UserControl, IBigBoxThemeElementPlugin
    {
        public IBigBoxThemeElementPlugin Proxy { get; set; }

        public GenericPluginProxyView()
        {
            InitializeComponent();
        }

        public bool OnUp(bool held) { return Proxy?.OnUp(held) ?? false; }
        public bool OnDown(bool held) { return Proxy?.OnDown(held) ?? false; }
        public bool OnLeft(bool held) { return Proxy?.OnLeft(held) ?? false; }
        public bool OnRight(bool held) { return Proxy?.OnRight(held) ?? false; }

        public bool OnPageUp() { return Proxy?.OnPageUp() ?? false; }
        public bool OnPageDown() { return Proxy?.OnPageDown() ?? false; }

        public bool OnEnter() { return Proxy?.OnEnter() ?? false; }
        public bool OnEscape() { return Proxy?.OnEscape() ?? false; }

        public void OnSelectionChanged(FilterType filterType, string filterValue, IPlatform platform, IPlatformCategory category, IPlaylist playlist, IGame game)
        {
            Proxy?.OnSelectionChanged(filterType, filterValue, platform, category, playlist, game);
        }
    }
}
