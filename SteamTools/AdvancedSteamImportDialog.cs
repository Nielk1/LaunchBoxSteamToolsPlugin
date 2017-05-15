using SteamLaunchable = CarbyneSteamContext.SteamLaunchable;
using SteamLaunchableApp = CarbyneSteamContext.SteamLaunchableApp;
using SteamLaunchableModGoldSrc = CarbyneSteamContext.SteamLaunchableModGoldSrc;
using SteamLaunchableModSource = CarbyneSteamContext.SteamLaunchableModSource;
using CarbyneSteamContextWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unbroken.LaunchBox.Plugins.Data;
using System.IO;

namespace SteamTools
{
    public partial class AdvancedSteamImportDialog : Form
    {
        private SteamContext context;
        List<SteamLaunchable> SteamApps;
        //List<SteamLaunchableListViewitem> ListItems;

        //private int clearImageIndex = 0;
        //private Dictionary<string, int> ImageIndexMap = new Dictionary<string, int>();
        private Dictionary<SteamLaunchable, bool> CheckedItems = new Dictionary<SteamLaunchable, bool>();

        public AdvancedSteamImportDialog()
        {
            SteamApps = new List<SteamLaunchable>();
            //ListItems = new List<SteamLaunchableListViewitem>();
            context = SteamContext.GetInstance();
            InitializeComponent();
            this.Icon = ((Icon)(new ComponentResourceManager(typeof(Resources)).GetObject("steam")));
            lvGames.SmallImageList = new ImageList();
        }

        internal void SetPlatforms(IPlatform[] platforms)
        {
            cbPlatforms.BeginUpdate();
            cbPlatforms.Items.Clear();
            foreach (IPlatform plat in platforms)
            {
                cbPlatforms.Items.Add(plat.Name);
            }
            cbPlatforms.SelectedItem = "Steam";
            if(cbPlatforms.SelectedIndex == -1)
                cbPlatforms.SelectedItem = "Windows";
            if (cbPlatforms.SelectedIndex == -1)
                cbPlatforms.SelectedIndex = 0;
            cbPlatforms.EndUpdate();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            lvGames.BeginUpdate();
            SteamApps.Clear();
            lvGames.SmallImageList.Images.Clear();
            CheckedItems.Clear();
            //clearImageIndex = 0;
            //ImageIndexMap.Clear();
            //ListItems.Clear();
            List<SteamLaunchableApp> apps = context.GetOwnedApps().Where(dr => new[] { "game", "demo", "application" }.Contains(dr.appType)).ToList();
            List<SteamLaunchableModGoldSrc> gmods = context.GetGoldSrcMods();
            List<SteamLaunchableModSource> smods = context.GetSourceMods();
            SteamApps.AddRange(apps);
            SteamApps.AddRange(gmods);
            SteamApps.AddRange(smods);

            /*foreach(SteamLaunchable app in SteamApps)
            {
                ListItems.Add(new SteamLaunchableListViewitem(app));
            }*/

            //lvGames.VirtualListSize = ListItems.Count;
            lvGames.VirtualListSize = SteamApps.Count;
            lvGames.EndUpdate();
        }

        private void lvGames_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            SteamLaunchable item = SteamApps[e.ItemIndex];
            if (e.Item == null) e.Item = new ListViewItem(item.Title);
            e.Item.SubItems.Add(item.GetShortcutID().ToString());
            e.Item.SubItems.Add(item.AppType);
            if (!string.IsNullOrEmpty(item.Icon) && File.Exists(item.Icon))
            {
                //if (!ImageIndexMap.ContainsKey(item.Icon))
                if (!lvGames.SmallImageList.Images.ContainsKey(item.Icon))
                {
                    //ImageIndexMap.Add(item.Icon, clearImageIndex++);
                    //lvGames.SmallImageList.Images.Add(Icon.ExtractAssociatedIcon(item.Icon));
                    lvGames.SmallImageList.Images.Add(item.Icon, Icon.ExtractAssociatedIcon(item.Icon));
                }
                //e.Item.ImageIndex = ImageIndexMap[item.Icon];
                e.Item.ImageKey = item.Icon;
            }
            //else if (!string.IsNullOrEmpty(item.Icon) && File.Exists(Path.ChangeExtension(item.Icon,".tga")))
            //{
            //    //if (!ImageIndexMap.ContainsKey(item.Icon))
            //    if (!lvGames.SmallImageList.Images.ContainsKey(item.Icon))
            //    {
            //        using (var fs = new System.IO.FileStream(Path.ChangeExtension(item.Icon, ".tga"), FileMode.Open, FileAccess.Read, FileShare.Read))
            //        using (var reader = new System.IO.BinaryReader(fs))
            //        {
            //            var tga = new TgaLib.TgaImage(reader);
            //            System.Windows.Media.Imaging.BitmapSource source = tga.GetBitmap();
            //            lvGames.SmallImageList.Images.Add(item.Icon, new Icon(source));
            //        }
            //    }
            //    //e.Item.ImageIndex = ImageIndexMap[item.Icon];
            //    e.Item.ImageKey = item.Icon;
            //}
            else
            {
                e.Item.ImageKey = "default";
            }
        }

