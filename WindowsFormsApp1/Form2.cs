using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YHExcelAddin.Calculator.UIController;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public bool IsRichBoxDisposed()
        {
            return this.richTextBox1.Disposing;
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
            //richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret(); 
            richTextBox1.HideSelection = false;
        }
        UIController con;
        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            UIController.ShowOutput += AppendText;
            con = new UIController();            
            con.GetInput += GetLastLineOfRichTextBox;
            con.GetDataToInput();
        }

        private void AppendText(string info,string type="")
        {
            int len=richTextBox1.Text.Length;
            this.richTextBox1.Text+=info;
            richTextBox1.Select(len, info.Length);
            switch(type)
            {
                case ">>":
                    richTextBox1.SelectionFont = new Font("Consolas", 12,FontStyle.Bold);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                    break;
                default:
                    richTextBox1.SelectionFont = new Font("Consolas", 12);
                    this.richTextBox1.SelectionColor = Color.FromArgb(0, 0, 0);
                    break;
            }
            richTextBox1.HideSelection = false;
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
        }
        private string LastLine = "";
        private string GetLastLineOfRichTextBox()
        {
            return LastLine;
        }
          
        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                string[] lines=this.richTextBox1.Text.Split('\n');
                LastLine= lines[lines.Length - 2];
                LastLine = LastLine.Substring(2);
                e.Handled = true;
                con.Run();
                con.GetDataToInput();
            }
            if(e.KeyCode==Keys.Escape )
            {
                AppendText(LastLine, ">>");
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Tab)
            {
                string[] lines = this.richTextBox1.Text.Split('\n');
                if (lines.Length < 2)
                    return;
                string result = lines[lines.Length - 2];
                try
                {
                    double.Parse(result);
                    AppendText(result, ">>");
                    e.Handled = true;
                }
                catch(Exception)
                {
                    return;
                }
                
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.richTextBox1.Dispose();
        }
    }
}
