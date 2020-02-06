using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelDna.Integration;
using ExcelDna.IntelliSense;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using Microsoft.Office.Core;
using ExcelDna.Registration;

namespace YHExcelAddin
{
    public class AddIn : IExcelAddIn
    {
        public static bool IsFirstRun = true;
        public static Excel.Application ExcelApp;


        public void AutoOpen()
        {
            if (AddIn.IsFirstRun == true)
            {
                IntelliSenseServer.Install();
                AddIn.IsFirstRun = false;  
                
            }

            ExcelApp = ExcelDnaUtil.Application as Excel.Application;
            //ExcelApp.SheetActivate += ExcelApp_SheetActivate;
            ExcelApp.SheetSelectionChange += ExcelApp_SheetSelectionChange;

            ExcelRegistration.GetExcelFunctions()//在此处显示注册函数
                .Where(func => func.FunctionAttribute.Name.StartsWith("Params"))
                .ProcessParamsRegistrations()
                .RegisterFunctions();
            //ComAddInConnection com_addin = new ComAddInConnection();
            //ExcelComAddInHelper.LoadComAddIn(com_addin);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="Sh"></param>
        /// <param name="Target"></param>
        private void ExcelApp_SheetSelectionChange(object Sh, Excel.Range Target)
        {

            Excel.Worksheet sht = Sh as Excel.Worksheet;
            Excel.Workbook wkb = sht.Parent;
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            try
            {
                List<string> names = new List<string>();
                foreach (Excel.Workbook workbook in app.Workbooks)
                {
                    names.Add(workbook.Name);
                }
                CTPManager.Instance.UpdateCTP(names);

            }
            catch (Exception)
            {
                return;
            }
            if (RibbonUI.dyeFrom != null&&!RibbonUI.dyeFrom.IsDisposed)
            {
                RibbonUI.dyeFrom.SetSelectedRange(Target);                  
            }

        }

        private void ExcelApp_SheetActivate(object Sh)
        {
            Excel.Worksheet sht = Sh as Excel.Worksheet;
            Excel.Workbook wkb = sht.Parent;
            Excel.Application app = wkb.Parent; 
            try
            {
                List<string> names = new List<string>();
                foreach (Excel.Workbook workbook in app.Workbooks)
                {
                    names.Add(workbook.Name);
                }
                CTPManager.Instance.UpdateCTP(names);
    
            }
            catch (Exception)
            {
                return;
            }

        }

        public void AutoClose()
        {
            IntelliSenseServer.Uninstall();
        }

    }
    //public class AddinVoid : AddIn
    //{
    //    public static void GoToCataLogSht()
    //    {
    //        AddIn.ExcelApp.ActiveWorkbook.Worksheets["工作表目录"].Activate();
    //    }


    //    public static void GotoTopWindow(string targetAddress)
    //    {
    //        ExcelApp.EnableEvents = false;
    //        ExcelApp.Goto(AddIn.ExcelApp.ActiveCell, true);
    //        ExcelApp.EnableEvents = true;
    //    }


    //}
}
