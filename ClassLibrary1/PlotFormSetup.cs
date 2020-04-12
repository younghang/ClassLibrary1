using ExcelDna.Integration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace YHExcelAddin
{
    public partial class PlotFormSetup : Form
    {
        public PlotFormSetup()
        {
            InitializeComponent();
        }
        public static void LoadWindow(Window win)
        {
            PlotFormSetup plot = new PlotFormSetup();
            plot.Show();
            //System.Windows.Forms.Application.Run(plot);
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(win);
            win.SourceInitialized += delegate
            {
                new Thread(
                    new ThreadStart(delegate { 
                        win.Closed += delegate
                        {
                            plot.Invoke(new Action(()=> {
                                //plot.WindowState = System.Windows.Forms.FormWindowState.Minimized;
                                plot.Close();
                                plot.Dispose();
                            }));
                        }; 
                    })
                    ).Start();
            };
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(win);
            win.Show();
        }
    
        private void btnStartExcelPlot_Click(object sender, EventArgs e)
        {
            PlotMainWindow.application = ExcelDnaUtil.Application as Microsoft.Office.Interop.Excel.Application;
            PlotMainWindow plotPlato = new PlotMainWindow();
            //System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(plotPlato);
            plotPlato.GetRange += RibbonUI.GetSelectRange;
            plotPlato.Show();
        }
    }
}
