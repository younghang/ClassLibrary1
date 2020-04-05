/*
 * 由SharpDevelop创建。
 * 用户： yanghang
 * 日期: 2016/9/12
 * 时间: 10:05
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Drawing;

namespace YHExcelAddin
{
	/// <summary>
	/// Description of ChessScore.
	/// </summary>
	public class ChessStrategy
	{
		public ChessStrategy()
		{
			scoreTable = new int[15, 15];
			
		}
		
		
		private int[,] scoreTable;
		
		public Point GetStepPoint()
		{
			CalScoreTable();
			Point p=new Point(7,7);
			int max=0;
			int x=7;
			int y=7;
			for (int i = 0; i <MainForm.MaxLine; i++) {
				for (int j = 0; j < MainForm.MaxLine; j++) {
					if (scoreTable[i,j]>max) {
						max=scoreTable[i,j];
						x=i;
						y=j;
					}
				}
			}
			if (max==scoreTable[7,7]) {
				return p;
			}
			p.X=x;
			p.Y=y;
			return p;
		}
		public int[,]GetScoreTable()
		{
			CalScoreTable();
			return scoreTable;
		}
		void CalScoreTable()
		{
			ChessState=MainForm.GetPanelState();
			for (int i = 0; i < MainForm.MaxLine; i++) {
				for (int j = 0; j < MainForm.MaxLine; j++) {
					scoreTable[i,j]=GetPointScore(i,j);
					
				}
			}
		}
		char [,]ChessState;
		int GetPointScore(int i,int j)
		{
			
			if (ChessState[i,j]!='0') {
				return 0;
			}
			
			int horizontalScore=GetHorizotalScore(i,j);
			int vertiaclScore=GetVerticalScore(i,j);
			int leftAngelScore=GetLeftAngelScore(i,j);
			int rightAngelScore=GetRightAngleScore(i,j);
			return horizontalScore+vertiaclScore+leftAngelScore+rightAngelScore;
			
		}
		
		int GetRightAngleScore(int x, int y)
		{
			string state="";
			int sum=0;
			for (int i = 0; i < 5; i++) {
				int endX=x-i;
				int endY=y+i;
				int startX=endX+4;
				int startY=endY-4;
				
				if (endX<0||startX>=15||endY>=15||startY<0) {
					continue;
				}
				for (int j = 0; j <= 4; j++) {
					state += ChessState[startX-j,startY+j]+"";
					
				}
				sum+= GetTupleScore(state);
				state="";
			}
			return sum;
		}
		
		int GetLeftAngelScore(int x, int y)
		{
			string state="";
			int sum=0;
			for (int i = 0; i < 5; i++) {
				int endX=x+i;
				int endY=y+i;
				int startX=endX-4;
				int startY=endY-4;
				
				if (endX>=15||startX<0||endY>=15||startY<0) {
					continue;
				}
				for (int j = 0; j <= 4; j++) {
					state += ChessState[startX+j,startY+j]+"";
					
				}
				sum+= GetTupleScore(state);
				state="";
			}
			return sum;
		}
		
		int GetVerticalScore(int x, int y)
		{
			string state="";
			int sum=0;
			for (int i = 0; i < 5; i++) {
				int end=y+i;
				int start=end-4;
				if (end>=15||start<0) {
					continue;
				}
				for (int j = start; j <= end; j++) {
					state += ChessState[x,j]+"";
					
				}
				sum+= GetTupleScore(state);
				state="";
			}
			return sum;
		}
		
		int GetHorizotalScore(int x, int y)
		{
			string state="";
			int sum=0;
			for (int i = 0; i < 5; i++) {
				int end=x+i;
				int start=end-4;
				if (end>=15||start<0) {
					continue;
				}
				for (int j = start; j <= end; j++) {
					state += ChessState[j,y]+"";
					
				}
				sum+= GetTupleScore(state);
				state="";
			}
			return sum;
		}
		
		
//		 enum TupleType
		//        {
		//            // tuple is empty
		//            Blank,7
		//            // tuple contains a black chess
		//            B,35
		//            // tuple contains two black chesses
		//            BB,800
		//            // tuple contains three black chesses
		//            BBB,15000
		//            // tuple contains four black chesses
		//            BBBB,800000
		//            // tuple contains a white chess
		//            W,15
		//            // tuple contains two white chesses
		//            WW,400
		//            // tuple contains three white chesses
		//            WWW,1800
		//            // tuple contains four white chesses
		//            WWWW,100000
		//            // tuple does not exist
		//            Virtual,
		//            // tuple contains at least one black and at least one white
		//            Polluted

		//        };
		public int GetTupleScore(string str)
		{
			
			if (str.Contains("B")&&str.Contains("W")) {
				return 0;
			}
			if (!str.Contains("B")&&!str.Contains("W")) {
				return 7;
			}
			int BCount=FindStringCount(str,'B');
			int WCount=FindStringCount(str,'W');
			if (BCount==4) {
//				return 300000;
				return 800000;
			}
//			if (str=="0BBB0") {
//				return 100000;
//			}
			if (BCount==3) {
//				return 100000;//若比对面小(10000)就是守
				return 15000;
			}
			if (BCount==2) {
//				return 4000;
				return 800;
			}
			if (BCount==1) {
				
//				return 10;
				return 35;
			}
			if (WCount==4) {
//				return 200000;
				return 100000;
			}
			if (WCount==3) {
//				return 20000;
				return 1800;
			}
//			if (str.Contains("WWW")) {
//				return 6000;
//			}
			if (WCount==2) {
//				return 1000;
				return 400;
			}
			if (WCount==1) {
//				return 6;
				return 15;
			}
			return 0;
		}
		int FindStringCount(string str,char target)
		{
			int count=0;
			for (int i = 0; i < str.Length; i++) {
				if (str[i]==target) {
					count++;
				}
			}
			return count;
			
		}
		
	}
}
