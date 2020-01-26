using System.Runtime.InteropServices;
using ExcelDna.Integration;
using ExcelDna.Integration.CustomUI;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace YHExcelAddin
{
    [ComVisible(true)]
    public class Class1 : ExcelRibbon, IExcelAddIn
    {
        public IRibbonUI R;
        public override string GetCustomUI(string RibbonID)
        {
            string xml = @"<customUI xmlns='http://schemas.microsoft.com/office/2009/07/customui' onLoad='OnLoad'>
    <ribbon startFromScratch='false'>
        <tabs>
            <tab id='Tab1' label='RibbonXmlEditor'>
                <group id='Group1' label='Author:ryueifu'>
                    <button id='Button1' label='CTP' imageMso='C' onAction='Button1_Click'/>
                    <button id='Button2' label='UnLoad' imageMso='U' onAction='Button2_Click'/>
                </group>
            </tab>
        </tabs>
    </ribbon>
</customUI>";
            return xml;
        }
        public void OnLoad(IRibbonUI ribbon)
        {
            R = ribbon;
            R.ActivateTab(ControlID: "Tab1");
        }
        public void Button1_Click(IRibbonControl control)
        {
            Module1.ctp.Visible = !Module1.ctp.Visible;
        }
        public void Button2_Click(IRibbonControl control)
        {
            Excel.AddIn ThisAddin = (ExcelDnaUtil.Application as Excel.Application).AddIns["Excel_DNA_Template_CS"];
            ThisAddin.Installed = false;
        }
        void IExcelAddIn.AutoClose()
        {
            Module1.DisposeCTP();
        }

        void IExcelAddIn.AutoOpen()
        {
            Module1.CreateCTP();
        }
    }
    public static class Module1
    {
        public static UserControl uc;
        public static CustomTaskPane ctp;
        public static void CreateCTP()
        {
            uc = new UserControl();
            ctp = CustomTaskPaneFactory.CreateCustomTaskPane(userControl: uc, title: "CTP");
            ctp.DockPosition = MsoCTPDockPosition.msoCTPDockPositionRight;
            ctp.Visible = true;
        }
        public static void DisposeCTP()
        {
            ctp.Delete();
            ctp = null;
        }
    }
}
