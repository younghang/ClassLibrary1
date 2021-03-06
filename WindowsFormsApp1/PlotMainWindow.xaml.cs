﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1.PlotWindow;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using Window = System.Windows.Window;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using WindowsFormsApp1.PlotWindow.Pages.UCWidgets;

namespace WindowsFormsApp1
{
    /// <summary>
    /// PlotPlatoForm.xaml 的交互逻辑
    /// </summary>
    public partial class PlotMainWindow : Window
    {
        public DataRegionInfo regionInfo = new DataRegionInfo();
        public static Excel.Application application;
        public DataRegionInfo GetRegionInformation()
        {
            return regionInfo;
        }
        public static BitmapSource CreateElementScreenshot(UIElement visual)
        {
            var renderSize = visual.RenderSize;            
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)renderSize.Width, (int)renderSize.Height, 96,96, PixelFormats.Default);
            bmp.Render(visual);

            return bmp;
        } 

        public PlotMainWindow()
        {
            InitializeComponent();
            this.dataPanel.DataContext = regionInfo;
            var menuPlot = new List<SubItem>();
            UserControlPlato ucpPlato = new UserControlPlato();
            ucpPlato.GetRegionInfos += GetRegionInformation; 
            menuPlot.Add(new SubItem("Plato", ucpPlato));
            
            menuPlot.Add(new SubItem("其他")); 
            var itemExcelPlot = new ItemMenu("Excel绘图", menuPlot, "plato.png");

            var menuDataAnalyse = new List<SubItem>();
            UCPHistogram uCPHistogram = new UCPHistogram();
            uCPHistogram.GetRegionInfos += GetRegionInformation;

            //UCPLineChart uCPLineChart = new UCPLineChart();

            menuDataAnalyse.Add(new SubItem("Histogram", uCPHistogram));
            menuDataAnalyse.Add(new SubItem("Lines"));
            var itemDataAnalyse = new ItemMenu("数据分析", menuDataAnalyse, "analyse.png");

            UCPDefault uCPDefault = new UCPDefault();
            uCPDefault.GetWindowContext+= delegate { return this; };
            var menuFileProcess = new List<SubItem>();
            menuFileProcess.Add(new SubItem("文本导出", uCPDefault));
            menuFileProcess.Add(new SubItem("其他"));
            var itemFileProcess = new ItemMenu("数据处理",menuFileProcess,"file.png");
            
            Menu.Children.Add(new UserControlMenuItem(itemFileProcess, this));
            Menu.Children.Add(new UserControlMenuItem(itemExcelPlot, this));
            Menu.Children.Add(new UserControlMenuItem(itemDataAnalyse, this));
        }
        public void UpdateMenuColor()
        {
            foreach(var item in Menu.Children )
            {
                (item as UserControlMenuItem).UpdateColor();
            }
        }
 

        internal void SwitchScreen(object sender)
        {
            if(PlotMainWindow.application!=null)
            SetSelectedRange();
            var screen = ((UserControl)sender);

            if (screen != null)
            {
                StackPanelMain.Children.Clear();
                StackPanelMain.Children.Add(screen);
            }
        }
        void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void dragPanelNotPanelButABorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            DragMove();
            e.Handled = true;

        }
        public delegate Range GetRangeDelegate();
        public event GetRangeDelegate GetRange;
        private Range rangeSelected;
        public void SetSelectedRange()
        {
            Range range = GetRange();
            if (range == null)
                return;
            if (range.Count == 1)
            {
                return;
            }
            rangeSelected = range;
            if (range.Areas.Count == 1)
            {
                if (range.Columns.Count == 2)
                { 
                    regionInfo.LableRegion = range.Columns[1].Address;
                    regionInfo.DataRegion = range.Columns[2].Address;
                }
                else
                {
                    regionInfo.LableRegion = range.Address;
                }

            }
            else if (range.Areas.Count == 2)
            {
                int i = 0;
                foreach (Range c in range.Areas)
                {
                    if (i == 0)
                    {
                        regionInfo.LableRegion = c.Address;
                        i++;
                    }
                    else
                    {
                        regionInfo.DataRegion = c.Address;
                    }

                }
            }
        }

        private void txtTitle_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void main_Activated(object sender, EventArgs e)
        {
            IntPtr handle2 = new WindowInteropHelper(this).Handle; ;
            SetForegroundWindow(handle2);
        }
        [DllImport("USER32.DLL")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
    public class DataRegionInfo : INotifyPropertyChanged
    {
        private  string lableTxt = "标签区域";
        private  string dataTxt = "数据区域";
        private string lblRegionString = "A1:A20";
        private string dataRegionString = "B1:B20";

        public string LableRegion
        {
            get { return lblRegionString; }
            set
            {
                lblRegionString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LableRegion"));
            }
        }
        public string DataRegion
        {
            get { return dataRegionString; }
            set
            {
                dataRegionString = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataRegion"));
            }
        }
        public string LableTxt
        {
            get { return lableTxt; }
            set
            {
                lableTxt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LableTxt"));
            }
        }
        public string DataTxt
        {
            get { return dataTxt; }
            set
            {
                dataTxt = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataTxt"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
