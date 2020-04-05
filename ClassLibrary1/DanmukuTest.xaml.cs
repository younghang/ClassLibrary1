using BilibiliStyleDanmu;
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

namespace YHExcelAddin
{
    /// <summary>
    /// PlotPlatoForm.xaml 的交互逻辑
    /// </summary>
    public partial class DanmukuTest : Window
    {
        public DanmukuTest()
        {
            InitializeComponent();
        }
        MainOverlay biliDanmu = null;

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            if (biliDanmu != null )
            {
                biliDanmu.AddDMText("文件", "hello", "g");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (biliDanmu == null)
            {
                biliDanmu = new MainOverlay();
                biliDanmu.OpenOverlay();
                biliDanmu.Show();
            }
        }
    }
}
