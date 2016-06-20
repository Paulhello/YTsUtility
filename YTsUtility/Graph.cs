using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace YTsUtility
{
    namespace GIS
    {
        /// <summary>
        /// 节点组
        /// </summary>
        public struct Nodes
        {
            /// <summary>
            /// 节点名组
            /// </summary>
            public List<string> names;
            /// <summary>
            /// 是否被用过的标记
            /// </summary>
            public List<bool> used;
            /// <summary>
            /// 邻接矩阵
            /// </summary>
            public double[,] adj_Matrix;
        }

        /// <summary>
        /// 结果结构
        /// </summary>
        [Serializable]
        public struct Result
        {
            /// <summary>
            /// 起点名
            /// </summary>
            public string sName;
            /// <summary>
            /// 终点名
            /// </summary>
            public string eName;
            /// <summary>
            /// 总值
            /// </summary>
            public double dist;
            /// <summary>
            /// 路径
            /// </summary>
            public List<string> way;
            /// <summary>
            /// 转换为字符串
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                string s = string.Empty;
                way.ForEach(w => s += (w + " "));
                return sName + "->" + eName + ":" + dist + "\t路径:" + s;
            }
        }

        /// <summary>
        /// 最短路径图
        /// </summary>
        public class Graph
        {
            /// <summary>
            /// 节点表
            /// </summary>
            public Nodes nodes;// { set; get; }
            /// <summary>
            /// 结果表
            /// </summary>
            public List<Result> results { private set; get; }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="names">点名组</param>
            /// <param name="matrix">邻接矩阵</param>
            public Graph(string[] names, double[,] matrix)
            {
                nodes = new Nodes { adj_Matrix = matrix, names = new List<string>(), used = new List<bool>() };
                nodes.names.AddRange(names);
                nodes.used.AddRange(new bool[names.Length]);
                for (int i = 0; i < names.Length; i++)
                    nodes.used[i] = false;
                results = new List<Result>();
            }

            //dijk算法
            private void Dijkstra(string SName)
            {
                //如果已经存在结果就返回
                var s = from cell in results
                        where cell.sName == SName
                        select cell;
                if (s.Count() != 0) return;

                int length = nodes.adj_Matrix.GetLength(0);
                double[] Dist = new double[length];
                List<string>[] WaysList = new List<string>[length];
                for (int i = 0; i < length; i++)
                {
                    WaysList[i] = new List<string>();
                    WaysList[i].Add(SName);
                }
                int Start = nodes.names.IndexOf(SName);
                //int End = nodes.names.IndexOf(EName);

                //bool[] used = new bool[length];
                int k = 0; int maxint = 9999;
                for (int i = 0; i < length; i++)
                {
                    Dist[i] = nodes.adj_Matrix[Start, i];
                    nodes.used[i] = false;
                }

                for (int i = 0; i < length - 1; i++)
                {
                    double tmin = maxint;
                    for (int j = 0; j < length; j++)
                        if (!nodes.used[j] && tmin > Dist[j])
                        {
                            tmin = Dist[j];
                            k = j;
                        }
                    if (WaysList[k][WaysList[k].Count - 1] != nodes.names[k]) WaysList[k].Add(nodes.names[k]);
                    nodes.used[k] = true;
                    for (int j = 0; j < length; j++)
                        if (Dist[k] + nodes.adj_Matrix[k, j] < Dist[j])
                        {
                            WaysList[j].Clear();
                            //WaysList[j].Add(Start);
                            WaysList[j].AddRange(WaysList[k]);
                            WaysList[j].Add(nodes.names[j]);
                            Dist[j] = Dist[k] + nodes.adj_Matrix[k, j];
                        }


                }
                for (int i = 0; i < length; i++)
                {
                    if (Dist[i] == maxint)
                    {
                        if (SName == nodes.names[i]) continue;
                        Result r = new Result { sName = SName, eName = nodes.names[i], way = new List<string>() };
                        r.dist = double.PositiveInfinity;
                        r.way.Add("No Way!");
                        results.Add(r);
                    }
                    else
                    {
                        Result r = new Result { sName = SName, eName = nodes.names[i], dist = Dist[i], way = new List<string>() };
                        r.way.AddRange(WaysList[i]);
                        results.Add(r);
                    }

                }

            }//dijk

            /// <summary>
            /// 获取两点之间的最短路径
            /// </summary>
            /// <param name="SName">起点</param>
            /// <param name="EName">终点</param>
            /// <returns>结果</returns>
            public Result Get_Way(string SName, string EName)
            {

                Dijkstra(SName);
                var result = from cell in results
                             where cell.sName == SName && cell.eName == EName
                             select cell;
                return result.First();
            }

            /// <summary>
            /// 获取所有的最短路径
            /// </summary>
            /// <returns>最短路径结果表</returns>
            public List<Result> Get_All()
            {
                for (int i = 0; i < nodes.adj_Matrix.GetLength(0); i++)
                {
                    Dijkstra(nodes.names[i]);
                }
                return results;
            }

            /// <summary>
            /// 将结果输出到文件，默认为追加
            /// </summary>
            /// <param name="filepath">文件路径</param>
            /// <param name="append">是否追加</param>
            /// <param name="SName">起点名</param>
            /// <param name="EName">终点名</param>
            public void OutPut(string filepath, string SName, string EName, bool append = true)
            {
                StreamWriter sw = new StreamWriter(filepath, append, Encoding.Default);
                sw.WriteLine(Get_Way(SName, EName).ToString());
                sw.Close();
            }
            /// <summary>
            /// 将所有结果作为文件输出,默认为不追加
            /// </summary>
            /// <param name="filepath">文件名</param>
            /// <param name="append">是否是追加</param>
            public void OutPutAll(string filepath, bool append = false)
            {
                StreamWriter sw = new StreamWriter(filepath, append, Encoding.Default);
                Get_All();
                results.ForEach(r => sw.WriteLine(r.ToString()));
                sw.Close();
            }

            /// <summary>
            /// 将所有结果二进制序列化存储
            /// </summary>
            /// <param name="filepath">文件路径</param>
            public void Save(string filepath)
            {
                Get_All();
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                //results.ForEach(r => formatter.Serialize(stream, r));
                formatter.Serialize(stream, results);
                stream.Close();
            }

            /// <summary>
            /// 读取二进制文件，反序列化得到结果表
            /// </summary>
            /// <param name="filepath">文件路径</param>
            public void Read(string filepath)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                results = (List<Result>)formatter.Deserialize(stream);
                stream.Close();
            }
        }
    }
}
