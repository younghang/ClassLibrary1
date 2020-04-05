using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
namespace WindowsFormsApp1.PlotWindow
{
    /// <summary>
    /// UserControlPlato.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlPlato : UserControl,IGetRegionInfo
    {
        public UserControlPlato()
        {
            InitializeComponent();
            ListViewDataList = new ObservableCollection<ListViewData>();
            for (int i = 0; i < 50; i++)
            {
                ListViewDataList.Add(new ListViewData()
                {
                    Num = i,
                    Name = "No_" + i.ToString(),
                    IsShown = true,
                    Status="是"
                });
                ;
            }
            this.ListView.ItemsSource = ListViewDataList;
        }
        
        public ObservableCollection<ListViewData> ListViewDataList { get; set; }

        public event GetRegionInfo GetRegionInfos;

        public class ListViewData: INotifyPropertyChanged
        {
            public int Num { get; set; }
            public string Name { get; set; }
            private bool _isShown = true;
            private string _status = "是";
            public bool IsShown { get { return _isShown; }
                set {
                        _isShown = value;                  
                        Status = _isShown ? "是" : "否";
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsShown"));
                            
                } }
            public string Status { get { return _status; } set { 
                if(_status!=value)
                    {
                        _status = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
                    }
                } }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataRegionInfo regionInfo = GetRegionInfos();
            regionInfo.DataTxt = "PlatoData";
            regionInfo.DataRegion = "ACFBGJ";
            //Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            //if (app.Workbooks.Count == 0)
            //{
            //    richTextBox1.Text = "还没打开表格呢";
            //    return;
            //}
            //Range range1 = app.Range[regionInfo.LableRegion.Text.Replace('：', ':').Trim(new char[] { ' ', '\n' })];
            //Range range2 = app.Range[this.txt_2_range.Text.Replace('：', ':').Trim(new char[] { ' ', '\n' })];
            //if (range1.Columns.Count != 1 || range2.Columns.Count != 1)
            //{
            //    MessageBox.Show("数据区域均需为单列");
            //    return;
            //}
            //lastOne = false;
            //string seperateStr = this.textBox1.Text.Trim(new char[] { ' ', '\n' });
            //seperator = seperateStr.ToCharArray();
            //int length = Math.Min(range1.Count, range2.Count);
            ////这个会触发Excel SelectChange事件
            //int line = app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            //length = Math.Min(line, length);
            //IsSameColor1 = this.lbl_1_diff.BackColor == this.lbl_1_same.BackColor;
            //IsSameColor2 = this.lbl_2_diff.BackColor == this.lbl_2_same.BackColor;
            //for (int i = 0; i < length; i++)
            //{
            //    if (i == length - 1)
            //    {
            //        lastOne = true;
            //    }
            //    int c = (i + 1) / length * 100;
            //    if ((c - this.customProgressBar1.Value) > 10)
            //    {
            //        Console.WriteLine(c);
            //        this.customProgressBar1.Value = c;
            //        this.customProgressBar1.Invalidate();
            //    }
            //    SetRangeColor(range1.Cells[i + 1], range2.Cells[i + 1]);
            //}
            //if (rangeSelected != null)
            //{
            //    rangeSelected.Select();
            //}


        }
       
    }
}
