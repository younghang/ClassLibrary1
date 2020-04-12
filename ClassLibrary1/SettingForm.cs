using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YHExcelAddin
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }
        public static string PLOT_WINDOW_IN_THREAD = "plt_window_in_thread";

        private void btnOK_Click(object sender, EventArgs e)
        {
            FileSettings.SetSettingItem(PLOT_WINDOW_IN_THREAD, ckPlotWindowInThread.Checked.ToString());
            this.Close();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            try { 
            ckPlotWindowInThread.Checked = bool.Parse(FileSettings.GetSettingItem(PLOT_WINDOW_IN_THREAD));
            }
            catch
            {
                ckPlotWindowInThread.Checked = false;
            }
        }
    }
}
