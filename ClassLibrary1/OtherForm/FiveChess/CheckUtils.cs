/*
 * 由SharpDevelop创建。
 * 用户： yanghang
 * 日期: 2016/9/9
 * 时间: 20:36
 * 
 * 要改变这种模板请点击 工具|选项|代码编写|编辑标准头文件
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace YHExcelAddin
{
	/// <summary>
	/// Description of CheckUtils.
	/// </summary>
	public class CheckUtils
	{
		public CheckUtils()
		{
		}
		static int MaxCount=5;
		public static bool CheckFiveInline(List<Point> points)
		{
			bool result=false;
			foreach(Point p in points)
			{
				int x=p.X;
				int y=p.Y;
				bool horizontal=CheckHorizontal(x,y,points);
				bool vertical=CheckVertical(x,y,points);
				bool leftAngle=CheckLeft(x,y,points);
				bool rightAngle=CheckRight(x,y,points);
				result=horizontal||vertical||leftAngle||rightAngle;
			}
			return result;
		}
	
		
		
		static bool CheckRight(int x, int y, List<Point> points)
		{
			int count=1;
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x+i,y-i))) {
					count++;
				}
				else
					break;
			}
			
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x-i,y+i))) {
					count++;
				}
				else
					break;
			}
			if (count>=MaxCount) {
				return true;
			}
			
			return false;
		}
		
		static bool CheckLeft(int x, int y, List<Point> points)
		{
			int count=1;
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x+i,y+i))) {
					count++;
				}
				else
					break;
			}
			
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x-i,y-i))) {
					count++;
				}
				else
					break;
			}
			if (count>=MaxCount) {
				return true;
			}
			
			return false;
		}
		
		static bool CheckVertical(int x, int y, List<Point> points)
		{
			int count=1;
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x,y+i))) {
					count++;
				}
				else
					break;
			}
			
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x,y-i))) {
					count++;
				}
				else
					break;
			}
			if (count>=MaxCount) {
				return true;
			}
			
			return false;
		}
		
		static bool CheckHorizontal(int x, int y, List<Point> points)
		{
			int count=1;
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x+i,y))) {
					count++;
				}
				else
					break;
			}
			
			for (int i = 1; i < MaxCount; i++) {
				if (points.Contains(new Point(x-i,y))) {
					count++;
				}
				else
					break;
			}
			if (count>=MaxCount) {
				return true;
			}
			
			return false;
		}
	}
}
