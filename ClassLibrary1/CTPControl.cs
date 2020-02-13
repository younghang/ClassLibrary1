using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelDna.Integration;
using Excel=Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using ExcelDna.Integration.CustomUI;

namespace YHExcelAddin
{
    [ComVisible(true)]
    public partial class CTPControl : UserControl
    {
        public Label TheLabel;
        public CTPControl()
        {
            InitializeComponent(); 
        }
 
        public void UpdateListView(List<string> names)
        { 
            listView1.Items.Clear();
            this.listView1.View = View.List;
            this.listView1.SmallImageList = this.imageList1;
            listView1.BeginUpdate();
            for (int i=0;i< names.Count;i++)
            {
                ListViewItem Item = new ListViewItem();
                Item.ImageIndex = 0;
                Item.Text = names[i];
                listView1.Items.Add(Item);
                 
            }
            listView1.EndUpdate();
        }
        internal void UpdateMessage(Dictionary<string, CustomTaskPane>.KeyCollection hwnds)
        {
            this.richTextBox1.Clear();
            foreach (string s in hwnds)
                this.richTextBox1.AppendText(s+'\n');
        }
        private void InitialListView()
        {
            this.listView1.View = View.List;
            this.listView1.SmallImageList = this.imageList1;
            dynamic app = ExcelDnaUtil.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;
            List<string> names = new List<string>();
            foreach (Excel.Workbook workbook in app.Workbooks)
            {  
               names.Add(workbook.Name); 
            }
            UpdateListView(names);


        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CTPControl_Load(object sender, EventArgs e)
        { 
            InitialListView();
        }
 

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
           

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    string name = listView1.SelectedItems[0].Text;
                    dynamic app = ExcelDnaUtil.Application;
                    Excel.Workbook workbook = app.Workbooks[name];
                    workbook.Activate();
                    app.ActiveWindow.WindowState = Excel.XlWindowState.xlMaximized;


                }
                catch (Exception)
                {
                    MessageBox.Show("error");
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
