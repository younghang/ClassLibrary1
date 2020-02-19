using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ExcelDna.Integration;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
namespace YHExcelAddin
{
    public static class MyExcelFunctions
    {
        [ExcelFunction(Description = "可变参数求和",
            ExplicitRegistration = true)]//必须显示注册
        public static object ParamsSum(params object[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                dynamic reference = values[i];

                for (int j = reference.GetLowerBound(0); j <= reference.GetUpperBound(0); j++)
                {
                    for (int k = reference.GetLowerBound(1); k <= reference.GetUpperBound(1); k++)
                    {
                        double s;
                        try
                        {
                            s = double.Parse(reference[j, k].ToString());
                        }
                        catch (Exception)
                        {
                            s = 0;
                        }
                        sum += s;
                    }

                }
            }
            return sum;
        }
        //[ExcelFunction(Description = "My first .NET function")]
    
        public static string SayHello(string name)
        {
            return "Hello " + name;
        }
        [ExcelCommand(MenuName = "Go To Top", MenuText = "A1")]
        public static void GotoA1()
        {
            dynamic app = ExcelDnaUtil.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;
            dynamic range = ws.Range["A1"];
            range.Select();

        }


        public static void RemoveXllMacro(string file)
        {
            Excel.Application app = ExcelDnaUtil.Application as Excel.Application;
            //string file="";
            //file = app.Selection.Text;
            try
            {
                dynamic vXlAutoRemoveHandle;

                string s = "REGISTER(\"" + file + "\", " + "\"xlAutoRemove\"" + "," + "\"J\"" + "," + ",,,1)";
                vXlAutoRemoveHandle = app.ExecuteExcel4Macro(s);
                dynamic vResultXlAutoRemove;
                s = "CALL(" + vXlAutoRemoveHandle + ")";
                vResultXlAutoRemove = app.ExecuteExcel4Macro(s);

                dynamic vResultUnregisterXlAutoRemove;
                s = "UNREGISTER(" + vXlAutoRemoveHandle + ")";
                vResultUnregisterXlAutoRemove = app.ExecuteExcel4Macro(s);
                dynamic vResultUnregister;
                s = "UNREGISTER(\"" + file + "\")";
                vResultUnregister = app.ExecuteExcel4Macro(s);
            }
            catch (Exception)
            {
                MessageBox.Show("Error");
            }

        }
        [ExcelCommand(MenuName = "Get Maximum Line", MenuText = "Get Maximum Line")]
        public static string GetMaxLineMacro()
        {
            dynamic app = ExcelDnaUtil.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;

            //ws.Name = "New name";
            //dynamic range = ws.Range["A1"];
            //range.Formula = "=1+2";
            int line = app.ActiveSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;
            //int line = ws.Application.Range["A65536", Missing.Value].End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;
            return line.ToString();
        }
        public static void GoToABottomMacro()
        {
            dynamic app = ExcelDnaUtil.Application;
            dynamic wb = app.ActiveWorkbook;
            dynamic ws = app.ActiveSheet;
            int line = ws.Application.Range["A65536", Missing.Value].End(Microsoft.Office.Interop.Excel.XlDirection.xlUp).Row;
            dynamic range = ws.Range["A" + line.ToString()];
            range.Select();
        }
        [ExcelFunction(Name = "GetMaxumumContinusCount", Description = "统计字段最多连续出现的次数,像“A,B,B,C,B”,找B的话，返回的就是最长2个连续", IsMacroType = true, IsVolatile = true)]
        public static object GetMaxumumContinusCount(
              [ExcelArgument(Name = "TargetArray", Description = "单元格区域", AllowReference = true)]object[,] arraySource,
            [ExcelArgument(Name = "FindCriteria", Description = "匹配字段")]string findCriteria)
        {
            if (findCriteria == "" || arraySource.Length == 0)
            {
                return "Cancel";
            }
            int MaxCount = 0;
            int temp = 0;
            if (arraySource.GetUpperBound(0) - arraySource.GetLowerBound(0) == 0 ||
                arraySource.GetUpperBound(1) - arraySource.GetLowerBound(1) == 0)
            {
                if (arraySource.GetUpperBound(0) - arraySource.GetLowerBound(0) == 0)
                {
                    for (int i = arraySource.GetLowerBound(1); i <= arraySource.GetUpperBound(1); i++)
                    {
                        string item = arraySource[arraySource.GetLowerBound(0), i].ToString();
                        if (item == "ExcelDna.Integration.ExcelEmpty")
                            item = "";
                        if (item.Contains(findCriteria))
                        {
                            temp++;
                            if (temp > MaxCount)
                                MaxCount = temp;
                        }
                        else
                        {
                            temp = 0;
                        }
                    }
                }
                else
                {
                    for (int i = arraySource.GetLowerBound(0); i <= arraySource.GetUpperBound(0); i++)
                    {
                        string item = arraySource[i,arraySource.GetLowerBound(1)].ToString();
                        if (item == "ExcelDna.Integration.ExcelEmpty")
                            item = "";
                        if (item.Contains(findCriteria))
                        {
                            temp++;
                            if (temp > MaxCount)
                                MaxCount = temp;
                        }
                        else
                        {
                            temp = 0;
                        }
                    }
                }
            }
            else
            {
                return "单列或单行才可以";
            }
            return MaxCount;
        }
        [ExcelFunction(Name = "MatchSheetData", Description = "匹配查找相应的单元格，绿色为匹配值，红色为重复值，需要手动判定", IsMacroType = true, IsVolatile = true)]
        public static object MatchSheetData(
            [ExcelArgument(Name = "ArrayDes", Description = "匹配依据的单元格区域", AllowReference = true)]object[,] arrayDes,
            [ExcelArgument(Name = "ArraySource", Description = "数据来源区域", AllowReference = true)]object[,] arraySource,
             [ExcelArgument(Name = "FillIndex", Description = "要套过来的内容在来源区域中第几列")]int sourceIndex,
            [ExcelArgument(Name = "FindCriteria", Description = "套过来的条件，多个用；隔开，如1=1；2=3")]string findCriteria

            )
        {

            if (!findCriteria.Contains("="))
            {
                return "Cancel";
            }
            string[] criterias = findCriteria.Split(new Char[] { ';', ',', '；' });
            if (criterias.Length < 1 || arraySource.Length == 0)
            {
                return "Cancel";
            }
            List<int> dest = new List<int>();
            List<int> source = new List<int>();
            sourceIndex = sourceIndex - 1;

            for (int i = 0; i < criterias.Length; i++)
            {
                dest.Add(int.Parse(criterias[i].Split('=')[0]) - 1);
                source.Add(int.Parse(criterias[i].Split('=')[1]) - 1);
            }
            Dictionary<string, int> dicDataFrequency = new Dictionary<string, int>();
            Dictionary<string, string> dicDataSource = new Dictionary<string, string>();
            for (int i = arraySource.GetLowerBound(0); i <= arraySource.GetUpperBound(0); i++)
            {
                string key = "";
                for (int j = 0; j < source.Count; j++)
                {
                    string item = arraySource[i, source[j]].ToString();
                    if (item == "ExcelDna.Integration.ExcelEmpty")
                        item = "";
                    key += item;
                }
                if (key == "")
                {
                    continue;
                }
                if(dicDataFrequency.ContainsKey(key))
                {
                    dicDataFrequency[key] = dicDataFrequency[key] + 1;
                }else
                {
                    dicDataFrequency.Add(key, 1);
                }
                
                while (dicDataSource.ContainsKey(key))
                {
                    key += "#";
                }
                string target = arraySource[i, sourceIndex].ToString();
                if (target == "ExcelDna.Integration.ExcelEmpty")
                    target = "";
                dicDataSource.Add(key, target);
            }
            if (dicDataSource.Count == 0)
            {
                return "Cancel";
            }
            List<string> keys = new List<string>();
            //Microsoft.Office.Interop.Excel.Application app = ExcelDnaUtil.Application as Microsoft.Office.Interop.Excel.Application;
            //Workbook wb = app.ActiveWorkbook;
            //Worksheet ws = app.ActiveSheet;
            //string s =(string) XlCall.Excel(XlCall.xlfReftext, arrayDes, true);
            //ExcelReference source = arraySource as ExcelReference;    
            ExcelReference caller = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
            //Worksheet sheet = XlCall.Excel(XlCall.xlfSheet, caller.SheetId) as Worksheet;
            Microsoft.Office.Interop.Excel.Application app = ExcelDnaUtil.Application as Microsoft.Office.Interop.Excel.Application;
            string address = XlCall.Excel(XlCall.xlfReftext, caller, true).ToString();
            Range pos = app.Range[address];
            int r = caller.RowFirst;
            int c = caller.ColumnFirst;
            for (int i = arrayDes.GetLowerBound(0); i <= arrayDes.GetUpperBound(0); i++)
            {
                pos = pos.Offset[1, 0];
                pos.Interior.Pattern = XlPattern.xlPatternNone;
                string key = "";
                for (int j = 0; j < dest.Count; j++)
                {
                    string item = arrayDes[i, dest[j]].ToString();
                    if (item == "ExcelDna.Integration.ExcelEmpty")
                        item = "";
                    key += item;
                }
                if (key == "")
                {
                    continue;
                }
                string strTemp = key;
                while (keys.Contains(key))
                {
                    key += "#";
                }
                keys.Add(key);
                if (dicDataSource.ContainsKey(key))
                {
                    string item = dicDataSource[key];
                    pos.Value = item;
                    if(dicDataFrequency[strTemp]>1)
                    {
                        pos.Interior.Color = Color.FromArgb(244, 176, 132);
                    }
                    else
                    {
                        pos.Interior.Color = Color.FromArgb(146, 208, 80); 
                    }                    
                }
            }

            return "OK";

        }
        public static IEnumerable<T[]> AsEnumerable<T>(this T[,] array)
        {
            for (int r = array.GetLowerBound(0); r <= array.GetUpperBound(0); r++)
            {
                if (array[r, 1] == null && array[r + 1, 1] == null && array[r + 2, 1] == null && array[r + 3, 1] == null && array[r + 4, 1] == null && array[r + 5, 1] == null) yield break;
                List<T> lst = new List<T>();
                for (int c = array.GetLowerBound(1); c <= array.GetUpperBound(1); c++)
                {
                    lst.Add(array[r, c]);
                }
                yield return lst.ToArray();
            }
        }

    }

}
