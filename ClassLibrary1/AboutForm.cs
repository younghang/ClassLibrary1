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
            label1.Text = "此Addin添加了一个函数MatchDataSheet。可以匹配数据，但是得放到上一列,还可以标记颜色。" +
                "还有个函数GetMaximumContinusCount, 可以得到连续单元格的最大重复个数" +
                "另外还有两个功能是标记重复以及填充空白单元格。" +
                "还有切换工作簿的窗口，还有插件删除。" +
                "还有一个计算器。" +
                "和一个五子棋。";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
