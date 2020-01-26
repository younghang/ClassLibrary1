using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExcelDna.Integration;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;

namespace YHExcelAddin
{
    public  partial class AddInForm : Form
    {
        public AddInForm()
        {
            InitializeComponent();
        }
        static string TAG = "";
        public void SetTag(string tag)
        {
            TAG = tag;
            LoadList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(TAG== "xll")
            {
                string s = listBox1.SelectedItem.ToString();
                Console.WriteLine(s);
                MyExcelFunctions.RemoveXllMacro(s);
            }else if(TAG=="com1"||TAG=="com2")
            {
                string s = listBox1.SelectedItem.ToString();
                Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
                dynamic list;
                if(TAG=="com1")
                {
                    list = app.AddIns;
                }else
                {
                    list = app.AddIns2;
                }
                foreach (dynamic j in list)
                {
                    if (j.FullName==s)
                    {
                        File.Delete(j.FullName);
                    }
                }
            }
            LoadList();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void AddInFrom_Load(object sender, EventArgs e)
        {

             

        }
        private void LoadList( )
        {
            listBox1.Items.Clear();
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;
            if (TAG=="xll")
            {
                dynamic RegisteredFunctions;         
                RegisteredFunctions = app.RegisteredFunctions;
                if (RegisteredFunctions==null)
                {
                    return;
                }
                for (int i = RegisteredFunctions.GetLowerBound(0); i <= RegisteredFunctions.GetUpperBound(0); i++)
                {
                    if (!this.listBox1.Items.Contains(RegisteredFunctions[i, RegisteredFunctions.GetLowerBound(1)].ToString()))
                    {
                        listBox1.Items.Add(RegisteredFunctions[i, RegisteredFunctions.GetLowerBound(1)].ToString());
                    }
                } 
            }
            else if(TAG=="com2")
            { 
                foreach (dynamic j in app.AddIns2)
                {
                    if (!this.listBox1.Items.Contains(j.FullName))
                    {
                        listBox1.Items.Add(j.FullName);
                    }
                }
            }
            else if (TAG == "com1")
            {
                foreach (dynamic k in app.AddIns)
                {
                    if (!this.listBox1.Items.Contains(k.FullName))
                    {
                        listBox1.Items.Add(k.FullName);
                    }
                }
            }
        }
    }
}