        private void lvGames_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            lvGames.BeginUpdate();
            switch(e.Column)
            {
                case 1:
                    SteamApps = SteamApps.OrderBy(dr => dr.GetShortcutID()).ThenBy(dr => dr.Title).ThenBy(dr => dr.AppType).ToList();
                    break;
                case 2:
                    SteamApps = SteamApps.OrderBy(dr => dr.AppType).ThenBy(dr => dr.Title).ToList();
                    break;
                case 0:
                default:
                    SteamApps = SteamApps.OrderBy(dr => dr.Title).ThenBy(dr => dr.AppType).ToList();
                    break;
            }
            lvGames.EndUpdate();
        }

        private void lvGames_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
            if (lvGames.SmallImageList.Images.ContainsKey(e.Item.ImageKey))
            {
                Rectangle rect = e.Bounds;
                rect.Width = 16;
                rect.Height = 16;
                rect.X += 16;
                e.Graphics.DrawImage(lvGames.SmallImageList.Images[e.Item.ImageKey], rect);
            }
            /*if (!e.Item.Checked)
            {
                e.Item.Checked = true;
                e.Item.Checked = false;
            }*/
            {
                Rectangle rect = e.Bounds;
                rect.Width = 10;
                rect.Height = 10;
                rect.X += 2;
                rect.Y += 2;
                e.Graphics.DrawRectangle(Pens.Black, rect);
            }
            SteamLaunchable app = SteamApps[e.ItemIndex];
            if (CheckedItems.ContainsKey(app) && CheckedItems[app])
            {
                Rectangle rect = e.Bounds;
                rect.Width = 7;
                rect.Height = 7;
                rect.X += 4;
                rect.Y += 4;
                e.Graphics.FillRectangle(Brushes.Black, rect);
            }
        }

        private void lvGames_MouseClick(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
            if (lvi != null)
            {
                if (e.X < (lvi.Bounds.Left + 16))
                {
                    if (lv.SelectedIndices.Contains(lvi.Index))
                    {
                        SteamLaunchable app = SteamApps[lvi.Index];
                        if (!CheckedItems.ContainsKey(app)) CheckedItems[app] = false;
                        CheckedItems[app] = !CheckedItems[app];
                        if (lv.SelectedIndices.Count > 0)
                        {
                            foreach (int index in lv.SelectedIndices)
                            {
                                SteamLaunchable appX = SteamApps[index];
                                if (!CheckedItems.ContainsKey(appX)) CheckedItems[appX] = false;
                                CheckedItems[appX] = CheckedItems[app];
                            }
                            lv.Invalidate();
                        }
                    }
                    else
                    {
                        SteamLaunchable app = SteamApps[lvi.Index];
                        if (!CheckedItems.ContainsKey(app)) CheckedItems[app] = false;
                        CheckedItems[app] = !CheckedItems[app];
                        lv.Invalidate(lvi.Bounds);
                    }
                }
            }
        }

        private void lvGames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView lv = (ListView)sender;
            ListViewItem lvi = lv.GetItemAt(e.X, e.Y);
            if (lvi != null)
                lv.Invalidate(lvi.Bounds);
        }

        private void lvGames_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void lvGames_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }
    }

    /*internal class SteamLaunchableListViewitem : ListViewItem
    {
        private SteamLaunchable app;

        public SteamLaunchableListViewitem(SteamLaunchable app)
        {
            this.app = app;
        }

        public override string ToString()
        {
            return app.Title;
        }
    }*/
}
