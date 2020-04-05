/*
 * 由SharpDevelop创建。
 * 用户： yanghang
 * 日期: 2016/10/2
 * 时间: 22:03
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace YHExcelAddin
{
	/// <summary>
	/// Description of MaxMinStategy.
	/// </summary>
	public class MaxMinStategy
	{
		public MaxMinStategy()
		{
		}
		
		#region 局面评分
		int GetEvaluateTableScore(char[,]chess)
		{
			int horizontalScore=GetHorizotalScore(chess);
			int vertiaclScore=GetVerticalScore(chess);
			int leftAngelScore=GetLeftAngelScore(chess);
			int rightAngelScore=GetRightAngleScore(chess);
			return horizontalScore+vertiaclScore+leftAngelScore+rightAngelScore;
			
		}
		
		int GetRightAngleScore(char[,] chess)
		{
			
			int sum=0;
			string str="";
			for (int i = 4; i < 15; i++) {
				for (int j = 0; j <=i; j++) {
					str+=chess[i-j,j];
				}
				sum+=GetStringScore(str);
				str="";
			}
			for (int i = 1; i < 11; i++) {
				for (int j = i; j < 15; j++) {
					str+=chess[14+i-j,j];
				}
				sum+=GetStringScore(str);
				str="";
			}
			return sum;
		}
		
		int GetLeftAngelScore(char[,] chess)
		{
			int sum=0;
			string str="";
			for (int i = 0; i < 11; i++) {
				for (int j = 0; j <15-i; j++) {
					str+=chess[j,j+i];
				}
				sum+=GetStringScore(str);
				str="";
			}
			for (int i = 1; i < 11; i++) {
				for (int j = 0; j <15-i; j++) {
					str+=chess[j+i,j];
				}
				sum+=GetStringScore(str);
				str="";
			}
			return sum;
		}
		
		int GetVerticalScore(char[,] chess)
		{
			int sum=0;
			string str="";
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					str+=chess[j,i];
				}
				sum+=GetStringScore(str);
				str="";
			}
			return sum;
		}
		
		int GetHorizotalScore(char[,] chess)
		{
			int sum=0;
			string str="";
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					str+=chess[i,j];
				}
				sum+=GetStringScore(str);
				str="";
			}
			return sum;
		}
		int GetStringScore(string str)
		{
			int total=0;
			if (str.Contains("BBBBB")) {
				total+=100000;
			}
			if (str.Contains("0BBBB0")) {
				total+= 10000;
			}
			
			if (str.Contains("00BBB0")) {
				total+=1000;
			}
			if (str.Contains("0BBB00")) {
				total+=1000;
			}
			
			if (str.Contains("00BB00")) {
				total+=100;
			}
			if (str.Contains("00B00")) {
				total+=10;
			}
			if (str.Contains("0BBBBW")) {
				total+=1000;
			}
			if (str.Contains("WBBBB0")) {
				total+=1000;
			}
			if (str.Contains("0BBBW")) {
				total+=100;
			}
			if (str.Contains("WBBB0")) {
				total+=100;
			}
			if (str.Contains("WBB0")) {
				total+=10;
			}
			if (str.Contains("0BBW")) {
				total+=10;
			}
			
			if (str.Contains("WWWWW")) {
				total-=100000;
			}
			if (str.Contains("0WWWW0")) {
				total-= 10000;
			}
			
			if (str.Contains("00WWW0")) {
				total-=1000;
			}
			if (str.Contains("0WWW00")) {
				total-=1000;
			}
			
			if (str.Contains("00WW00")) {
				total-=100;
			}
			if (str.Contains("00W00")) {
				total-=10;
			}
			if (str.Contains("0WWWWB")) {
				total-=1000;
			}
			if (str.Contains("BWWWW0")) {
				total-=1000;
			}
			if (str.Contains("0WWWB")) {
				total-=100;
			}
			if (str.Contains("BWWW0")) {
				total-=100;
			}
			if (str.Contains("BWW0")) {
				total-=10;
			}
			if (str.Contains("0WWB")) {
				total-=10;
			}
			
			return total;
		}
		#endregion
		
		
		
		char [,]chessState=new char[15,15];
		
		
		List<Point> GenerateValidPositions(char[,]chess)
		{
			List<Point> points=new List<Point>();
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					if (chess[i,j]!='0') {
						continue;
					}
					Point p=new Point(i,j);
					if (CheckValidPoint(p,chess)) {
						if (!points.Contains(p)) {
							points.Add(p);
						}
					}
				}
			}
			return points;
		}
		bool CheckValidPoint(Point p,char[,]chess)
		{
			int startX=p.X-1;
			int startY=p.Y-1;
			bool re=false;
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					int tempx=startX+i;
					int tempy=startY+j;
					if (tempx>14||tempx<0||tempy>14||tempy<0) {
						continue;
					}
					if (chess[tempx,tempy]!='0') {
						re=true;
						break;
					}
				}
			}
			return re;
			
		}
		
		int MAX_DEPTH=4;
		
		
		public void InitialChessState(char[,]temp)
		{
			for (int i = 0; i <15; i++) {
				for (int j = 0; j <15; j++) {
					chessState[i,j]=temp[i,j];
				}
			}
		}
		int count=0;
		public Point GetPoint()
		{
			count++;
			if (count==2) {
				
			}
			List<Point> avaliblePoints =GenerateValidPositions(chessState);
			int s=0;
			Point p=new Point(7,7);
			for (int i = 0; i < avaliblePoints.Count; i++) {
				int score=Max(MAX_DEPTH,avaliblePoints[i],chessState,1000000000);
				if (score>s) {
					s=score;
					p=avaliblePoints[i];
				}
			}
			return p;
		}
		
		int Max(int depth,Point p,char[,]tempChess,int min_beta )
		{
			int max=0;
			if (depth<=0) {
				return GetEvaluateTableScore(tempChess);
			}
			tempChess[p.X,p.Y]='B';
			int temp=0;
			List<Point> points=GenerateValidPositions(tempChess);
			for (int i = 0; i < points.Count; i++) {
				temp=Min(depth-1,points[i],tempChess,max );
				
				if (temp>max) {
					max=temp;
				}
				if (temp>min_beta) {
					tempChess[p.X,p.Y]='0';
					return min_beta;
					break;
				}
			}
				tempChess[p.X,p.Y]='0';
			return max;
		}
		
		int Min(int depth,Point p,char[,]tempChess  ,int max_alpha)
		{
			int min=1000000000;
			if (depth<=0) {
				return GetEvaluateTableScore(tempChess);
			}
			tempChess[p.X,p.Y]='W';
			
			int temp=0;
			List<Point> points=GenerateValidPositions(tempChess);
			for (int i = 0; i < points.Count; i++) {
				temp=Max(depth-1,points[i],tempChess,min);
				if (temp<max_alpha) {
					tempChess[p.X,p.Y]='0';
					return temp;
				}
				if (temp<min) {
					min=temp;
				}
			}
			tempChess[p.X,p.Y]='0';
			return min;
			
		}
		
		
	}
}
