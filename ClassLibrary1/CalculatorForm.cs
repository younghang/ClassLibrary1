using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YHExcelAddin.Calculator.UIController;

namespace YHExcelAddin
{
    public partial class CalculatorForm : Form
    {
        public CalculatorForm()
        {
            InitializeComponent();
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
            this.richTextBox1.AppendText(info);
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
                if (lines.Length < 2)
                    return;                
                LastLine= lines[lines.Length - 2];
                if (LastLine.Length < 2)
                    return;
                LastLine = LastLine.Substring(2);
                e.Handled = true;
                con.Run();
                con.GetDataToInput();
            }
            if(e.KeyCode==Keys.ControlKey)
            {
                AppendText(LastLine, ">>");
            }

        }

        private void CalculatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}
