namespace YHExcelAddin
{
    partial class PlotFormSetup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartExcelPlot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartExcelPlot
            // 
            this.btnStartExcelPlot.BackColor = System.Drawing.Color.Transparent;
            this.btnStartExcelPlot.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartExcelPlot.Location = new System.Drawing.Point(0, 0);
            this.btnStartExcelPlot.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartExcelPlot.Name = "btnStartExcelPlot";
            this.btnStartExcelPlot.Size = new System.Drawing.Size(80, 60);
            this.btnStartExcelPlot.TabIndex = 0;
            this.btnStartExcelPlot.Text = "Excel绘图启动中";
            this.btnStartExcelPlot.UseVisualStyleBackColor = false;
            this.btnStartExcelPlot.Click += new System.EventHandler(this.btnStartExcelPlot_Click);
            // 
            // PlotFormSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.Controls.Add(this.btnStartExcelPlot);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PlotFormSetup";
            this.Opacity = 0.6D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlotFormSetup";
            this.TransparencyKey = System.Drawing.SystemColors.ActiveBorder;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartExcelPlot;
    }
}