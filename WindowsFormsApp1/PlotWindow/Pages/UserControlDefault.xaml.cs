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

namespace WindowsFormsApp1.PlotWindow
{
    /// <summary>
    /// UserControlDefault.xaml 的交互逻辑
    /// </summary>
    public partial class UCPDefault : UserControl,IGetContext
    {
        public UCPDefault()
        {
            InitializeComponent();
        }

        public event GetContext GetWindowContext;

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var context = GetWindowContext();
            if (context != null)
            {
                context.wb.Background = (sender as Border).Background;
                context.contain.Background= (sender as Border).BorderBrush;
                Color color1= ToColor((sender as Border).Background.Clone().ToString());
                Color color2= ToColor((sender as Border).BorderBrush.Clone().ToString());
                Color color = HalfColor( color1 , color2);
                context.lableRegion.BorderBrush = new SolidColorBrush(color);
                context.UpdateMenuColor();
                //MessageBox.Show(context.lableRegion.BorderBrush.ToString());
            } 
        }
        private Color HalfColor(Color c1,Color c2)
        {
            return Color.FromArgb( Convert.ToByte((c1.A + c2.A) / 2),
                Convert.ToByte((c1.R + c2.R) / 2),
                 Convert.ToByte((c1.G + c2.G) / 2),
                 Convert.ToByte((c1.B + c2.B) / 2) );
        }
        public Color ToColor(string colorName)
        {
            if (colorName.StartsWith("#"))
                colorName = colorName.Replace("#", string.Empty);
            int v = int.Parse(colorName, System.Globalization.NumberStyles.HexNumber);
            return new Color()
            {
                A = Convert.ToByte((v >> 24) & 255),
                R = Convert.ToByte((v >> 16) & 255),
                G = Convert.ToByte((v >> 8) & 255),
                B = Convert.ToByte((v >> 0) & 255)
            };
        }
    }
}
