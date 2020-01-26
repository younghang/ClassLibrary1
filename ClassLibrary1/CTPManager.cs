using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExcelDna.Integration.CustomUI;

namespace YHExcelAddin
{
    internal  class CTPManager:IDisposable
    {
        static CTPManager()
        {
            disposed = false;
            Instance = new CTPManager();
        }
        ~CTPManager()
        {
            Dispose(false);
        }
        public static List<string> sheetNames = new List<string>();
        private static bool disposed = false;
        private Dictionary<string, CustomTaskPane> DicCustomCTP = new Dictionary<string, CustomTaskPane>();
        public static CTPManager Instance { get; private set; } = null;
        static CustomTaskPane ctp;
        static CTPControl uc;
        public void ShowCTP(string hwnd)
        {
            //Office 2013 is SDI(single document interface) https://www.add-in-express.com/creating-addins-blog/2013/02/28/excel2013-single-document-interface-task-panes/
            if(DicCustomCTP.ContainsKey(hwnd))
            {
                CustomTaskPane ctp = DicCustomCTP[hwnd];
                if (ctp != null) ctp.Visible = true;
            }else
            {
                uc = new CTPControl();
                ctp = CustomTaskPaneFactory.CreateCustomTaskPane(userControl: uc, "Workbooks");
                ctp.DockPosition = MsoCTPDockPosition.msoCTPDockPositionRight;
                ctp.DockPositionStateChange += ctp_DockPositionStateChange;
                ctp.VisibleStateChange += ctp_VisibleStateChange;
                ctp.Visible = true;
                DicCustomCTP.Add(hwnd, ctp);
            } 
        }
        public void UpdateCTP(List<string> names)
        {
            foreach (var i in DicCustomCTP.Keys)
            {
                CustomTaskPane ctp = DicCustomCTP[i];
                CTPControl uc = (CTPControl)ctp.ContentControl;
                uc.UpdateListView(names);
            }     
            
        }
        public void DeleteCTP(string hwnd)
        {
            if(DicCustomCTP.ContainsKey(hwnd))
            {
                CustomTaskPane ctp = DicCustomCTP[hwnd];
                ctp.DockPositionStateChange -= ctp_DockPositionStateChange;
                ctp.VisibleStateChange -= ctp_VisibleStateChange;
                ctp.Delete();
                ctp = null;
                DicCustomCTP.Remove(hwnd);
            }
            if (ctp != null)
            {
                // Could hide instead, by calling ctp.Visible = false;
                ctp.Delete();
                ctp = null;
            }
        }

        static void ctp_VisibleStateChange(CustomTaskPane CustomTaskPaneInst)
        {
            //MessageBox.Show("CTP visible: " + CustomTaskPaneInst.Visible);
        }
        static void ctp_DockPositionStateChange(CustomTaskPane CustomTaskPaneInst)
        {
            //((CTPControl)ctp.ContentControl).TheLabel.Text = "CTP DockPosition: " + CustomTaskPaneInst.DockPosition.ToString();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposed)
            {
                return;
            }
            if(disposing)
            {
                foreach(var i in DicCustomCTP.Keys)
                {
                    CustomTaskPane ctp = DicCustomCTP[i];
                    ctp.DockPositionStateChange -= ctp_DockPositionStateChange;
                    ctp.VisibleStateChange -= ctp_VisibleStateChange;
                    ctp.Delete();
                    ctp = null;
                    DicCustomCTP.Remove(i);
                }
            }
            disposed = true;
        }
    }
 

}
