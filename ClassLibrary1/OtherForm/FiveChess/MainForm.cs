/*
 * 由SharpDevelop创建。
 * 用户： yanghang
 * 日期: 2016/9/9
 * 时间: 16:51
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using YHExcelAddin.Properties;

namespace YHExcelAddin
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		int padding = 30;
		public static int MaxLine=15;
		int MarginTop=50;
		int MarginLeft=50;
		
		int count=1;
		private Image WhitePiece;
		private Image BlackPiece;
		private Image Bg;
		private float PieceSize;
		
		private List<Point> WhitePoints=new List<Point>();
		private List<Point> BlackPoints=new List<Point>();
		private bool IsBlack=true;//设置先手，下棋顺序
		
		private bool IsGameOver=false;
		private bool IsWhiteWin=false;
		
		
		private bool NeedLastShow=false;
		
		static AutoResetEvent  autoEvent;
		private bool ComputerFirst=true;
		
		private ChessStrategy chessStategy=new ChessStrategy();
		MaxMinStategy mms=new MaxMinStategy();
		
		private static readonly string[] COLUMN_MARKS = new string[]
		{
			"A",
			"B",
			"C",
			"D",
			"E",
			"F",
			"G",
			"H",
			"I",
			"J",
			"K",
			"L",
			"M",
			"N",
			"O"
		};
		
		private static readonly Font FONT_NUM = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, 0);

		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			InitSource();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			
		}
		void InitSource()
		{
			//Assembly asm = Assembly.GetExecutingAssembly();
			//Stream smw = asm.GetManifestResourceStream("stone_w2.jpg");
			//Stream smb = asm.GetManifestResourceStream("stone_b1.jpg");
			WhitePiece=    Resources.stone_w2 ;
			BlackPiece=  Resources.stone_b1;
			//            Bg = Resource1.bg;
			PieceSize=3.0f*padding/4;
			autoEvent=new AutoResetEvent (false);
			//            this.BackgroundImage = Bg;
			
			
		}
		
		void MainFormPaint(object sender, PaintEventArgs e)
		{
			DrawPanel();
			
			if (NeedLastShow) {
				DrawPieces();
			}
			if (checkBox_SquareShow.Checked) {
				NeedLastShow=true;
			}
			else
				NeedLastShow=false;
			
		}
		bool SingleClick=true;
		
		void MainFormMouseDown(object sender, MouseEventArgs e)
		{
			
//			GetLeftAngelScore();
//			return;
			//点快了容易出问题
			if (!StartGame) {
				return;
			}
			if (!SingleClick) {
				return;
			}
			SingleClick=false;
//			if (IsBlack!=ComputerFirst) {
//				return;
//			}
//			if (IsBlack) {
//				return;
//			}
			if (ComputerFirst) {
				autoEvent.WaitOne();//Set了才能继续往下走
			}
			
			SetUserMouseDownPoint(e);
			
			
			
			
		}
		void SetUserMouseDownPoint(MouseEventArgs e)
		{
			int x=e.X;
			int y=e.Y;
			if (x<MarginLeft-PieceSize/2||y<MarginTop-PieceSize/2) {
				return;
			}
			Point p=GetValidPoint(x,y);
			
			if (!AddPointToList(p)) {
				autoEvent.Set();
				SingleClick=true;
				return;
			}
			if (!IsGameOver) {
				new Thread(new ThreadStart(WaitWork)).Start();
			}
			
			
			
		}
		void SetTitle(Point  p){
			
			string text="落子位置：("+COLUMN_MARKS[p.X]+" , "+(p.Y+1)+")   轮到"+(!IsBlack?"黑棋":"白棋");
			if (this.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!this.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (this.Disposing || this.IsDisposed)
						return;
				}
				this.Invoke(new Action<Point>((a)=>SetTitle(a)),new object[]{ p});
			}
			else
			{
				this.Text = text;
			}
			
		}
		bool AddPointToList(Point p)
		{
			if (WhitePoints.Contains(p)||BlackPoints.Contains(p)) {
				return false;
			}
			if (p.X>=15||p.Y>=15) {
				return false;
			}
			if (!IsBlack) {
				WhitePoints.Add(p);
			}
			else
			{
				BlackPoints.Add(p);
			}
			SetTitle(p);
//			Invalidate();
//			DrawPieces();
			if (LastPoint.X!=-1&&LastPoint.Y!=-1&&NeedLastShow) {
				ClearMark(LastPoint);
				
			}
			
			DrawPieces(p,IsBlack);
			LastPoint=p;
			IsBlack=!IsBlack;
			return true;
			
		}
		void GetLeftAngelScore( )
		{ 
			for (int i = 0; i < 11; i++) {
				for (int j = 0; j <15-i; j++) {
					 
					Point p=new Point(j,j+i);
					DrawPieces(p,true);
				}
				
			}
			for (int i = 1; i < 11; i++) {
				for (int j = 0; j <15-i; j++) {
					 
					Point p=new Point(j+i,j);
					DrawPieces(p,true);
				}
				
			}
			
		}
		void GetRightAngleScore( )
		{
			for (int i = 4; i < 15; i++) {
				for (int j = 0; j <=i; j++) {
					 
					Point p=new Point(i-j,j);
					DrawPieces(p,true);
				}
				
				
			}
			for (int i = 1; i < 11; i++) {
				for (int j = i; j < 15; j++) {
				  
					Point p=new Point(14+i-j,j);
					DrawPieces(p,true);
				}
			}
		}
		void DrawPanel()
		{
			
			Graphics g = CreateGraphics();
			Pen pen = new Pen(Color.Black);
			//画横线
			for (int i = 0; i <MaxLine; i++)            {
				//画线的方法，第一个参数为起始点X的坐标，第二个参数为起始
				//点Y的坐标；第三个参数为终点X的坐标，第四个参数为终
				//点Y的坐标；
				g.DrawLine(pen, MarginLeft, padding * i+MarginTop,MarginLeft+ (MaxLine-1)*padding, padding * i+MarginTop);
				g.DrawString(i+1+"",FONT_NUM,Brushes.Green,MarginLeft-padding, padding * i+MarginTop-10f/2);
				
			}
			//循环绘制多条竖线
			for (int i = 0; i <MaxLine; i++)
			{
				g.DrawLine(pen, MarginLeft+padding * i, MarginTop, MarginLeft+ padding * i,MarginTop+ (MaxLine-1)*padding);
				g.DrawString(COLUMN_MARKS[i],FONT_NUM,Brushes.Green, MarginLeft+ padding * i-10f/2, MarginTop-padding);
//				g.DrawString(i+"",FONT_NUM,Brushes.Green, MarginLeft+ padding * i-10f/2, MarginTop-padding);
			}
			
			g.Dispose();
			
			
		}
		void DrawPieces()
		{
			Graphics g = CreateGraphics();
			for (int i = 0; i < WhitePoints.Count; i++) {
				Point p=WhitePoints[i];
				g.DrawImage(WhitePiece,MarginLeft+p.X*padding-PieceSize/2,
				            MarginTop+p.Y*padding-PieceSize/2,PieceSize,PieceSize);
			}
			for (int i = 0; i < BlackPoints.Count; i++) {
				Point p=BlackPoints[i];
				g.DrawImage(BlackPiece,MarginLeft+p.X*padding-PieceSize/2,
				            MarginTop+p.Y*padding-PieceSize/2,PieceSize,PieceSize);
			}
			g.Dispose();
			
			
		}
		Rectangle restoreRect=new Rectangle(0,0,20,20);
		Rectangle InvalidRect=new Rectangle(0,0,20,20);
		Point LastPoint=new Point(-1,-1);
		void ClearMark(Point p)
		{
			if (InvalidRect!=new  Rectangle(0,0,20,20)) {
				Invalidate(InvalidRect,false);
			}
		}
		//draw point
		void DrawPieces(Point p,bool isBlack)
		{
			Thread.Sleep(100);
			Graphics g = CreateGraphics();
			
			RectangleF rectangle=new RectangleF(MarginLeft+p.X*padding-PieceSize/2,
			                                    MarginTop+p.Y*padding-PieceSize/2,PieceSize,PieceSize);
			int ReWidth=(int)PieceSize+3;
			restoreRect=new Rectangle(MarginLeft+p.X*padding-ReWidth/2,
			                          MarginTop+p.Y*padding-ReWidth/2,ReWidth,ReWidth);
			int ValidWidth=ReWidth+3;
			InvalidRect=new Rectangle(MarginLeft+p.X*padding-ValidWidth/2,
			                          MarginTop+p.Y*padding-ValidWidth/2,ValidWidth,ValidWidth);
			if (!isBlack) {
				g.DrawImage(WhitePiece,rectangle);
				SizeF textSize=g.MeasureString(count+"",FONT_NUM);
				g.DrawString(count+"",FONT_NUM,Brushes.Black,MarginLeft+p.X*padding-textSize.Width/2,
				             MarginTop+p.Y*padding-FONT_NUM.Height/2);
			}
			else
			{
				g.DrawImage(BlackPiece,rectangle);
				SizeF textSize=g.MeasureString(count+"",FONT_NUM);
				g.DrawString(count+"",FONT_NUM,Brushes.Red,MarginLeft+p.X*padding-textSize.Width/2,
				             MarginTop+p.Y*padding-FONT_NUM.Height/2);
			}
			count++;
			if (NeedLastShow) {
				g.DrawRectangle(new Pen(Color.Green,1),restoreRect);
			}			
			
			CheckGameOver();
			GeneratePanelState();
			if (checkBox_ShowScore.Checked) {
				if (checkBox_NoDll.Checked) {
					PrintChessState(chessStategy.GetScoreTable());
				}
				else{
					//LoadStategyDll.SetChessState(ChessState);
					//PrintChessState (LoadStategyDll.GetScoreTable());
				}
			}			
			
			g.Dispose();
			
			
		}
		void CheckGameOver()
		{
			if (WhitePoints.Count>5) {
				
			}
			bool whiteWin=CheckFiveInLine(WhitePoints);
			bool blackWin=CheckFiveInLine(BlackPoints);
			
			
			if (whiteWin||blackWin) {
				IsGameOver=true;
				IsWhiteWin=whiteWin;
			}
			if (IsGameOver) {
				string info=IsWhiteWin?"白棋胜利":"黑棋胜利";
				DialogResult result=MessageBox.Show(info,"重来");
				if (result==DialogResult.OK) {
					WhitePoints.Clear();
					BlackPoints.Clear();
					SetEnable();
					IsWhiteWin=false;
					count=1;
					StartGame=false;
					IsBlack=false;
					
					Invalidate();
				}else{
					if (result== DialogResult.Cancel) {
						Close();
					}
				}
			}
			
			
		}
		void SetEnable()
		{
			
			if (this.InvokeRequired)//如果调用控件的线程和创建创建控件的线程不是同一个则为True
			{
				while (!this.IsHandleCreated)
				{
					//解决窗体关闭时出现“访问已释放句柄“的异常
					if (this.Disposing || this.IsDisposed)
						return;
				}
				this.Invoke(new Action(()=>SetEnable()));
			}
			else
			{
				btn_StartGame.Enabled=true;//
				checkBox_YouFirst.Enabled=true;
			}
		}
		
		
		bool CheckFiveInLine(List<Point> points)
		{
			return CheckUtils.CheckFiveInline(points);
		}
		
		
		bool StartGame=false;
		
		
		void WaitWork()
		{
			Point p=new Point(-1,-1);
			Thread.Sleep(200);//
			while(true)
			{
				
				GeneratePanelState();
				if (checkBox_NoDll.Checked) {
					p=chessStategy.GetStepPoint();

					//mms.InitialChessState(ChessState);
					//p=mms.GetPoint();
				}
				else{
					mms.InitialChessState(ChessState);
					p = mms.GetPoint();

					//LoadStategyDll.SetChessState(ChessState);
					//p=LoadStategyDll.GetStepPoint();
				}
				
				if ( !WhitePoints.Contains(p)&&!BlackPoints.Contains(p)) {
					break;
				}
			}
			if(!AddPointToList(p))
			{
				return;
			}
//			DrawPieces(p,IsBlack);
//			IsBlack=!IsBlack;
			
			SingleClick=true;
			autoEvent.Set();
			
		}
		Point GenerateRandomPoint()
		{
			Random rm=new Random();
			int x=rm.Next(0,14);
			int y=rm.Next(0,14);
			return new Point(x,y);
		}
		
		Point GetValidPoint(int x,int y)
		{
			double px= (x-MarginLeft)*1.0/padding;
			double py=(y-MarginTop)*1.0/padding;
			
			return new Point(FourFive(px),FourFive(py));
		}
		int  FourFive(double x)
		{
			if ((x-(int)x)>0.5) {
				return (int)x+1;
			}
			else return (int)x;
		}
		void PrintChessState (char[,]ChessState,char except='0')
		{
			
			int leftPadding=MaxLine*padding+MarginLeft*2;
			int topPadding=MarginTop;
			Graphics g = CreateGraphics();
			for (int i = 0; i < ChessState.GetLength(0); i++) {
				for (int j = 0; j < ChessState.GetLength(0); j++) {
					if (ChessState[i,j]==except) {
						continue;
					}
					g.DrawString(ChessState[i,j]+"",FONT_NUM,Brushes.Pink,leftPadding+padding*i,topPadding+j*padding);
				}
				
			}
			
		}
		void PrintChessState (int[,]ChessState)
		{
			
			int leftPadding=MaxLine*padding+MarginLeft*2;
			int topPadding=MarginTop;
			Invalidate(new Rectangle(leftPadding,topPadding,MaxLine*padding,MaxLine*padding));
			Graphics g = CreateGraphics();
			for (int i = 0; i < ChessState.GetLength(0); i++) {
				for (int j = 0; j < ChessState.GetLength(0); j++) {
					g.DrawString(ChessState[i,j]+"",FONT_NUM,Brushes.White,leftPadding+padding*i,topPadding+j*padding);
				}
				
			}
			
			
			
		}
		static char [,] ChessState=new char[15,15];
		public static  char[,]GetPanelState()
		{
			
			return ChessState;
		}
		void GeneratePanelState()
		{
			
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					Point p=new Point(i,j);
					if (WhitePoints.Contains(p)) {
						if (ComputerFirst) {
							ChessState[i,j]='W';
						}
						else
							ChessState[i,j]='B';
						
					}
					else if (BlackPoints.Contains(p)) {
						if (ComputerFirst) {
							ChessState[i,j]='B';
						}
						else
						{
							ChessState[i,j]='W';
						}
						
					}
					else
						ChessState[i,j]='0';
				}
			}
			
			
		}
		void Btn_StartGameClick(object sender, EventArgs e)
		{
			if (checkBox_YouFirst.Checked) {
				ComputerFirst=false;
				
			}
			else
			{
				ComputerFirst=true;
			}
			checkBox_YouFirst.Enabled=false;
			if (ComputerFirst) {
				
				new Thread(new ThreadStart(WaitWork)).Start();
//				ComputerFirst=!ComputerFirst;
			}
			else
			{
				autoEvent.Set();
			}
			
			StartGame=true;
			IsGameOver=false;
			if (StartGame) {
				btn_StartGame.Enabled=false;
			}
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Bg = Resources.bg;
			this.BackgroundImage = Bg;
		}

		private void MainForm_Activated(object sender, EventArgs e)
		{
			//this.Invalidate();
		}
	}
}
