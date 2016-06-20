using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace YTsUtility
{
    namespace GeodeticSurveying
    {
        /// <summary>
        /// 测站结构
        /// </summary>
        [Serializable]
        public class Station
        {
            /// <summary>
            /// 后视点名
            /// </summary>
            public string backName;
            /// <summary>
            /// 前视点名
            /// </summary>
            public string frontName;
            /// <summary>
            /// 后视距
            /// </summary>
            public double backdistance;
            /// <summary>
            /// 前视距
            /// </summary>
            public double frontdistance;
            /// <summary>
            /// 视距差
            /// </summary>
            public double slight;
            /// <summary>
            /// 累积视距差
            /// </summary>
            public double sumslight;
            /// <summary>
            /// 后尺读数1
            /// </summary>
            public double back1;
            /// <summary>
            /// 后尺读数2
            /// </summary>
            public double back2;
            /// <summary>
            /// 前尺读数1
            /// </summary>
            public double front1;
            /// <summary>
            /// 前尺读数2
            /// </summary>
            public double front2;
            /// <summary>
            /// 高差
            /// </summary>
            public double hight;
        }

        /// <summary>
        /// 水准点
        /// </summary>
        [Serializable]
        public class LevelPoint
        {
            /// <summary>
            /// 点名
            /// </summary>
            public string name;
            /// <summary>
            /// 是否已知高程或初值
            /// </summary>
            public bool known;
            /// <summary>
            /// 高程(默认为0)
            /// </summary>
            public double hight;
            /// <summary>
            /// 点名:高程
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return name+":"+hight;
            }
            /// <summary>
            /// 点名:特定小数点位数的高程
            /// </summary>
            /// <param name="n">高程的小数位数</param>
            /// <returns></returns>
            public string ToString(int n)
            {
                return name + ":" + hight.ToFormatString(n);
            }
        }

        /// <summary>
        /// 测段结构
        /// </summary>
        [Serializable]
        public struct Section
        {
            /// <summary>
            /// 测段名
            /// </summary>
            public string name;
            /// <summary>
            /// 测段高差
            /// </summary>
            public double hight;
            /// <summary>
            /// 测段距离
            /// </summary>
            public double distance;
            /// <summary>
            /// 测站数
            /// </summary>
            public int stationCount;
            /// <summary>
            /// 测段:高差,长度
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return name+":"+hight+","+distance;
            }

            /// <summary>
            /// 测段:特定小数位数的高差，长度
            /// </summary>
            /// <param name="n1">高差小数位数</param>
            /// <param name="n2">长度小数位数</param>
            /// <returns></returns>
            public string ToString(int n1,int n2)
            {
                return name+":"+hight.ToFormatString(n1)+","+distance.ToFormatString(n2);
            }
        }
        /// <summary>
        /// 二等水准测量
        /// </summary>
        [Serializable]
        public class LevelingMeasure2
        {
            ObservingManua om;
            List<Station> stationList;
            /// <summary>
            /// 测段表
            /// </summary>
            public List<Section> sectionList=new List<Section>();
            /// <summary>
            /// 测站数
            /// </summary>
            public int count;
            /// <summary>
            /// 通过测站列表构造
            /// </summary>
            /// <param name="stations"></param>
            public LevelingMeasure2(List<Station> stations)
            {
                this.stationList = stations;
                count = stations.Count;
            }

            /// <summary>
            /// 通过文件路径构造(暂时只支持天宝Dini3)
            /// </summary>
            /// <param name="filepath"></param>
            /// <param name="mode"></param>
            public LevelingMeasure2(string filepath,int mode=0)
            {
                om = new ObservingManua(filepath,mode);
                count = om.stationList.Count;
                stationList = om.stationList;
            }

            /// <summary>
            /// 计算高差(程)
            /// </summary>
            /// <param name="startpoint">起始站名</param>
            /// <param name="endpoint">终点站名</param>
            /// <param name="initialH">起点高程,默认为0,需要计算终点高程时对其赋值</param>
            /// <param name="distance">测段距离变量</param>
            /// <returns></returns>
            public double GetH(string startpoint, string endpoint,out double distance, double initialH = 0)
            {
                distance = 0;
                int start = (from s in stationList
                             where s.backName == startpoint
                             select stationList.IndexOf(s)).First();
                int end = (from s in stationList
                           where s.frontName == endpoint
                           select stationList.IndexOf(s)).First();
                double sumH = initialH;
                for (int i = start; i <= end; i++)
                {
                    sumH += stationList[i].hight;
                    distance += (stationList[i].backdistance + stationList[i].frontdistance);
                }
                return sumH;
            }

            /// <summary>
            /// 获取所有测段信息
            /// </summary>
            /// <param name="sectionName">测段列表,测段格式为s-e,s表示起点站名，e表示终点站名</param>
            public void GetAllSection(List<string> sectionName)
            {
                
                for (int i = 0; i < sectionName.Count; i++)
                {
                    var sn = sectionName[i].Split("-".ToCharArray());
                    var rel = from s in stationList
                              where sn[0] == s.backName
                              select s.backName;
                    var rel1 = from s in stationList
                               where sn[1] == s.frontName
                               select s.frontName;
                    Section sec = new Section { name = rel.First()+"-"+rel1.First() };
                    sec.hight = GetH(sn[0], sn[1],out sec.distance);
                    sectionList.Add(sec);
                }
            }
        }

        /// <summary>
        /// 观测数据类
        /// </summary>
        [Serializable]
        public class ObservingManua
        {
            /// <summary>
            /// 测站列表
            /// </summary>
            public List<Station> stationList;

            /// <summary>
            /// 通过文件实例化
            /// </summary>
            /// <param name="filepath">数据文件路径</param>
            /// <param name="mode">0表示天宝Dini03数据,1表示**数据</param>
            public ObservingManua(string filepath,int mode=0)
            {
                stationList = ReadData(filepath,mode);
            }
 
            /// <summary>
            /// 获取观测手簿的excel表格
            /// </summary>
            public void GetExcel()
            {
                Application app = new Application();
                Workbooks books = app.Workbooks;
                Workbook book = books.Add();
                Sheets sheets = book.Worksheets;
                _Worksheet sheet = (_Worksheet)sheets.get_Item(1);
                sheet.Cells.RowHeight = 20;

                #region 设置表头
                Range range1 = sheet.get_Range("A1", "A4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 3;
                range1.Value = "测站编号";

                range1 = sheet.get_Range("B1", "B2");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 3;
                range1.Value = "后尺";

                range1 = sheet.get_Range("C1");
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 9;
                range1.Value = "上丝";

                range1 = sheet.get_Range("C2");
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 9;
                range1.Value = "下丝";

                range1 = sheet.get_Range("B3", "C3");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "后距";

                range1 = sheet.get_Range("B4", "C4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "视距差d";

                range1 = sheet.get_Range("D1", "D2");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 3;
                range1.Value = "前尺";

                range1 = sheet.get_Range("E1");
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 9;
                range1.Value = "上丝";

                range1 = sheet.get_Range("E2");
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 9;
                range1.Value = "下丝";

                range1 = sheet.get_Range("D3", "E3");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "前距";

                range1 = sheet.get_Range("D4", "E4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "Σd";

                range1 = sheet.get_Range("F1", "F4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 6;
                range1.Value = "方向\r\n及\r\n尺号";

                range1 = sheet.get_Range("G1", "H2");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 12;
                range1.Value = "标尺读数";

                range1 = sheet.get_Range("G3", "G4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "基本分划\r\n（一次）";

                range1 = sheet.get_Range("H3", "H4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.Value = "辅助分划\r\n（两次）";

                range1 = sheet.get_Range("I1", "I4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 12;
                range1.Value = "基加K\r\n减辅\r\n（一减二）";

                range1 = sheet.get_Range("J1", "J4");
                range1.Merge(0);
                range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                range1.WrapText = true;
                range1.ColumnWidth = 12;
                range1.Value = "备考";
                #endregion
                app.Visible = true;
                for (int i = 0; i < stationList.Count; i++)
                {
                    #region 设置测站名
                    range1 = sheet.get_Range("A" + (4 * i + 5), "A" + (4 * i + 8));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.Value = stationList[i].backName;// +"-" + stationList[i].frontName;
                    #endregion

                    #region 设置后距以及视距差
                    range1 = sheet.get_Range("B" + (4 * i + 5), "C" + (4 * i + 5));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;

                    range1 = sheet.get_Range("B" + (4 * i + 6), "C" + (4 * i + 6));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;

                    range1 = sheet.get_Range("B" + (4 * i + 7), "C" + (4 * i + 7));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = stationList[i].backdistance.ToFormatString(1);

                    range1 = sheet.get_Range("B" + (4 * i + 8), "C" + (4 * i + 8));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = stationList[i].slight.ToFormatString(1);
                    #endregion

                    #region 设置后距以及视距差
                    range1 = sheet.get_Range("D" + (4 * i + 5), "E" + (4 * i + 5));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;

                    range1 = sheet.get_Range("D" + (4 * i + 6), "E" + (4 * i + 6));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;

                    range1 = sheet.get_Range("D" + (4 * i + 7), "E" + (4 * i + 7));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = stationList[i].frontdistance.ToFormatString(1);

                    range1 = sheet.get_Range("D" + (4 * i + 8), "E" + (4 * i + 8));
                    range1.Merge(0);
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = stationList[i].sumslight.ToFormatString(1);
                    #endregion

                    #region 设置方向及尺号
                    range1 = sheet.get_Range("F" + (4 * i + 5));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.Value = "后";

                    range1 = sheet.get_Range("F" + (4 * i + 6));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.Value = "前";

                    range1 = sheet.get_Range("F" + (4 * i + 7));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.Value = "后-前";

                    range1 = sheet.get_Range("F" + (4 * i + 8));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.Value = "h";
                    #endregion

                    #region 设置前后尺读数
                    range1 = sheet.get_Range("G" + (4 * i + 5));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].back1*100).ToFormatString(2);

                    range1 = sheet.get_Range("G" + (4 * i + 6));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].front1*100).ToFormatString(2);

                    range1 = sheet.get_Range("G" + (4 * i + 7));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].back1*100 - stationList[i].front1*100).ToFormatString(2);

                    range1 = sheet.get_Range("G" + (4 * i + 8));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;

                    range1 = sheet.get_Range("H" + (4 * i + 5));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].back2*100).ToFormatString(2);

                    range1 = sheet.get_Range("H" + (4 * i + 6));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].front2*100).ToFormatString(2);

                    range1 = sheet.get_Range("H" + (4 * i + 7));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    range1.Value = (stationList[i].back2*100 -100* stationList[i].front2).ToFormatString(2);

                    range1 = sheet.get_Range("H" + (4 * i + 8));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    double h = (100 * (stationList[i].back1 + stationList[i].back2 - stationList[i].front1 - stationList[i].front2) / 2);
                    int n=0;
                    if((100*h).SetAccurate(1)-(100*h).SetAccurate(0)!=0)
                        n=3;
                    else
                        n=2;
                    range1.Value =h.ToFormatString(n) ;
                    #endregion

                    #region 设置基辅差
                    range1 = sheet.get_Range("I" + (4 * i + 5));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    string s = "";
                    double d = stationList[i].back1 - stationList[i].back2;
                    if (Methods.Integer(10000 * d) > 0)
                        s = "+";
                    else
                        s = string.Empty;
                    range1.Value = s + Methods.Integer(10000 * d);

                    range1 = sheet.get_Range("I" + (4 * i + 6));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    d = stationList[i].front1 - stationList[i].front2;
                    if (Methods.Integer(10000 * d) > 0)
                        s = "+";
                    else
                        s = string.Empty;
                    range1.Value = s + Methods.Integer(10000 * d);

                    range1 = sheet.get_Range("I" + (4 * i + 7));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    range1.NumberFormatLocal = "@";
                    d = stationList[i].back1 - stationList[i].back2 - stationList[i].front1 + stationList[i].front2;
                    if (Methods.Integer(10000 * d) > 0)
                        s = "+";
                    else
                        s = string.Empty;
                    range1.Value = s + Methods.Integer(10000 * d);

                    range1 = sheet.get_Range("I" + (4 * i + 8));
                    range1.VerticalAlignment = XlVAlign.xlVAlignCenter;
                    range1.HorizontalAlignment = XlVAlign.xlVAlignCenter;
                    range1.WrapText = true;
                    #endregion
                }


            }

            private List<Station> ReadData(string filepath,int mode=0)
            {
                switch(mode)
                {
                    case 0:
                        #region 读天宝Dini03数据
                        List<Station> sl = new List<Station>();
                        StreamReader sr = new StreamReader(filepath, Encoding.Default);
                        //int count = 0;
                        while (sr.Peek() != -1)
                        {
                            string[] ss = new string[8];
                            int i = 0;
                            while (sr.Peek() != -1 && i < 8)
                            {
                                string s = sr.ReadLine();
                                if (s.Contains("#") || (!s.Contains("HD")))
                                    continue;
                                ss[i] = s;
                                i++;

                            }
                            try
                            {
                                #region
                                if (ss[0] == null) break;
                                Station st1 = new Station(), st2 = new Station();
                                st1.back1 = double.Parse(ss[0].Substring(ss[0].IndexOf("Rb") + 10, 7));

                                st1.back2 = double.Parse(ss[3].Substring(ss[3].IndexOf("Rb") + 10, 7));
                                st1.front1 = double.Parse(ss[1].Substring(ss[1].IndexOf("Rf") + 10, 7));
                                st1.front2 = double.Parse(ss[2].Substring(ss[2].IndexOf("Rf") + 10, 7));
                                st1.backdistance = double.Parse(ss[0].Substring(ss[0].IndexOf("HD") + 11, 6));
                                st1.frontdistance = double.Parse(ss[1].Substring(ss[1].IndexOf("HD") + 11, 6));
                                st1.backName = ss[0].Substring(ss[0].IndexOf("KD1") + 8, 5);
                                st1.backName = st1.backName.Trim();
                                st1.frontName = ss[1].Substring(ss[1].IndexOf("KD1") + 8, 5);
                                st1.frontName = st1.frontName.Trim();
                                //#region 设置读数格式
                                st1.back1 = Methods.Set_Accurate(st1.back1, 4);
                                st1.back2 = Methods.Set_Accurate(st1.back2, 4);
                                st1.front1 = Methods.Set_Accurate(st1.front1, 4);
                                st1.front2 = Methods.Set_Accurate(st1.front2, 4);
                                st1.backdistance = Methods.Set_Accurate(st1.backdistance, 1);
                                st1.frontdistance = Methods.Set_Accurate(st1.frontdistance, 1);
                                //#endregion
                                st1.slight = st1.backdistance - st1.frontdistance;
                                st1.hight = (st1.back1 - st1.front1 + st1.back2 - st1.front2) / 2;
                                if (sl.Count > 0)
                                    st1.sumslight = sl[sl.Count - 1].sumslight + st1.slight;
                                else
                                    st1.sumslight = st1.slight;
                                sl.Add(st1);

                                st2.front1 = double.Parse(ss[4].Substring(ss[4].IndexOf("Rf") + 10, 7));
                                st2.front2 = double.Parse(ss[7].Substring(ss[7].IndexOf("Rf") + 10, 7));
                                st2.back1 = double.Parse(ss[5].Substring(ss[5].IndexOf("Rb") + 10, 7));
                                st2.back2 = double.Parse(ss[6].Substring(ss[6].IndexOf("Rb") + 10, 7));
                                st2.backdistance = double.Parse(ss[5].Substring(ss[5].IndexOf("HD") + 11, 6));
                                st2.frontdistance = double.Parse(ss[4].Substring(ss[4].IndexOf("HD") + 11, 6));
                                st2.backName = ss[5].Substring(ss[5].IndexOf("KD1") + 8, 5);
                                st2.backName = st2.backName.Trim();
                                st2.frontName = ss[4].Substring(ss[4].IndexOf("KD1") + 8, 5);
                                st2.frontName = st2.frontName.Trim();

                                //#region 设置读数格式
                                st2.back1 = Methods.Set_Accurate(st2.back1 , 4);
                                st2.back2 = Methods.Set_Accurate(st2.back2 , 4);
                                st2.front1 = Methods.Set_Accurate(st2.front1 , 4);
                                st2.front2 = Methods.Set_Accurate(st2.front2 , 4);
                                st2.backdistance = Methods.Set_Accurate(st2.backdistance, 1);
                                st2.frontdistance = Methods.Set_Accurate(st2.frontdistance, 1);
                                //#endregion

                                st2.slight = st2.backdistance - st2.frontdistance;
                                st2.sumslight = sl[sl.Count - 1].sumslight + st2.slight;
                                st2.hight = (st2.back1 - st2.front1 + st2.back2 - st2.front2) / 2;
                                sl.Add(st2);
                                #endregion
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("测站" + ss[0].Substring(ss[0].IndexOf("KD1") + 8, 5).Trim() + "附近数据有误");
                                //return new List<Station>();
                                throw new Exception();
                            }
                        }
                        sr.Close();
                        return sl;
                    #endregion
                    case 1:
                    #region 读取**数据
                        sl = new List<Station>();
                        sr = new StreamReader(filepath, Encoding.Default);
                        //int count = 0;
                        while (sr.Peek() != -1)
                        {
                            string[] ss = new string[8];
                            int i = 0;
                            while (sr.Peek() != -1 && i < 8)
                            {
                                string s = sr.ReadLine();
                                if (!s.Contains("mm"))
                                    continue;
                                ss[i] = s;
                                i++;

                            }

                            string perrors="";
                            try
                            {
                                #region
                                if (ss[0] == null) break;
                                Station st1 = new Station(), st2 = new Station();
                                var sst1 = Regex.Split(ss[0], " +");
                                var sst2 = Regex.Split(ss[1], " +");
                                var sst3 = Regex.Split(ss[2], " +");
                                var sst4 = Regex.Split(ss[3], " +");

                                

                                char[] cend = new char[] { 'm' };
                                sst1[5] += "     "; sst2[5] += "     ";
                                st1.backName = sst1[5].Substring(sst1[5].IndexOf("m") + 2, 5);
                                st1.backName = st1.backName.Trim();
                                st1.frontName = sst2[5].Substring(sst2[5].IndexOf("m") + 2, 5);
                                st1.frontName = st1.frontName.Trim();
                                perrors = st1.backName;
                                st1.back1 = double.Parse(sst1[1].TrimEnd(cend));
                                st1.back2 = double.Parse(sst4[1].TrimEnd(cend));
                                st1.front1 = double.Parse(sst3[1].TrimEnd(cend));
                                st1.front2 = double.Parse(sst3[1].TrimEnd(cend));
                                st1.backdistance = Methods.Average(double.Parse(sst1[2].TrimEnd(cend)), double.Parse(sst4[2].TrimEnd(cend)));
                                st1.frontdistance = Methods.Average(double.Parse(sst2[2].TrimEnd(cend)), double.Parse(sst3[2].TrimEnd(cend)));



                                #region 设置读数格式
                                st1.back1 = Methods.Set_Accurate(st1.back1, 4);
                                st1.back2 = Methods.Set_Accurate(st1.back2, 4);
                                st1.front1 = Methods.Set_Accurate(st1.front1, 4);
                                st1.front2 = Methods.Set_Accurate(st1.front2 , 4);
                                st1.backdistance = Methods.Set_Accurate(st1.backdistance, 1);
                                st1.frontdistance = Methods.Set_Accurate(st1.frontdistance, 1);
                                #endregion
                                st1.slight = st1.backdistance - st1.frontdistance;
                                st1.hight = (st1.back1 - st1.front1 + st1.back2 - st1.front2) / 2;
                                if (sl.Count > 0)
                                    st1.sumslight = sl[sl.Count - 1].sumslight + st1.slight;
                                else
                                    st1.sumslight = st1.slight;
                                sl.Add(st1);

                                sst1 = Regex.Split(ss[4], " +");
                                sst2 = Regex.Split(ss[5], " +");
                                sst3 = Regex.Split(ss[6], " +");
                                sst4 = Regex.Split(ss[7], " +");


                                //sst1.ToList().ForEach(s => s = s);
                                //sst2.ToList().ForEach(s => s = s.TrimEnd(cend));
                                //sst3.ToList().ForEach(s => s = s.TrimEnd(cend));
                                //sst4.ToList().ForEach(s => s = s.TrimEnd(cend));

                                st2.back1 = double.Parse(sst2[1].TrimEnd(cend));
                                st2.back2 = double.Parse(sst3[1].TrimEnd(cend));
                                st2.front1 = double.Parse(sst1[1].TrimEnd(cend));
                                st2.front2 = double.Parse(sst4[1].TrimEnd(cend));
                                st2.backdistance = Methods.Average(double.Parse(sst2[2].TrimEnd(cend)), double.Parse(sst3[2].TrimEnd(cend)));
                                st2.frontdistance = Methods.Average(double.Parse(sst1[2].TrimEnd(cend)), double.Parse(sst4[2].TrimEnd(cend)));
                                sst1[5] += "     "; sst2[5] += "     "; 
                                st2.backName = sst2[5].Substring(sst2[5].IndexOf("m") + 2, 5);
                                st2.backName = st2.backName.Trim();
                                st2.frontName = sst1[5].Substring(sst1[5].IndexOf("m") + 2, 5);
                                st2.frontName = st2.frontName.Trim();

                                #region 设置读数格式
                                st2.back1 = Methods.Set_Accurate(st2.back1, 4);
                                st2.back2 = Methods.Set_Accurate(st2.back2, 4);
                                st2.front1 = Methods.Set_Accurate(st2.front1, 4);
                                st2.front2 = Methods.Set_Accurate(st2.front2, 4);
                                st2.backdistance = Methods.Set_Accurate(st2.backdistance, 1);
                                st2.frontdistance = Methods.Set_Accurate(st2.frontdistance, 1);
                                #endregion

                                st2.slight = st2.backdistance - st2.frontdistance;
                                st2.sumslight = sl[sl.Count - 1].sumslight + st2.slight;
                                st2.hight = (st2.back1 - st2.front1 + st2.back2 - st2.front2) / 2;
                                sl.Add(st2);
                                #endregion
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("测站" + perrors + "附近数据有误");
                                //return new List<Station>();
                                throw new Exception();
                            }
                        }
                        sr.Close();
                        return sl;
                    #endregion
                    default: return null;
                }
 


                
            }

        }

        /// <summary>
        /// 水准平差类
        /// </summary>
        [Serializable]
        public class LevelingAdjust
        {
            /// <summary>
            /// 测段表
            /// </summary>
            public List<Section> sectionList;
            /// <summary>
            /// 水准点表
            /// </summary>
            public List<LevelPoint> pointList;
            /// <summary>
            /// 未知数的改正数
            /// </summary>
            public Matrix Hx;
            /// <summary>
            /// B矩阵
            /// </summary>
            public Matrix HB;
            /// <summary>
            /// l矩阵
            /// </summary>
            public Matrix Hl;
            /// <summary>
            /// 权阵P
            /// </summary>
            public Matrix HP;
            /// <summary>
            /// 改正后的高程值
            /// </summary>
            public Matrix HH;
            /// <summary>
            /// 未知数的初值
            /// </summary>
            public Matrix H0;
            /// <summary>
            /// 未知数的协因数阵
            /// </summary>
            public Matrix HQ;
            /// <summary>
            /// 未知数的协方差阵
            /// </summary>
            public Matrix HD;
            /// <summary>
            /// 未知水准点名
            /// </summary>
            public List<string> xnames;
            /// <summary>
            /// 每公里单位权中误差
            /// </summary>
            public double xigema;
            List<double> pList;
            List<int> knownp;//已知点索引
            List<int> xindex;//未知数列号

            /// <summary>
            /// 构造
            /// </summary>
            /// <param name="secl">测段表</param>
            /// <param name="pl">水准点表</param>
            public LevelingAdjust(List<Section> secl,List<LevelPoint> pl)
            {
                sectionList = secl;
                pointList = pl;
                pList = new List<double>();
                knownp = new List<int>();
                xindex = new List<int>();
                xnames = new List<string>();
            }

            /// <summary>
            /// 序列化要求
            /// </summary>
            private LevelingAdjust()
            {

            }

            /// <summary>
            /// 获取各个点的初值
            /// </summary>
            /// <param name="knownPoint"></param>
            public void GetInitValue(List<string> knownPoint)
            {
                #region 计算初值
                for (int i = 0; i < knownPoint.Count; i++)
                {
                    var result = from p in pointList
                                 where p.name == knownPoint[i]
                                 select pointList.IndexOf(p);
                    pointList[result.First()].known = true;
                    knownp.Add(result.First());
                }
                List<int> curps = new List<int> { knownp[0] };
                while(!pointList.All(p=>p.known==true))
                {
                    List<int> tempindex = new List<int>();
                    for (int i = 0; i < curps.Count; i++)
                    {
                        var rel = from sec in sectionList
                                  where sec.name.Split("-".ToCharArray())[0] == pointList[curps[i]].name
                                  select sectionList.IndexOf(sec);
                        for (int j = 0; j < rel.Count(); j++)
                        {
                            int sindex = rel.ElementAt(j);
                            string nextp = sectionList[sindex].name.Split("-".ToCharArray())[1];
                            var rel1 = from p in pointList
                                       where p.name == nextp
                                       select pointList.IndexOf(p);
                            if (pointList[rel1.First()].known == false)
                            {
                                double h = pointList[curps[i]].hight;
                                pointList[rel1.First()].hight = h + sectionList[sindex].hight;
                                pointList[rel1.First()].known = true;
                            }
                            tempindex.AddRange(rel1);
                        }
                    }
                    curps = tempindex;
                    
                    
                    
                   
                }
                #endregion

                #region 初始化未知数表
                H0 = new Matrix(pointList.Count - knownp.Count, 1);
                for (int i = 0,j=0; i < pointList.Count; i++)
                {
                    if (!knownp.Contains(i))
                    {
                        xnames.Add(pointList[i].name);
                        xindex.Add(i);
                        H0[j,0] = pointList[i].hight;
                        j++;
                    }
                }
                #endregion
            }

            /// <summary>
            /// 水准网平差
            /// </summary>
            public void Adjust()
            {
                Matrix B = new Matrix(sectionList.Count, pointList.Count - knownp.Count);
                Matrix l = new Matrix(sectionList.Count, 1);
                Matrix P = new Matrix(sectionList.Count, sectionList.Count);
                for (int i = 0; i < sectionList.Count; i++)
                {
                    var sn=sectionList[i].name.Split(new char[]{'-'});
                    if(GetJ(sn[0])!=-1)
                       B[i, GetJ(sn[0])] = -1;
                    if (GetJ(sn[1]) != -1)
                       B[i, GetJ(sn[1])] = 1;
                    l[i, 0] = sectionList[i].hight + pointList[GetIndex(sn[0])].hight - pointList[GetIndex(sn[1])].hight;
                    pList.Add(1 / sectionList[i].distance);
                }

                for (int i = 0; i < P.Row; i++)
                {
                    P[i, i] = pList[i]*1000;
                }

                var x = (HQ=(B.T()*P*B).Inverse())*B.T() * P * l;
                x.SetAccurate(4);
                Hx = x;
                HB = B;
                Hl = l;
                HP = P;
                HH = H0 + Hx;
                
                var v = B * x - l;
                xigema = 1000*Math.Sqrt((v.T() * P * v)[0, 0] / (sectionList.Count - x.Column));
                HD = xigema *xigema* HQ;
                for (int i = 0; i < xindex.Count; i++)
                {
                    pointList[xindex[i]].hight = HH[i, 0];
                }
            }

            /// <summary>
            /// 通过点名获取未知数所在列数
            /// </summary>
            /// <param name="pname"></param>
            /// <returns></returns>
            private int GetJ(string pname)
            {
                var index = (from p in pointList
                             where p.name == pname
                             select pointList.IndexOf(p)).First();
                return xindex.IndexOf(index);
            }

            /// <summary>
            /// 获取点在list中的索引
            /// </summary>
            /// <param name="pname"></param>
            /// <returns></returns>
            private int GetIndex(string pname)
            {
                var rel = from p in pointList
                          where p.name == pname
                          select pointList.IndexOf(p);
                return rel.First();
            }
        }
    }
}
