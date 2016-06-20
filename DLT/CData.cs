using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTsUtility;
using System.IO;
using System.Text.RegularExpressions;

namespace DLT
{
    public class CData
    {
        public List<string> pNames=new List<string>();//控制点点名
        public List<string> pINames=new List<string>();//所有点点名
        public List<List<_2D_Point>> oIPoints=new List<List<_2D_Point>>();//控制点像方坐标
        public List<List<_2D_Point>> iPoints=new List<List<_2D_Point>>();//所有点像方坐标
        public List<_3D_Point> allPoints=new List<_3D_Point>();//所有点物方坐标
        public List<_3D_Point> oPoints=new List<_3D_Point>();//控制点物方坐标
        public int pCount=0;//点数
        public int Count=0;//重叠度
        public int oCount=0;//控制点数
        public void Clear()
        {
            Count = 0;
            pCount = 0;
            oCount = 0;
            pNames.Clear();
            oIPoints.Clear();
            oPoints.Clear();
            allPoints.Clear();
            iPoints.Clear();
            pINames.Clear();
        }
        public void ReadData(string path1,string path2)//读取像方数据文件,控制点，加密点
        {
            //StreamReader sr = new StreamReader(path1);
            var s = File.ReadAllLines(path1, Encoding.Default);
            oCount = s.Length;
            var ss = Regex.Split(s[0], " +");
            Count = (ss.Length - 4) / 2;
	        for (int i = 0; i < Count; i++)
	        {
		        oIPoints.Add(new List<_2D_Point>());
		        iPoints.Add(new List<_2D_Point>());
	        }
	        for (int i = 0; i < oCount;i++)
	        {
                ss = Regex.Split(s[i], " +");
		        pNames.Add(ss[0]);
		        for (int j = 0; j < Count;j++)
		        {
			        _2D_Point p=new _2D_Point();
                    p.X = double.Parse(ss[1+2*j]);
                    p.Y = double.Parse(ss[2 + 2 * j]);
			        oIPoints[j].Add(p);
		        }
		        _3D_Point p3=new _3D_Point();
                p3.X = double.Parse(ss[1 + 2 * Count]);
                p3.Y = double.Parse(ss[2 + 2 * Count]);
                p3.Z = double.Parse(ss[3 + 2 * Count]);
		        oPoints.Add(p3);
	        }

            s = File.ReadAllLines(path2, Encoding.Default);
            pCount = s.Length;
	        for (int i = 0; i < pCount;i++)
	        {
                ss = Regex.Split(s[i], " +");
		        pINames.Add(ss[0]);
		        for (int j = 0; j < Count; j++)
		        {
			        _2D_Point p=new _2D_Point();
                    p.X = double.Parse(ss[1 + 2 * j]);
                    p.Y = double.Parse(ss[2 + 2 * j]);
			        iPoints[j].Add(p);
		        }
	        }
        }

    }
}
