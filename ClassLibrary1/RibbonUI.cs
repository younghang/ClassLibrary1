using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Drawing;
using YHExcelAddin.Calculator.UIController;
using WindowsFormsApp1;

namespace YHExcelAddin
{
    [ComVisible(true)]
    public class RibbonUI : ExcelRibbon 
    {
        private static IRibbonUI customRibbon;
        private static Range selectRange = null;
        
        public static void  SelectionChanged(Range range)
        {
            if (RibbonUI.dyeForm != null && !RibbonUI.dyeForm.IsDisposed)
            {
                RibbonUI.dyeForm.SetSelectedRange(range);
            }
            selectRange = range;            
        }
        public static Range GetSelectRange()
        {
            return selectRange;
        }
        public override string GetCustomUI(string RibbonID)
        {
 
            string ribbonxml = string.Empty; 
            try
            {
                ribbonxml =  GetResourceText("CustomUI14.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return ribbonxml;
        }
        //获取资源文本文件，文件要在属性-生成操作-嵌入资源
        internal static string GetResourceText(string resourceName)
        {
            string text = string.Empty;

            Assembly assm = Assembly.GetExecutingAssembly();
            string[] resources = assm.GetManifestResourceNames();    //获取全部资源

            foreach (string resource in resources)
            {
                if (resource.EndsWith(resourceName))
                {
                    System.IO.Stream streamText = assm.GetManifestResourceStream(resource);
                    StreamReader reader = new StreamReader(streamText);
                    text = reader.ReadToEnd();
                    reader.Close();
                    streamText.Close();
                    break;
                }
            }
            return text;
        }
        public static AddInForm addinForm;
        public static DyeCellForm dyeForm;
        public static CalculatorForm clcForm;
        public static PlotFormSetup plotFormSetup;
        public static void ShowAddInForm(string tag)
        {
            if (addinForm==null || addinForm.IsDisposed)
            {
                addinForm = new AddInForm();
            } 
            addinForm.SetTag(tag);
            addinForm.Show();
            addinForm.Focus();
        }
        public void button_Click(IRibbonControl control)
        { 
            switch (control.Id)
            {
                case "GetMaxLineButton":
                    MessageBox.Show("当前文档最大行数 "+MyExcelFunctions.GetMaxLineMacro());
                    break;
                case "fivechess":
                    MainForm main = new MainForm();
                    main.Show();break;
                case "btn_xll":
                    ShowAddInForm("xll");
                    break;
                case "btn_com1":
                    ShowAddInForm("com1");
                    break;
                case "btn_com2":
                    ShowAddInForm("com2");
                    break;
                case "dyecolor":                  
                    if (dyeForm == null || dyeForm.IsDisposed)
                    {
                        dyeForm = new DyeCellForm();
                    }
                    dyeForm.Show();
                    dyeForm.Focus();
                    break;
                case "markSame":
                    MarkSame();break;
                case "FillEmpty":
                    FillEmpty(); break;
                case "Calculator":
                    if (clcForm == null || clcForm.IsDisposed||clcForm.IsRichBoxDisposed())
                    {
                        clcForm = new CalculatorForm();                       
                    }
                    clcForm.Show();
                    clcForm.Focus(); 
                    clcForm.Activate();
                    clcForm.WindowState = FormWindowState.Normal;
                    break;
                case "DrawPlatoGraph":
                    ShowPlato();break;
                case "ShowDanmu":
                    ShowDanmu();break;
                default:
                    MessageBox.Show("Hello:" + control.Id);
                    break;
            }
           
        }
        private void ShowDanmu()
        {
            DanmukuTest plot = new DanmukuTest();
            plot.Show();
        }
        private void ShowPlato()
        {
            PlotMainWindow.application = ExcelDnaUtil.Application as Microsoft.Office.Interop.Excel.Application;
            PlotMainWindow plotPlato = new PlotMainWindow();           
            plotPlato.GetRange += RibbonUI.GetSelectRange;
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(plotPlato);
            //plotPlato.Show();
            PlotFormSetup.LoadWindow(plotPlato);

        }


        public void MarkSame()
        {
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            Range range = app.Selection;
            int length = range.Count;
            int line = app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            length = Math.Min(line, length);
            List<string> listCells = new List<string>();
            if (range.Areas.Count > 1)
            {
                return;
            }
            for (int i = 0; i < length; i++)
            { 
                Range item= range.Cells[i + 1] ;
                item.Interior.Pattern = XlPattern.xlPatternNone;
                if (item.FormulaR1C1Local == "")
                {
                    continue;
                }
                if(listCells.Contains(item.FormulaR1C1Local))
                {
                    item.Interior.Color= Color.FromArgb(255, 192, 0);
                }
                else
                {
                    listCells.Add(item.FormulaR1C1Local);
                } 
            }
        }
        public void FillEmpty()
        {
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            Range range = app.Selection;    
            if (range.Areas.Count != 2)
            {
                return;
            }
            Range fillRange = range.Areas[2];
            if(fillRange.Count!=1)
            {
                return;
            }
            string fillText = fillRange.FormulaR1C1Local;
            Range regionRange = range.Areas[1];
            int length = regionRange.Count;
            int line = app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            int cols= app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Column;
            length = Math.Min(line*cols, length);
            for (int i = 0; i < length; i++)
            {
                Range item = range.Cells[i + 1];               
                if (item.FormulaR1C1Local == "")
                {
                    item.Value2= fillText;
                }              
            }
        }

        public void ribbonLoad(IRibbonUI ribbon)
         {
            customRibbon = ribbon;
            //customRibbon.ActivateTab(ControlID: "CustomTab");
         }
        public void GetGalleryItem(IRibbonControl control )
        {
            int n = 1;
            dynamic app = ExcelDnaUtil.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;
            string name = "";
            foreach ( Workbook workbook in app.Workbooks)
            {
                //RibbonGallery gallery = this.Factory.CreateRibbonGallery();
                name += workbook.Name + " * ";              
                n++;
            }

            MessageBox.Show(name);
            
        }


        public void OnShowCTP(IRibbonControl control)
        {
            string hwnd = ((dynamic)ExcelDnaUtil.Application).ActiveWindow.Hwnd.ToString();
            CTPManager.Instance.ShowCTP(hwnd);
        }

        public void OnDeleteCTP(IRibbonControl control)
        {
            string hwnd = ((dynamic)ExcelDnaUtil.Application).ActiveWindow.Hwnd.ToString();
            CTPManager.Instance.DeleteCTP(hwnd); 
        }


        public override object LoadImage(string imageId)
        {
            return GetResourceBitmap(imageId);
        }
        public void OnShowInfo(IRibbonControl control)
        {
            //MessageBox.Show("gongyanfang@atlbattery.com");
            new AboutForm().Show();
        }
        public static System.Drawing.Bitmap GetResourceBitmap(string resourceName)
        {
            System.Drawing.Bitmap image = null;

            //String projectName = Assembly.GetExecutingAssembly().GetName().Name.ToString();获取项目名
            string[] resources = Assembly.GetExecutingAssembly().GetManifestResourceNames();    //获取全部资源

            //office.png 属性-生成操作-嵌入的资源，然后才能在GetManifestResourceStream被找到。
            //asm.GetManifestResourceStream("项目命名空间.资源文件所在文件夹名.资源文件名"); GetManifestResourceStream("CSharpAddIn.Img.office.png");  

            string extension = System.IO.Path.GetExtension(resourceName).ToLower();             //扩展名

            foreach (string resource in resources)
            {
                if (resource.EndsWith(resourceName))
                {
                    System.IO.Stream streamImg = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
                    switch (extension)
                    {
                        //http://blogs.msdn.com/b/jensenh/archive/2006/11/27/ribbonx-image-faq.aspx
                        case ".ico":
                            image = new System.Drawing.Icon(streamImg).ToBitmap();
                            break;

                        case ".png":
                        case ".jpg":
                        case ".bmp":
                        default:
                            image = new System.Drawing.Bitmap(streamImg);
                            image.MakeTransparent();
                            break;
                    }
                    streamImg.Close();
                    break;
                }
            }
            return image;
        }
 
       
    }
}
