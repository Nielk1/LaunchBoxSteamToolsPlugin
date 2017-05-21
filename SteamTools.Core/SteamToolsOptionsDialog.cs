using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTools
{
    public partial class SteamToolsOptionsDialog : Form
    {
        public int PollingSteamRate { get { return (int)(nudPollRate.Value); } set { nudPollRate.Value = value; } }
        public bool ShowInstalled { get { return cbShow.Checked; } set { cbShow.Checked = value; } }
        public bool HideUninstalled { get { return cbHide.Checked; } set { cbHide.Checked = value; } }

        public SteamToolsOptionsDialog()
        {
            InitializeComponent();
            this.Icon = ((Icon)(new ComponentResourceManager(typeof(Resources)).GetObject("steam")));

            {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                Assembly[] assems = currentDomain.GetAssemblies();
                string LibVersion = assems.Where(assembly =>
                {
                    var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true).FirstOrDefault();
                    return attribute != null && attribute.Value == "b0442f11-eb3f-4af0-8b09-ec67d7d8d701";
                }).Select(assembly => assembly.GetName().Version.ToString()).FirstOrDefault();
                txtVersion.Text = LibVersion;
            }
        }
    }
}
