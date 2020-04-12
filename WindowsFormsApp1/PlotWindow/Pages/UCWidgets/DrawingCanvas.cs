using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WindowsFormsApp1.PlotWindow.Pages.UCWidgets
{
    public class DrawingCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();

        public const double YZero = 50;
        public const double YMargin = 50;
        private double YLabelLen = 5;
        private double yHeight;
        private Brush line1Color = Brushes.Black;
        private Brush labelColor = Brushes.Black;
        private Brush axisColor = Brushes.Black;
        private Brush line2Color = Brushes.Black;
        private double thinkness = 1;
        private double canvasWidth = 0;
        private double xWidth;
        private const int yLinesCount = 12;

        List<LineDatas> allDatas;
        private double canvasHeight;
        int dataCount = 0;
        int rtCount = 0;
        public enum MouseMode
        {
            ZOOM,
            VIEW
        }
        public MouseMode mMode = MouseMode.VIEW;
        public DrawingCanvas()
        {
            this.Background = Brushes.Transparent;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Margin = new Thickness(0, 0, 0, YMargin);
            AllDatas = new List<LineDatas>();

        }

        //获取Visual的个数
        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        //获取Visual
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        //添加Visual
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        //删除Visual
        public void RemoveVisual(Visual visual)
        {
            visuals.Remove(visual);

            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        //命中测试
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }

        //使用DrawVisual画Polyline
        //DrawingVisual lineVisual;
        public void addData(string title, double min, double max, DoubleCollection xData, DoubleCollection mData, DoubleCollection sData)
        {
            AllDatas.Add(new LineDatas(title, min, max, xData, mData, sData));

            dataCount++;
            rtCount = mData.Count;
        }
        public void removeFirst()
        {
            for (int i = 0; i < AllDatas.Count; i++)
            {
                AllDatas[i].RtData.RemoveAt(0);
                AllDatas[i].ThData.RemoveAt(0);
            }
        }
        public void removeLast()
        {
            for (int i = 0; i < AllDatas.Count; i++)
            {
                AllDatas[i].RtData.RemoveAt(AllDatas[i].RtData.Count - 1);
                AllDatas[i].ThData.RemoveAt(AllDatas[i].ThData.Count - 1);
            }
        }
        /// <summary>
        /// 添加数据点
        /// </summary>
        /// <param name="xdatas">x坐标</param>
        /// <param name="rtdatas">实测数据</param>
        /// <param name="thdatas">理论数据</param>
        public void addPoint(double[] xdatas, double[] rtdatas, double[] thdatas)
        {
            if (rtdatas.Length != allDatas.Count)
                throw new Exception() { Source = "数据个数不匹配" };
            for (int i = 0; i < allDatas.Count; i++)
            {
                AllDatas[i].XData.Insert(0, xdatas[i]);
                AllDatas[i].RtData.Insert(0, rtdatas[i]);
                AllDatas[i].ThData.Insert(0, thdatas[i]);
                if (allDatas[i].Min > rtdatas[i])
                    allDatas[i].Min = rtdatas[i];
                if (allDatas[i].Max < rtdatas[i])
                    allDatas[i].Max = rtdatas[i];
            }
        }
        //清空所有数据
        public void clearData()
        {
            AllDatas.Clear();

        }
        //将数据清零
        public void cleanData()
        {
            foreach (LineDatas item in allDatas)
            {
                for (int i = 0; i < item.RtData.Count; i++)
                {
                    item.RtData[i] = 0;
                    item.ThData[i] = 0;
                }
                item.Min = -1;
                item.Max = 1;
            }
        }
        public void Polyline()
        {
            //如果虚画布没有数据
            if (this.visuals.Count == 0)
            {
                for (int i = 0; i < AllDatas.Count; i++)
                {
                    this.visuals.Add(new DrawingVisual());
                }
            }
            for (int count = 0; count < AllDatas.Count; count++)
            {
                //计算基础坐标系
                double x0, y0;
                x0 = YZero;
                y0 = (count + 1) * YMargin + count * yHeight;
                string title = AllDatas[count].Title;

                LineDatas datas = allDatas[count];
                DrawingVisual lineVisual = (DrawingVisual)this.visuals[count];
                DrawingContext dc = lineVisual.RenderOpen();
                Pen penAxis = new Pen(AxisColor, Thinkness);
                penAxis.Freeze();

                int yLabelMax = (int)AllDatas[count].Max + 1;
                int yLabelMin = (int)AllDatas[count].Min - 1;
                //画标题
                FormattedText fttitle = new FormattedText(title, new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 15, Brushes.Black);
                dc.DrawText(fttitle, new Point(xWidth / 2 + x0 - fttitle.Width / 2, y0 - fttitle.Height));
                //画Y轴
                dc.DrawLine(penAxis, new Point(x0, y0), new Point(x0, y0 + yHeight));
                dc.DrawLine(penAxis, new Point(x0 - YLabelLen, y0), new Point(x0, y0));
                dc.DrawLine(penAxis, new Point(x0 - YLabelLen, y0 + yHeight / 2), new Point(x0, y0 + yHeight / 2));
                dc.DrawLine(penAxis, new Point(x0 - YLabelLen, y0 + yHeight), new Point(x0, y0 + yHeight));
                //y轴文本
                FormattedText ft1 = new FormattedText(yLabelMax.ToString(), new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 10, Brushes.Black);
                dc.DrawText(ft1, new Point(x0 - YLabelLen - ft1.Width, y0 - ft1.Height / 2));
                FormattedText ft2 = new FormattedText(((yLabelMax + yLabelMin) / 2).ToString(), new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 10, Brushes.Black);
                dc.DrawText(ft2, new Point(x0 - YLabelLen - ft2.Width, y0 + YHeight / 2 - ft2.Height / 2));
                FormattedText ft3 = new FormattedText(yLabelMin.ToString(), new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 10, Brushes.Black);
                dc.DrawText(ft3, new Point(x0 - YLabelLen - ft3.Width, y0 + yHeight - ft3.Height / 2));
                //画线
                Pen linePen1 = new Pen(Line1Color, Thinkness);
                linePen1.Freeze();
                Pen linePen2 = new Pen(Line2Color, Thinkness);
                linePen2.Freeze();
                double ratio = (yLabelMax - yLabelMin) / yHeight;
                double step = xWidth / (datas.EndIndex - datas.StartIndex + 1);
                //Console.WriteLine("=======datastep:" + step);
                for (int i = datas.StartIndex; i < datas.EndIndex; i++)
                {
                    //将数值转换成位置   
                    //理论值 
                    Point p3 = new Point(x0 + (i - datas.StartIndex) * step, y0 + (yLabelMax - datas.ThData[i]) / ratio);
                    Point p4 = new Point(x0 + (i - datas.StartIndex + 1) * step, y0 + (yLabelMax - datas.ThData[i + 1]) / ratio);
                    dc.DrawLine(linePen2, p3, p4);
                    //实测值
                    Point p1 = new Point(x0 + (i - datas.StartIndex) * step, y0 + (yLabelMax - datas.RtData[i]) / ratio);
                    Point p2 = new Point(x0 + (i - datas.StartIndex + 1) * step, y0 + (yLabelMax - datas.RtData[i + 1]) / ratio);
                    dc.DrawLine(linePen1, p1, p2);
                }
                //绘制纵向网格和x轴文本
                Pen pen3 = new Pen(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#666666")), 1);
                pen3.DashStyle = new DashStyle(new double[] { 2.5, 2.5 }, 0);
                pen3.Freeze();
                double stepX = xWidth / yLinesCount;
                int stepData = (datas.EndIndex - datas.StartIndex + 1) / yLinesCount;
                if (stepData < 1)
                    stepData = 1;
                for (int i = 1; i < yLinesCount; i++)
                {
                    //纵向网格
                    Point p1 = new Point(x0 + i * stepX, y0 + yHeight);
                    Point p2 = new Point(x0 + i * stepX, y0);
                    dc.DrawLine(pen3, p1, p2);
                    //x轴文本
                    FormattedText ftX = new FormattedText(datas.XData[i * stepData].ToString(), new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 10, Brushes.Black);
                    Point p3 = new Point(x0 + i * stepX - ftX.Width / 2, yHeight + y0);
                    dc.DrawText(ftX, p3);
                }
                dc.Close();
            }
            this.InvalidateVisual();
        }

        public double YHeight
        {
            get
            {
                return yHeight;
            }

            set
            {
                yHeight = value;
            }
        }



        public Brush LabelColor
        {
            get
            {
                return labelColor;
            }

            set
            {
                labelColor = value;
            }
        }

        public Brush AxisColor
        {
            get
            {
                return axisColor;
            }

            set
            {
                axisColor = value;
            }
        }

        public double Thinkness
        {
            get
            {
                return thinkness;
            }

            set
            {
                thinkness = value;
            }
        }

        public static double YZero1
        {
            get
            {
                return YZero;
            }
        }

        public double CanvasWidth
        {
            get
            {
                return canvasWidth;
            }

            set
            {
                canvasWidth = value;
                this.Width = value;
                xWidth = value - YMargin - YZero;
            }
        }

        public Brush Line2Color
        {
            get
            {
                return line2Color;
            }

            set
            {
                line2Color = value;
            }
        }

        public Brush Line1Color
        {
            get
            {
                return line1Color;
            }

            set
            {
                line1Color = value;
            }
        }

        public double CanvasHeight
        {
            get
            {
                return canvasHeight;
            }

            set
            {
                canvasHeight = value;
            }
        }


        public int DataCount
        {
            get
            {
                return dataCount;
            }

            set
            {
                dataCount = value;
            }
        }

        public int RtCount
        {
            get
            {
                return rtCount;
            }

            set
            {
                rtCount = value;
            }
        }

        internal List<LineDatas> AllDatas
        {
            get
            {
                return allDatas;
            }

            set
            {
                allDatas = value;
            }
        }
    }
}
