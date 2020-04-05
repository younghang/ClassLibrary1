using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsFormsApp1.PlotWindow.Pages.UCWidgets;
 
namespace WindowsFormsApp1.PlotWindow
{
    /// <summary>
    /// UserControlDefault.xaml 的交互逻辑
    /// </summary>
    public partial class UCPHistogram : UserControl, IGetRegionInfo
    {
        public UCPHistogram()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = groupCount;
            //binding.Path = groupCount.ToString();
            //binding.Mode = BindingMode.TwoWay;
            //binding.Converter = new SliderConverter();
            //binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //this.slider.SetBinding(Slider.ValueProperty, binding);
        }
      
        public List<double> originalData = new List<double>();
        double UL = 1000;
        double LL = 0;
        double MaxNum = -10000000;
        double MinNum = 10000000;
        List<double> rankValue = null;
        int groupCount =16;

        public void SetData()
        {
            standard_deviation=0;
            mean = 0;
            sum = 0;
            square_sum = 0;
            UL = double.Parse(dataUpperLimits.Text) ;
            LL = double.Parse(dataLowerLimits.Text);
            var data= File.ReadAllLines("./data.txt");
            groupCount = (int)slider.Value;
            rankValue = new List<double>();
            var c = from d in data 
                           where IsInRange(d)
                           let dd = double.Parse(d)
                           orderby dd ascending
                    select dd;
            originalData = c.ToList();
            SortData();
        }
        int currentRank = 0;
        double currentRef = 0;
        class HisData
        {

