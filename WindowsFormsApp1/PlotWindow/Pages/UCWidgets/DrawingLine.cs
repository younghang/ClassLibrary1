using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
namespace WindowsFormsApp1.PlotWindow.Pages.UCWidgets
{
    class DrawingLine : Canvas
    {
        private List<Visual> visuals = new List<Visual>();
        DrawingCanvas drawingCanvas;
        private double x0;
        private double y0;
        private double mWidth;
        private double mHeight;
        public enum MouseMode
        {
            ZOOM,
            VIEW
        }
        public MouseMode mMode = MouseMode.VIEW;
        private double canvasWidth;
        public DrawingLine(DrawingCanvas _dc)
        {
            this.drawingCanvas = _dc;
            this.Background = Brushes.Transparent;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.Height = drawingCanvas.CanvasHeight;
            this.Width = drawingCanvas.CanvasWidth;

            x0 = DrawingCanvas.YZero;
            y0 = DrawingCanvas.YMargin;

            mHeight = _dc.AllDatas.Count * (DrawingCanvas.YMargin + _dc.YHeight);

            this.PreviewMouseLeftButtonDown += DrawingCanvas_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += DrawingCanvas_MouseLeftButtonUp;
            this.MouseMove += DrawingCanvas_MouseMove;
            this.MouseLeave += DrawingCanvas_MouseLeave;
        }
        //鼠标离开画布
        private void DrawingCanvas_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mMode == MouseMode.VIEW)
            {
                this.RemoveVisual(textVisual);
                this.InvalidateVisual();
            }
        }
        //选中放大区域完成，显示放大区域
        private void DrawingCanvas_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isMouseDown = false;
            mMode = MouseMode.VIEW;
            endup = e.GetPosition(this).X;
            if (endup < x0)
                endup = x0;
            if (endup > x0 + mWidth)
                endup = x0 + mWidth;
            //重画 选中区域
            int len = drawingCanvas.AllDatas[0].EndIndex - drawingCanvas.AllDatas[0].StartIndex + 1;
            if (drawingCanvas.AllDatas.Count < 1 || len < 1)
                return;
            double step = mWidth / len;
            if (isMouseMoved && Math.Abs(endup - startDown) > step)
            {
                int startIndex = 0;
                int endIndex = 0;
                if (endup > startDown)
                {
                    startIndex = (int)((startDown - x0) / step) + 1;
                    endIndex = (int)((endup - x0) / step);
                }
                else
                {
                    startIndex = (int)((endup - x0) / step) + 1;
                    endIndex = (int)((startDown - x0) / step);
                }

                foreach (LineDatas data in drawingCanvas.AllDatas)
                {
                    data.EndIndex = data.StartIndex + endIndex;
                    data.StartIndex = data.StartIndex + startIndex;
                }
                drawingCanvas.Polyline();
                this.RemoveVisual(rectVisual);
                this.InvalidateVisual();
            }
            isMouseMoved = false;
        }
        double preMoved = DrawingCanvas.YZero;
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        //long preTime = 0;
        private const long MIN_TIME = 15000;
        private void DrawingCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

            double positionX = e.GetPosition(this).X;
            if (mMode == MouseMode.VIEW)
            {
                int len = drawingCanvas.AllDatas[0].EndIndex - drawingCanvas.AllDatas[0].StartIndex + 1;
                if (positionX > x0 && positionX < x0 + mWidth && Math.Abs(positionX - preMoved) >= mWidth / len)
                {
                    //计算速度鼠标移动速度，如果速度过快 ，则不绘制
                    if (stopwatch.IsRunning)
                    {
                        stopwatch.Stop();
                    }
                    long curTime = stopwatch.ElapsedTicks;
                    //Console.WriteLine(curTime);
                    if (curTime > MIN_TIME)
                    {
                        this.PolyText(positionX);
                    }
                    stopwatch.Restart();
                    preMoved = positionX;

                }
            }
            else if (isMouseDown && startDown > x0 && startDown < mWidth + x0)
            {
                isMouseMoved = true;
                this.PolyRect(startDown, positionX);
            }


            //stopwatch.Start();
        }

        private bool isMouseDown = false;
        private bool isMouseMoved = false;
        private double startDown;
        private double endup;
        private int clickTimes = 0;
        private void DrawingCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            isMouseDown = true;
            mMode = MouseMode.ZOOM;
            startDown = e.GetPosition(this).X;
            this.RemoveVisual(textVisual);
            this.InvalidateVisual();
            //双击
            clickTimes++;
            DispatcherTimer timer = new DispatcherTimer();

            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);

            timer.Tick += (s, e1) => { timer.IsEnabled = false; clickTimes = 0; };

            timer.IsEnabled = true;

            if (clickTimes % 2 == 0)
            {
                //Console.WriteLine("double");
                timer.IsEnabled = false;

                clickTimes = 0;
                foreach (LineDatas data in drawingCanvas.AllDatas)
                {
                    data.StartIndex = 0;
                    data.EndIndex = data.RtData.Count - 1;
                }

                drawingCanvas.Polyline();
                drawingCanvas.InvalidateVisual();
                //this.InvalidateVisual();              
            }
        }


        //获取Visual的个数
        protected override int VisualChildrenCount
        {
            get { return visuals.Count; }
        }

        public double MWidth
        {
            get
            {
                return mWidth;
            }

            set
            {
                mWidth = value;
            }
        }

        public double MHeight
        {
            get
            {
                return mHeight;
            }

            set
            {
                mHeight = value;
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
                mWidth = value - x0 - y0;
            }
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
            //visual = null;
        }

        //命中测试
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult.VisualHit as DrawingVisual;
        }


        //绘制鼠标选择放大的矩形
        DrawingVisual rectVisual;
        public void PolyRect(double startx, double endx)
        {
            if (rectVisual != null)
            {
                this.RemoveVisual(rectVisual);
            }
            rectVisual = new DrawingVisual();
            DrawingContext dc = rectVisual.RenderOpen();

            Pen pen = new Pen(new SolidColorBrush(Color.FromArgb(100, 255, 200, 200)), 1);
            dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(100, 255, 200, 200)), pen, new Rect(new Point(startx, y0), new Point(endx, y0 + mHeight)));
            dc.Close();
            this.AddVisual(rectVisual);
            this.InvalidateVisual();
        }
        //绘制显示选中的数值
        DrawingVisual textVisual;
        public void PolyText(double xPosition)
        {
            if (drawingCanvas.AllDatas.Count < 1)
                return;
            if (textVisual != null)
            {
                this.RemoveVisual(textVisual);
            }
            textVisual = new DrawingVisual();

            DrawingContext dc = textVisual.RenderOpen();
            int len = drawingCanvas.AllDatas[0].EndIndex - drawingCanvas.AllDatas[0].StartIndex + 1;
            double step = mWidth / len;
            // Console.WriteLine("*****linestep:" + step);
            int index = (int)((xPosition - x0) / step);
            double ax = x0 + index * step;
            //竖线
            Pen pen = new Pen(Brushes.Transparent, 3);
            pen.Freeze();
            FormattedText[] ftXs = new FormattedText[drawingCanvas.AllDatas.Count];

            for (int i = 0; i < drawingCanvas.AllDatas.Count; i++)
            {
                int mIndex = index + drawingCanvas.AllDatas[i].StartIndex;
                ftXs[i] = new FormattedText("X:" + drawingCanvas.AllDatas[i].XData[mIndex] + " 实测:" + drawingCanvas.AllDatas[i].RtData[mIndex] + " 设计:" + drawingCanvas.AllDatas[i].ThData[mIndex], new System.Globalization.CultureInfo("zh-CHS", false), FlowDirection.LeftToRight, new Typeface("Microsoft YaHei"), 15, Brushes.White);
                //计算是否超出范围
                if (ax + ftXs[i].Width < x0 + mWidth)
                {
                    dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(200, 0, 150, 179)), pen, new Rect(new Point(ax, (i + 1) * y0 + i * drawingCanvas.YHeight), new Point(ax + ftXs[i].Width, (i + 1) * y0 + i * drawingCanvas.YHeight + ftXs[i].Height)));
                    dc.DrawText(ftXs[i], new Point(ax, (i + 1) * y0 + i * drawingCanvas.YHeight));
                }
                else
                {
                    dc.DrawRectangle(new SolidColorBrush(Color.FromArgb(200, 0, 150, 179)), pen, new Rect(new Point(ax - ftXs[i].Width, (i + 1) * y0 + i * drawingCanvas.YHeight), new Point(ax, (i + 1) * y0 + i * drawingCanvas.YHeight + ftXs[i].Height)));
                    dc.DrawText(ftXs[i], new Point(ax - ftXs[i].Width, (i + 1) * y0 + i * drawingCanvas.YHeight));
                }
            }
            Pen penLine = new Pen(new SolidColorBrush(Color.FromArgb(200, 0, 150, 179)), 3);
            penLine.DashStyle = new DashStyle(new double[] { 2.5, 2.5 }, 0);
            penLine.Freeze();
            dc.DrawLine(penLine, new Point(ax, y0), new Point(ax, y0 + mHeight));
            dc.Close();
            this.AddVisual(textVisual);
            this.InvalidateVisual();
        }

    }
}
