using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
 

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        //[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        //[return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        //public static extern bool AllocConsole();

        //[System.Runtime.InteropServices.DllImport("Kernel32")]
        //public static extern void FreeConsole();
        private CustomProgressBar progressBar2;
        public Form1()
        {

            InitializeComponent();
            this.progressBar2 = new CustomProgressBar();
            this.progressBar2.Location = new System.Drawing.Point(25, 308);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(328, 10);
            this.progressBar2.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar2.ForeColor = System.Drawing.Color.LimeGreen;
            this.progressBar2.TabIndex = 70;
            this.progressBar2.Value = 90;
            this.Controls.Add(progressBar2);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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
        public static extern bool ReleaseCapture();
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



        private void btn_OK_Click(object sender, EventArgs e)
        {
            string seperateStr = this.textBox1.Text.Trim(new char[] { ' ', '\n' });
            char[] seperator = seperateStr.ToCharArray();
            MessageBox.Show(""+(this.lbl_1_diff.BackColor==this.lbl_1_same.BackColor));
            string str1 = "p9605, p98,P60,P30   \n";
            string str2= "p9505,P98, P61,P33 ,P30 \n";
            string str1Temp = str1.ToUpper();
            string str2Temp = str2.ToUpper();
            string[] str1Sub = str1Temp.Split(seperator);
            for (int i = 0; i < str1Sub.Length; i++)
            {
                str1Sub[i] = str1Sub[i].Trim(new char[] { ' ','\n'});
            }
            int[] indexCollect = new int[str1Sub.Length];
            for (int i=0;i<str1Sub.Length;i++)
            {
                indexCollect[i]=str2Temp.IndexOf(str1Sub[i]);
            }
            this.richTextBox1.Text = str2;
            for (int i = 0; i < str1Sub.Length; i++)
            {
                if(indexCollect[i]!=-1)
                {
                    this.richTextBox1.Select(indexCollect[i], str1Sub[i].Length);
                    richTextBox1.SelectionColor = Color.Red;
                    richTextBox1.SelectionBackColor = Color.White;
                    richTextBox1.HideSelection = false;
                }
               
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

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            
            return;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (clcForm == null || clcForm.IsDisposed || clcForm.IsRichBoxDisposed())
            {
                clcForm = new Form2();                
            }
            clcForm.Show();
            clcForm.Focus();
        }
        public static Form2 clcForm;

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
            rec.Width = (int)(rec.Width * ((double)Value / Maximum)) ;
            brush = new SolidBrush(this.ForeColor);
            e.Graphics.FillRectangle(brush, 0, 0, rec.Width, rec.Height);
        }
    }
}
