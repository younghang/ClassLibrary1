using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YHExcelAddin
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            label1.Text = "此Addin添加了一个函数MatchDataSheet。可以匹配数据，但是得放到上一列。另外还有两个功能是查看当前工作表的列数。还有切换工作簿的窗口，还有插件卸载。";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
