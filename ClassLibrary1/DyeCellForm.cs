using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
namespace YHExcelAddin
{
    
    public partial class DyeCellForm : Form
    {
        public DyeCellForm()
        {
            InitializeComponent(); 
            this.customProgressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.customProgressBar1.ForeColor = System.Drawing.Color.LimeGreen;
            this.customProgressBar1.TabIndex = 70; 
        }
 

        internal void SetSelectedRange(Range range)
        {
            if (range == null)
                return;            
            if(range.Count==1)
            {
                return;
            }
            rangeSelected = range;            
            if (range.Areas.Count == 1)
            {
                if (range.Columns.Count == 2)
                {
                    //这个杂碎从1开始，且不说这个，调试竟然显示不存在,什么几把玩意儿
                    txt_1_range.Text = range.Columns[1].Address;
                    txt_2_range.Text = range.Columns[2].Address;
                }
                else
                {
                    txt_1_range.Text = range.Address;
                }

            }
            else if (range.Areas.Count == 2)
            {
                int i = 0;
                foreach (Range c in range.Areas)
                {
                    if (i == 0)
                    {
                        txt_1_range.Text = c.Address;
                        i++;
                    }
                    else
                    {
                        txt_2_range.Text = c.Address;
                    }

                }
            }
        }
        private Range rangeSelected;
        private void Form1_Load(object sender, EventArgs e)
        {
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            Range range = app.Selection;
            SetSelectedRange(range);
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.customProgressBar1.Value = 0;
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            Range range1=app.Range[this.txt_1_range.Text.Replace('：',':').Trim(new char[] { ' ', '\n' })];
            Range range2 = app.Range[this.txt_2_range.Text.Replace('：', ':').Trim(new char[] { ' ', '\n' })];
            if(range1.Columns.Count!=1||range2.Columns.Count!=1)
            {
                MessageBox.Show("数据区域均需为单列");
                return;
            }
            lastOne = false;
            string seperateStr = this.textBox1.Text.Trim(new char[] { ' ', '\n' });
            seperator = seperateStr.ToCharArray();
            int length = Math.Min(range1.Count, range2.Count);
            //这个会触发Excel SelectChange事件
            int line = app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            length = Math.Min(line, length);
            IsSameColor1 = this.lbl_1_diff.BackColor == this.lbl_1_same.BackColor;
            IsSameColor2 = this.lbl_2_diff.BackColor == this.lbl_2_same.BackColor;
            for (int i=0;i<length;i++)
            {   
                if(i==length-1)
                {
                    lastOne = true;                    
                }
                int c = (i + 1) / length * 100;
                if((c-this.customProgressBar1.Value)>10)
                {
                    Console.WriteLine(c);
                    this.customProgressBar1.Value = c;
                    this.customProgressBar1.Invalidate();
                }                
                SetRangeColor(range1.Cells[i + 1], range2.Cells[i + 1]);                
            }
            if (rangeSelected != null)
            {
                rangeSelected.Select();
            }


        } 
        private bool IsSameColor1 = true;
        private bool IsSameColor2 = true;
        private char[] seperator;
        private void SetRangeColor(Range a,Range b)
        {
            if(a.FormulaR1C1Local==""||b.FormulaR1C1Local=="")
            {
                return;
            }
            a.Font.Color = lbl_1_same.BackColor;
            b.Font.Color = lbl_2_same.BackColor;
            if(!IsSameColor1)
            {
                SetCellColor(a, b.FormulaR1C1Local + "",this.lbl_1_diff.BackColor);
            }
           
            if (!IsSameColor2)
            {
                SetCellColor(b, a.FormulaR1C1Local + "",this.lbl_2_diff.BackColor);
            } 
        }
        private bool lastOne=false;
        private void SetCellColor(Range a,string s,Color color)
        {
            //string str1 = "p9605, p98,P60,P30   \n";
            //string str2 = "p9505,P98, P61,P33 ,P30 \n";
            string str1 = s;
            string str2 = a.FormulaR1C1Local;
            if(str2=="")
            {
                return;
            }
            string str1Temp = str1.ToUpper();
            string str2Temp = str2.ToUpper();
            string[] str1Sub = str1Temp.Split(seperator);
            List<string> strItems = new List<string>();
            for (int i = 0; i < str1Sub.Length; i++)
            {
                str1Sub[i] = str1Sub[i].Trim(new char[] { ' ', '\n' });
                if (string.IsNullOrEmpty(str1Sub[i]))
                    continue;
                if(!strItems.Contains(str1Sub[i]))
                {
                    strItems.Add(str1Sub[i]);
                }
            }  
            List<int> indexCollect = new List<int>();
            for (int i = 0; i < strItems.Count; i++)
            {
                int temp = str2Temp.IndexOf(strItems[i]);
                while(temp!=-1)
                {
                    indexCollect.Add(temp);
                    if(temp+1<str2Temp.Length)
                    {
                        temp= str2Temp.IndexOf(strItems[i],temp+1);
                    }else
                    {
                        break;
                    }
                    
                }
                indexCollect.Add(-2);
            } 
            if(lastOne) this.richTextBox1.Text = str2;
            int j = 0;
            for (int i = 0; i < indexCollect.Count; i++)
            {
                if (indexCollect[i] != -1&&indexCollect[i]!=-2)
                {
                    a.Characters[indexCollect[i]+1, strItems[j].Length].Font.Color = color;
                    if (lastOne)
                    {
                        this.richTextBox1.Select(indexCollect[i], strItems[j].Length);
                        richTextBox1.SelectionColor = Color.Red;
                        richTextBox1.SelectionBackColor = Color.White;
                        richTextBox1.HideSelection = false;
                    }
                }
                if (indexCollect[i] == -2)
                    j++;
                
            }
        }
        private void lbl_Close_MouseHover(object sender, EventArgs e)
        {
            this.lbl_Close.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void lbl_Close_MouseDown(object sender, MouseEventArgs e)
        {
            this.lbl_Close.BackColor = System.Drawing.SystemColors.ControlDarkDark;
        }

        private void lbl_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbl_Close_MouseLeave(object sender, EventArgs e)
        {
            this.lbl_Close.BackColor = System.Drawing.SystemColors.ActiveBorder;
        }
        //定义无边框窗体Form
        [DllImport("user32.dll")]//*********************拖动无窗体的控件
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int _HTCAPTION = 0x0002;
        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + _HTCAPTION, 0);

        }
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        // Implement the drag of the form
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
            {
                message.Result = (IntPtr)HTCAPTION;
            }
        }


        private void lbl_1_same_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            this.lbl_1_same.BackColor = this.colorDialog1.Color;
        }

        private void lbl_1_diff_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            this.lbl_1_diff.BackColor = this.colorDialog1.Color;
        }

        private void lbl_2_same_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            this.lbl_2_same.BackColor = this.colorDialog1.Color;
        }

        private void lbl_2_diff_Click(object sender, EventArgs e)
        {
            this.colorDialog1.ShowDialog();
            this.lbl_2_diff.BackColor = this.colorDialog1.Color;
        }

        private void label14_DoubleClick(object sender, EventArgs e)
        {
            Color temp = this.lbl_1_diff.BackColor;
            this.lbl_1_diff.BackColor = this.lbl_2_diff.BackColor;
            this.lbl_2_diff.BackColor = temp;
        }

        private void label1_DoubleClick(object sender, EventArgs e)
        {
            Color temp = this.lbl_1_diff.BackColor;
            this.lbl_1_diff.BackColor = this.lbl_1_same.BackColor;
            this.lbl_1_same.BackColor = temp;
        }

        private void label12_DoubleClick(object sender, EventArgs e)
        {
            Color temp = this.lbl_2_diff.BackColor;
            this.lbl_2_diff.BackColor = this.lbl_2_same.BackColor;
            this.lbl_2_same.BackColor = temp;
        }
    }
    public class CustomProgressBar : ProgressBar
    {
        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            SolidBrush brush = null;
            System.Drawing.Rectangle rec = new System.Drawing.Rectangle(0, 0, this.Width, this.Height);
            if (ProgressBarRenderer.IsSupported)
            {
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec);
            }
            Pen pen = new Pen(this.ForeColor, 1);
            e.Graphics.DrawRectangle(pen, rec);
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, rec.Width, rec.Height);
            rec.Width = (int)(rec.Width * ((double)Value / Maximum));
            brush = new SolidBrush(this.ForeColor);
            e.Graphics.FillRectangle(brush, 0, 0, rec.Width, rec.Height);
        }
    }
}