            public double value;
            public double percent;
            public HisData(double value, double percent)
            {
                this.value = value;
                this.percent = percent;
            }
        }
        List<HisData> datas = new List<HisData>();
        private static bool Computing = false;
        private void SortData()
        {
            currentRank = -1;
            currentRef = MinNum;
            rankValue.Add(currentRef);
            var temp=from d in originalData
                     group d by GetRank(d) into nums
                     select new HisData(rankValue[nums.Key], (100.0 * nums.Count() / originalData.Count()));

            //select new {value=rankValue[nums.Key],percent= (100.0 * nums.Count() / originalData.Count()).ToString("f2") };
            datas = temp.ToList();
            this.txtInfo.Text = "计算中...";
            new Thread(new ThreadStart(() =>
            {
                if (Computing)
                    return;
                Computing = true;
                this.mean = this.sum / this.originalData.Count();
                this.standard_deviation = Math.Sqrt(this.square_sum / this.originalData.Count() - this.mean * this.mean);
                this.GetPValue();
                this.txtInfo.Dispatcher.Invoke(new Action(()=>
                {
                this.txtInfo.Text =
                    "\nCount:\n" + this.originalData.Count +
                    "\nMean:\n" + this.mean.ToString()
                    + "\nsigma:\n" + this.standard_deviation
                    + "\npValue:\n" + this.pValue
                    + "\nKS:\n" + ks.ToString("f2");
                    Computing = false;
                }));
             
            })).Start();
           

           
            //string s = "";
            //foreach(var i in temp)
            //{
            //    s += i.value+"    " + i.percent+"\n";
            //}
            //File.WriteAllText("result.txt", s);
        }
        private int GetRank(double d)
        {         
           if(d>=currentRef)
            {
                currentRank++;
                currentRef = MinNum + (1+currentRank)*(MaxNum - MinNum) / groupCount;
                rankValue.Add(currentRef);
            }
            return currentRank;          

        }
        double standard_deviation;        
        double mean;
        double sum;
        double square_sum;
        private bool IsInRange(string s)
        {
            try
            {
                double c=double.Parse(s);
                if (c > UL || c < LL)
                {
                    return false;
                }
                else
                {
                    if (c > MaxNum)
                        MaxNum = c;
                    if (c < MinNum)
                        MinNum = c;
                    sum += c;
                    square_sum += c * c;
                    return true;
                }
            }catch(Exception e)
            {
                return false;
            }

        }
        /// <summary>
        /// 复化辛浦生公式
        /// </summary>
        /// <param name="fun">被积函数</param>
        /// <param name="N">区间划分快数</param>
        /// <param name="up">积分上限</param>
        /// <param name="down">积分下限</param>
        /// <returns>积分值</returns>
        public static double FSimpson(Func<double, double> fun, int N, double up, double down)
        {
            double h = (up - down) / N;
            double result = 0;
            for (int n = 0; n < N; n++)
            {
                result += h / 6 * (fun(down) + 4 * fun(down + h / 2) + fun(down + h));
                down += h;
            }
            return result;
        }
        public static double GetGaussianCumulativeDistributionFunction(double end)
        {
            double result = 0;
            result= FSimpson(x=>1/Math.Sqrt(2*Math.PI)*Math.Exp(-x*x/2),1000,end,-5);
            return result;
        } 
        double ks=0;
        public void GetPValue2()
        {
            ks = 0;
            pValue = 0;
            int n = originalData.Count;
            double cumulateDensity = 0;
            for (int i = 0; i < datas.Count; i++)
            { 
                cumulateDensity += datas[i].percent / 100;
                double normalizeValue = (datas[i].value - this.mean) / this.standard_deviation;
                double std_cd = GetGaussianCumulativeDistributionFunction(normalizeValue);
                double differ = Math.Abs(cumulateDensity - std_cd);
                if (differ > ks)
                    ks = differ;
            }
            double s = n * ks * ks;
            pValue = 2 * Math.Exp(-(2.000071 + 0.331 / Math.Sqrt(n) + 1.409 / n) * s); 
        }
        double pValue;
        public void GetPValue()
        {
            ks = 0;
            pValue = 0;
            int n = originalData.Count;
            double cumulateDensity = 0;
            for (int i=0;i<n;i++)
            { 

                cumulateDensity =1.0*(i+1)/n;
                double normalizeValue = (originalData[i] - this.mean) / this.standard_deviation;
                double std_cd = GetGaussianCumulativeDistributionFunction(normalizeValue);
                double differ = Math.Abs(cumulateDensity - std_cd);
                if (differ > ks)
                    ks = differ;
            }
            double s = n * ks * ks;
            pValue = 2 * Math.Exp(-(2.000071 + 0.331 / Math.Sqrt(n) + 1.409 / n) * s); 
        }
        public event GetRegionInfo GetRegionInfos;
        AxisXModel axisX;
        AxisYModel axisY;
        List<AxisXDataModel> dataX;
        List<AxisYDataModel> dataY;
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SetData();
            axisX = new AxisXModel();
            axisX.Height = 20;
            axisX.ForeGround = Brushes.White;
            dataX = new List<AxisXDataModel>();            
            foreach (var i in datas)
            {
                AxisXDataModel a = new AxisXDataModel();
                a.Name = i.value.ToString("f2");
                a.Value = i.percent;               
                a.FillBrush = Colors.DeepSkyBlue;
                a.FillEndBrush=(Color)ColorConverter.ConvertFromString("#FF6CD7FB");
                dataX.Add(a);
            }
            axisX.Datas = dataX;

            axisY = new AxisYModel();
            axisY.Width = 40;
            dataY = new List<AxisYDataModel>();
            int MaxLabelCount = 5;
            double MaxY = this.datas.Max((t) => (t.percent))*1.2;
            for (int i = 0; i < MaxLabelCount; i++)
            {
                AxisYDataModel a = new AxisYDataModel();

                a.Name = (i * MaxY / MaxLabelCount).ToString("f2");
                a.Value = (i * MaxY / MaxLabelCount);
                dataY.Add(a);
            }
            axisY.Titles = dataY;
            this.barChart.DrawData(axisX, axisY);
            //this.barChart.Dispatcher.Invoke((Action)delegate ()  {
            //    this.barChart.DrawData(axisX, axisY); 
            //}
            // );
           
        }
    }

    public class SliderConverter : IValueConverter
    {
        //用于将绑定源数据转换成目标属性数据类型
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((int)value).ToString();
        }

        //用于将目标属性数据转换成绑定源数据
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return int.Parse((string)value);
        }
    }
 
 
}
