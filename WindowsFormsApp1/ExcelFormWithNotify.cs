using BilibiliStyleDanmu;
using System;
using System.IO;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
namespace WindowsFormsApp1
{
    public partial class ExcelFormWithNotify : Form
    {
        public ExcelFormWithNotify()
        {
            InitializeComponent();
        }
        MainOverlay biliDanmu = null;
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        public static string fileName = "E:/task.xlsx";
        public static string filePath = "E:/";
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Filter = "*.xl*|*.*";
            openFileDialog1.ShowDialog();
            fileName= openFileDialog1.FileName;
            if (biliDanmu != null&&fileName!="" )
            {
                biliDanmu.AddDMText("文件", fileName,"g");
            }
            if(fileName.EndsWith("xls")||fileName.EndsWith("xlsx") || fileName.EndsWith("xlsm"))
            {
                //创建
                Excel.Application xlApp = new Excel.Application();
                xlApp.DisplayAlerts = false;
                xlApp.Visible = false;
                xlApp.ScreenUpdating = false;
                //打开Excel
                Excel.Workbook xlsWorkBook = xlApp.Workbooks.Open(fileName, System.Type.Missing, System.Type.Missing, System.Type.Missing,
                System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing,
                System.Type.Missing, System.Type.Missing, System.Type.Missing, System.Type.Missing);

                //处理数据过程，更多操作方法自行百度
                Excel.Worksheet sheet = xlsWorkBook.Worksheets[1];//工作薄从1开始，不是0
                MessageBox.Show(sheet.Cells[1, 1].FormulaR1C1Local) ;

                //另存
                //xlsWorkBook.SaveAs(exportExcelPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                //    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                //关闭Excel进程
                ClosePro(xlApp, xlsWorkBook);
            }

        }
        public void ClosePro(Excel.Application xlApp, Excel.Workbook xlsWorkBook)
        {
            if (xlsWorkBook != null)
                xlsWorkBook.Close(true, Type.Missing, Type.Missing);
            xlApp.Quit();
            // 安全回收进程
            System.GC.GetGeneration(xlApp);
            IntPtr t = new IntPtr(xlApp.Hwnd);   //获取句柄
            int k = 0;
            GetWindowThreadProcessId(t, out k);   //获取进程唯一标志
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);
            p.Kill();     //关闭进程
        }
        private void ExcelFormWithNotify_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (biliDanmu != null)
            {
                biliDanmu.Close();
            }
        }

        private void ExcelFormWithNotify_Load(object sender, EventArgs e)
        {
            //PlotPlatoWindow plotPlato = new PlotPlatoWindow();
            //plotPlato.Show();

            return;
            if (biliDanmu==null)
            {
                biliDanmu = new MainOverlay();
                biliDanmu.OpenOverlay();
                biliDanmu.Show();
            }
           
        }

        private void btn_FileFolderChoose(object sender, EventArgs e)
        {
            string path = "E:/";
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog.SelectedPath;
            }
            if (!string.IsNullOrEmpty(path))
            {
                filePath = path;
                //lbl_Path.Text = filePath;
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            PlotPlatoWindow plotPlato = new PlotPlatoWindow();
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(plotPlato);
            plotPlato.Show();
        }
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
            result = FSimpson(x => 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(-x * x / 2), 1000, end, -6);
            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           MessageBox.Show( GetGaussianCumulativeDistributionFunction(0).ToString());
        }
    }
}
