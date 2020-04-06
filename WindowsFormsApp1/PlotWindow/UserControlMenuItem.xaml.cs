using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Resources;
using System.Windows.Shapes;

namespace WindowsFormsApp1
{
    /// <summary>
    /// UserControlMenuItem.xaml 的交互逻辑
    /// </summary>
    public delegate DataRegionInfo GetRegionInfo();
    public interface IGetRegionInfo
    {
        event GetRegionInfo GetRegionInfos;
    }
    public partial class UserControlMenuItem : UserControl
    {
        PlotMainWindow _context;
        public UserControlMenuItem()
        {
            InitializeComponent();
        }
        public UserControlMenuItem(ItemMenu itemMenu, PlotMainWindow context)
        {
            InitializeComponent();
            _context = context;
            ExpanderMenu.Visibility = itemMenu.SubItems == null ? Visibility.Collapsed : Visibility.Visible;
            //ListViewItemMenu.Visibility = itemMenu.SubItems == null ? Visibility.Visible : Visibility.Collapsed;

            this.DataContext = itemMenu;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SubItem selectedItem =(SubItem)((ListView)sender).SelectedItem;
            if(selectedItem.Name== "Histogram")
            {
                this._context.regionInfo.DataTxt = "先不管";
                this._context.regionInfo.LableTxt = "Data";
            }
            _context.SwitchScreen(selectedItem.Screen);         
        }
    }
    public class ItemMenu
    {
        public ItemMenu(string header, List<SubItem> subItems, string imagePath)
        {
            Header = header;
            SubItems = subItems;
            Icon = imagePath;
        }

        public string Header { get; private set; }
        public string Icon { get; private set; }
        public List<SubItem> SubItems { get; private set; }
 
    }
    public class SubItem 
    {
        public SubItem(string name, UserControl screen = null)
        {
            Name = name;
            Screen = screen;
        }
        public string Name { get; private set; }
        public UserControl Screen { get; private set; }

 
    }
 

public class MenuIconConVerter : IValueConverter
    {
        BitmapImage LoadImage(string path)
        {
            StreamResourceInfo ifo = Application.GetResourceStream(new Uri("pack://application:,,,/WindowsFormsApp1;component/" + path, UriKind.Absolute));
            Stream data = ifo.Stream;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = data;
            image.EndInit();
            data.Close();
            return image;
        }
        //单向绑定
        public object Convert(object path, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            BitmapImage image = null;
            try
            {
                //image = new BitmapImage(new Uri("PlotWindow/Pic/" + path.ToString(), UriKind.Relative));
                image=LoadImage("PlotWindow/Pic/" + path.ToString()); 
            }
            catch (Exception)
            {
                image=LoadImage("PlotWindow/Pic/rect.png"); 
            }
            Button btn = new Button();
            btn.Width = 60;
            btn.Height = 60;
            btn.Background = new ImageBrush(image);
            return new VisualBrush(btn);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
