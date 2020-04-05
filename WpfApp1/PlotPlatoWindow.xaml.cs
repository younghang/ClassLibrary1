using System;
using System.Collections.Generic;
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

namespace WindowsFormsApp1
{
    /// <summary>
    /// PlotPlatoForm.xaml 的交互逻辑
    /// </summary>
    public partial class PlotPlatoWindow : Window
    {
        public PlotPlatoWindow()
        {
            InitializeComponent();
            var menuPlot = new List<SubItem>();
            menuPlot.Add(new SubItem("Plato", new UserControlPlato()));
            menuPlot.Add(new SubItem("其他")); 
            var itemExcelPlot = new ItemMenu("Excel绘图", menuPlot, "plato.png");

            var menuDataAnalyse = new List<SubItem>();
            menuDataAnalyse.Add(new SubItem("正态性检验"));
            menuDataAnalyse.Add(new SubItem("其他"));
            var itemDataAnalyse = new ItemMenu("数据分析", menuDataAnalyse, "analyse.png");
            
            var menuFileProcess = new List<SubItem>();
            menuFileProcess.Add(new SubItem("文本导出", new UserControlDefault()));
            menuFileProcess.Add(new SubItem("其他"));
            var itemFileProcess = new ItemMenu("数据处理",menuFileProcess,"file.png");
            
            Menu.Children.Add(new UserControlMenuItem(itemFileProcess, this));
            Menu.Children.Add(new UserControlMenuItem(itemExcelPlot, this));
            Menu.Children.Add(new UserControlMenuItem(itemDataAnalyse, this));
        }

        internal void SwitchScreen(object sender)
        {
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
    }
}
