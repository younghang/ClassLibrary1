using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace WindowsFormsApp1.PlotWindow.Pages.UCWidgets
{
    class LineDatas
    {
        string title;
        double min;
        double max;
        DoubleCollection xData;
        DoubleCollection rtData;
        DoubleCollection thData;
        int startIndex;
        int endIndex;
        public double Min
        {
            get
            {
                return min;
            }

            set
            {
                min = value;
            }
        }

        public double Max
        {
            get
            {
                return max;
            }

            set
            {
                max = value;
            }
        }



        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
            }
        }

        public DoubleCollection RtData
        {
            get
            {
                return rtData;
            }

            set
            {
                rtData = value;
                min = value.Min();
                max = value.Max();
            }
        }

        public DoubleCollection ThData
        {
            get
            {
                return thData;
            }

            set
            {
                thData = value;

            }
        }

        public int StartIndex
        {
            get
            {
                return startIndex;
            }

            set
            {
                startIndex = value;
            }
        }

        public int EndIndex
        {
            get
            {
                return endIndex;
            }

            set
            {
                endIndex = value;
            }
        }

        public DoubleCollection XData
        {
            get
            {
                return xData;
            }

            set
            {
                xData = value;
            }
        }

        public LineDatas()
        {
            this.title = "";
            this.Max = 0;
            this.Min = 0;
            this.XData = new DoubleCollection();
            this.RtData = new DoubleCollection();
            this.ThData = new DoubleCollection();

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_title">数据标题</param>
        /// <param name="min">数据最小值</param>
        /// <param name="max">数据最大值</param>
        /// <param name="_xData">x坐标</param>
        /// <param name="rtdata">实测数据</param>
        /// <param name="thdata">理论数据</param>
        public LineDatas(string _title, double min, double max, DoubleCollection _xData, DoubleCollection rtdata, DoubleCollection thdata)
        {
            this.title = _title;
            this.Min = min;
            this.Max = max;
            this.XData = _xData;
            this.RtData = rtdata;
            this.ThData = thdata;
            this.StartIndex = 0;
            this.EndIndex = rtData.Count - 1;
        }
        public void clear()
        {
            this.XData.Clear();
            this.RtData.Clear();
            this.ThData.Clear();
        }
    }
}
