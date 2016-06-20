using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
//using Microsoft.Office.Interop.Excel;
using System.Diagnostics;

namespace YTsUtility
{
    /// <summary>
    /// 静态工具类
    /// </summary>
    public  static partial class Methods
    {
        
        /// <summary>
        /// 1弧度对应的秒
        /// </summary>
        /// <returns></returns>
        public static readonly double ρ= 180 / Math.PI * 3600;

        /// <summary>
        /// pi
        /// </summary>
        public static readonly double PI = 3.141592653589793;

        /// <summary>
        /// e
        /// </summary>
        public static readonly double E = 2.718281828459;

        /// <summary>
        ///求平均值 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Average(double x, double y)
        {
            return (x + y) / 2;
        }

        /// <summary>
        /// 精确到小数点后几位
        /// </summary>
        /// <param name="f"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double Set_Accurate(double f, int n)
        {
            bool flag = true;
            if (f < 0) flag = false;
            long t = Convert.ToInt64(Math.Pow(10, n) * Math.Abs(f));
            if (f * 10 - t * 10 > 5)
            {
                t++;
                if (flag)
                    return t / (Math.Pow(10, n));
                else return -t / (Math.Pow(10, n));
            }
            else if (f * 10 - t * 10 < 5)
            {
                if (flag)
                    return t / (Math.Pow(10, n));
                else return -t / (Math.Pow(10, n));
            }
            else
            {
                if (t % 2 == 1)
                {
                    if (flag)
                        return ++t / Math.Pow(10, n);
                    else return -++t / Math.Pow(10, n);
                }
                else
                {
                    if (flag)
                        return t / Math.Pow(10, n);
                    else return -t / Math.Pow(10, n);
                }
            }

        }

        //public static string Set_Accurate(int n,double f)
        //{
        //    f = Set_Accurate(f, n);

        //    return f.ToString(format);
        //}

        /// <summary>
        /// 向下取整
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static int Integer(double f)
        {
            if (f >= 0)
                return (int)(f + 0.5);
            else
                return (int)(f - 0.5);
        }

        /// <summary>
        /// 确定观测角的方向
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// 左角
            /// </summary>
            left,
            /// <summary>
            /// 右角
            /// </summary>
            right
        }

        /// <summary>
        /// 将字符标记转换为角方向
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Direction CovertToDirect(string s)
        {
            Direction direct;
            switch (s)
            {
                case "l":
                case "L": direct= Direction.left;break;
                case "r":
                case "R": direct= Direction.right;break;
                default: direct = Direction.left;break;
            }
            return direct;
        }

        /// <summary>
        /// 设置精度
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n">保留到小数点后n位</param>
        /// <returns></returns>
        public static double SetAccurate(this double x,int n)
        {
            return Set_Accurate(x, n);
        }

        /// <summary>
        /// 转换为保留特定小数位的字符串
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n">保留到小数点后n位</param>
        /// <returns></returns>
        public static string ToFormatString(this double x,int n)
        {
            string format = "0.";
            for (int i = 0; i < n; i++)
            {
                format += "0";
            }
            return x.SetAccurate(n).ToString(format);
        }

        /// <summary>
        /// 在字符串后面加换行
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Endl(this string s)
        {
            return s+Environment.NewLine;
        }

        /// <summary>
        /// 在字符串后面加上时间
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DataTime(this string s)
        {
            return s +"\t"+ System.DateTime.Now.ToString();
        }

        /// <summary>
        /// 二进制序列化泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="filepath">文件路径</param>
        public static void Serialize<T>(this T t,string filepath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, t);
            stream.Close();
        }

        /// <summary>
        /// 反二进制序列化得到对象
        /// </summary>
        /// <typeparam name="T">类型名</typeparam>
        /// <param name="filepath">文件路径</param>
        public static T Deserialize<T>(string filepath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            T tt = (T)formatter.Deserialize(stream);
            stream.Close();
            return tt;
        }

        /// <summary>
        /// Xml序列化泛型方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="filepath">文件路径</param>
        public static void SerializeXml<T>(this T t,string filepath)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, t);
            stream.Close();
        }

        /// <summary>
        /// 反Xml序列化得到对象
        /// </summary>
        /// <typeparam name="T">类型名</typeparam>
        /// <param name="filepath">文件路径</param>
        public static T DeserializeXml<T>(string filepath)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            T tt = (T)formatter.Deserialize(stream);
            stream.Close();
            return tt;
        }

        /// <summary>
        /// 创建多个子文件夹
        /// </summary>
        /// <param name="di"></param>
        /// <param name="paths"></param>
        public static void CreateSubs(this DirectoryInfo di,List<string> paths)
        {
            paths.ForEach(p => di.CreateSubdirectory(p));
        }

        /// <summary>
        /// 执行dos命令(当前路径为可执行文件所在路径)
        /// </summary>
        /// <param name="command">命令字符串</param>
        /// <param name="second">等待毫秒</param>
        /// <returns></returns>
        public static string Execute(string command,int second)
        {
            string output = string.Empty;
            if(command!=null&&command!=string.Empty)
            {
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;
                process.StartInfo = startInfo;
                try
                {
                    if (process.Start())
                    {
                        process.StandardInput.WriteLine(command + Environment.NewLine + "exit");
                        if (second == 0)
                            process.WaitForExit();
                        else
                            process.WaitForExit(second);
                    }
                    output = process.StandardOutput.ReadToEnd();
                }
                catch
                {

                }
                finally
                {
                    if (process != null)
                        process.Close();
                }
            }

            return output;
        }

        /// <summary>
        /// 执行dos命令
        /// </summary>
        /// <param name="command">命令字符串</param>
        /// <returns></returns>
        public static string Execute(string command)
        {
            return Execute(command, 0);
        }

        /// <summary>
        /// 求二维数组的和
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double Sum2D(double[,]array)
        {
            double sum=0;
            for (int i = 0; i < array.GetLength(0); ++i)
                for (int j = 0; j < array.GetLength(1); ++j)
                    sum += array[i, j];
            return sum;
        }

        /// <summary>
        /// 判断是否小于阈值
        /// </summary>
        /// <param name="accurate">阈值</param>
        /// <param name="x">矩阵</param>
        /// <returns>是否小于阈值</returns>
        public static bool IsTerminating(double accurate, Matrix x)
        {
            for (int i = 0; i < x.Row; i++)
                for (int j = 0; j < x.Column; j++)
                {
                    if (x[i, j] <= accurate)
                        continue;
                    else
                        return false;
                }
            return true;
        }
    }

    /// <summary>
    /// 用于简化Math中函数的委托
    /// </summary>
    /// <param name="a">参数</param>
    /// <returns>值</returns>
    public delegate double DMath(double a);

}
