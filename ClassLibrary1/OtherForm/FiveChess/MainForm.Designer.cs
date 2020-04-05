/*
 * 由SharpDevelop创建。
 * 用户： yanghang
 * 日期: 2016/9/9
 * 时间: 16:51
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using YHExcelAddin.Properties;

namespace YHExcelAddin
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.btn_StartGame = new System.Windows.Forms.Button();
            this.checkBox_ShowScore = new System.Windows.Forms.CheckBox();
            this.checkBox_SquareShow = new System.Windows.Forms.CheckBox();
            this.checkBox_YouFirst = new System.Windows.Forms.CheckBox();
            this.checkBox_NoDll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btn_StartGame
            // 
            this.btn_StartGame.BackColor = System.Drawing.Color.Transparent;
            this.btn_StartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_StartGame.Location = new System.Drawing.Point(553, 35);
            this.btn_StartGame.Name = "btn_StartGame";
            this.btn_StartGame.Size = new System.Drawing.Size(104, 27);
            this.btn_StartGame.TabIndex = 0;
            this.btn_StartGame.Text = "开始";
            this.btn_StartGame.UseVisualStyleBackColor = false;
            this.btn_StartGame.Click += new System.EventHandler(this.Btn_StartGameClick);
            // 
            // checkBox_ShowScore
            // 
            this.checkBox_ShowScore.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_ShowScore.Location = new System.Drawing.Point(553, 99);
            this.checkBox_ShowScore.Name = "checkBox_ShowScore";
            this.checkBox_ShowScore.Size = new System.Drawing.Size(104, 24);
            this.checkBox_ShowScore.TabIndex = 1;
            this.checkBox_ShowScore.Text = "显示程序视角";
            this.checkBox_ShowScore.UseVisualStyleBackColor = false;
            // 
            // checkBox_SquareShow
            // 
            this.checkBox_SquareShow.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_SquareShow.Location = new System.Drawing.Point(553, 129);
            this.checkBox_SquareShow.Name = "checkBox_SquareShow";
            this.checkBox_SquareShow.Size = new System.Drawing.Size(104, 24);
            this.checkBox_SquareShow.TabIndex = 2;
            this.checkBox_SquareShow.Text = "显示小正方形";
            this.checkBox_SquareShow.UseVisualStyleBackColor = false;
            // 
            // checkBox_YouFirst
            // 
            this.checkBox_YouFirst.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_YouFirst.Checked = true;
            this.checkBox_YouFirst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_YouFirst.Location = new System.Drawing.Point(553, 159);
            this.checkBox_YouFirst.Name = "checkBox_YouFirst";
            this.checkBox_YouFirst.Size = new System.Drawing.Size(104, 24);
            this.checkBox_YouFirst.TabIndex = 3;
            this.checkBox_YouFirst.Text = "你先";
            this.checkBox_YouFirst.UseVisualStyleBackColor = false;
            // 
            // checkBox_NoDll
            // 
            this.checkBox_NoDll.BackColor = System.Drawing.Color.Transparent;
            this.checkBox_NoDll.Checked = true;
            this.checkBox_NoDll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_NoDll.Location = new System.Drawing.Point(553, 189);
            this.checkBox_NoDll.Name = "checkBox_NoDll";
            this.checkBox_NoDll.Size = new System.Drawing.Size(104, 24);
            this.checkBox_NoDll.TabIndex = 4;
            this.checkBox_NoDll.Text = "不用Dll";
            this.checkBox_NoDll.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(684, 522);
            this.Controls.Add(this.checkBox_NoDll);
            this.Controls.Add(this.checkBox_YouFirst);
            this.Controls.Add(this.checkBox_SquareShow);
            this.Controls.Add(this.checkBox_ShowScore);
            this.Controls.Add(this.btn_StartGame);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "MainForm";
            this.Text = "FiveChess";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainFormPaint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFormMouseDown);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.CheckBox checkBox_NoDll;
		private System.Windows.Forms.CheckBox checkBox_YouFirst;
		private System.Windows.Forms.CheckBox checkBox_SquareShow;
		private System.Windows.Forms.CheckBox checkBox_ShowScore;
		private System.Windows.Forms.Button btn_StartGame;
	}
}
