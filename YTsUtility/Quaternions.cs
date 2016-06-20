using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 四元数类
    /// </summary>
    public class Quaternions
    {
        /// <summary>
        /// 实数部分
        /// </summary>
        public double T { set; get; }
        /// <summary>
        /// 虚部数对
        /// </summary>
        public Vector3 V { set; get; }

        /// <summary>
        /// 默认构造函数，均为0
        /// </summary>
        public Quaternions()
        {
            T  = 0;
            V = new Vector3();
        }

        /// <summary>
        /// 通过有序数对构造
        /// </summary>
        /// <param name="t">实部</param>
        /// <param name="x">虚部1</param>
        /// <param name="y">虚部2</param>
        /// <param name="z">虚部3</param>
        public Quaternions(double t,double x,double y,double z)
        {
            T = t;
            V=new Vector3(x,y,z);
        }

        /// <summary>
        /// 通过实部和虚部构造
        /// </summary>
        /// <param name="t">实部</param>
        /// <param name="v">虚部数对</param>
        public Quaternions(double t,Vector3 v)
        {
            T = t;
            V = v.Clone() as Vector3;
        }

        /// <summary>
        /// 通过旋转轴与旋转角构造
        /// </summary>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">旋转角</param>
        public Quaternions(Vector3 axis,double angle)
        {

        }

        /// <summary>
        /// 四元数的模
        /// </summary>
        /// <returns></returns>
        public double GetNorm()
        {
            return Math.Sqrt(T * T + V.X * V.X + V.Y * V.Y + V.Z * V.Z);
        }

        /// <summary>
        /// 单位化后的四元数
        /// </summary>
        /// <returns></returns>
        public Quaternions Normlize()
        {
            double norm = GetNorm();
            return new Quaternions(T / norm, V / norm);
        }

        /// <summary>
        /// 共轭四元数
        /// </summary>
        /// <returns></returns>
        public Quaternions Conjugate()
        {
            return new Quaternions(T, -V);
        }

        /// <summary>
        /// 四元数的逆
        /// </summary>
        /// <returns></returns>
        public Quaternions Inverse()
        {
            return Conjugate() / GetNorm() / GetNorm();
        }

        /// <summary>
        /// 形如t+Xi+Yj+Zk的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "("+T+";"+V.X+","+V.Y+","+V.Z+")";
        }

        /// <summary>
        /// 四元数的和
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static Quaternions operator+(Quaternions q1,Quaternions q2)
        {
            return new Quaternions(q1.T + q2.T, q1.V+q2.V);
        }

        /// <summary>
        /// 四元数的积
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static Quaternions operator*(Quaternions q1,Quaternions q2)
        {
            Quaternions q = new Quaternions();
            q.T = q1.T * q2.T - q1.V * q2.V;
            q.V = q1.T * q2.V + q2.T * q1.V + q1.V.XMultpy(q2.V);
            return q;
        }

        /// <summary>
        /// 同除以一个实数
        /// </summary>
        /// <param name="q"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Quaternions operator/(Quaternions q,double f)
        {
            Quaternions t = new Quaternions();
            t.T = q.T / f;
            t.V = q.V / f;
            return t;
        }

        /// <summary>
        /// 转换成四元矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix ToMatrix()
        {
            Matrix m = new Matrix(4, 4);
            m[0, 0] = m[1, 1] = m[2, 2] = m[3, 3] = T;
            m[0, 1] = m[3, 2] = V.Z;
            m[0, 2] = m[1, 3] = V.X;
            m[0, 3] = m[2, 1] = V.Y;
            m[1, 0] = m[2, 3] = -V.Z;
            m[1, 2] = m[3, 0] = -V.Y;
            m[2, 0] = m[3, 1] = -V.X;
            return m;
        }
    }

    /// <summary>
    /// 字符四元数类
    /// </summary>
    public class StrQuaternions
    {
        /// <summary>
        /// 实数部分
        /// </summary>
        public string T { set; get; }
        /// <summary>
        /// 虚部数对
        /// </summary>
        public StrVector3 V { set; get; }

        /// <summary>
        /// 默认构造函数，均为0
        /// </summary>
        public StrQuaternions()
        {
            T = "0";
            V = new StrVector3();
        }

        /// <summary>
        /// 通过有序数对构造
        /// </summary>
        /// <param name="t">实部</param>
        /// <param name="x">虚部1</param>
        /// <param name="y">虚部2</param>
        /// <param name="z">虚部3</param>
        public StrQuaternions(string t, string x, string y, string z)
        {
            T = t;
            V = new StrVector3(x, y, z);
        }

        /// <summary>
        /// 通过实部和虚部构造
        /// </summary>
        /// <param name="t">实部</param>
        /// <param name="v">虚部数对</param>
        public StrQuaternions(string t, StrVector3 v)
        {
            T = t;
            V = v;
        }

        /// <summary>
        /// 通过旋转轴与旋转角构造
        /// </summary>
        /// <param name="axis">旋转轴</param>
        /// <param name="angle">旋转角</param>
        public StrQuaternions(StrVector3 axis, string angle)
        {

        }

        /// <summary>
        /// 四元数的模
        /// </summary>
        /// <returns></returns>
        public string GetNorm()
        {
            return "√("+T + "*" + T + "+" + V.X + "*" + V.X + "+" + V.Y + "*" + V.Y +"+"+ V.Z +"*"+ V.Z+")";
        }

        /// <summary>
        /// 单位化后的四元数
        /// </summary>
        /// <returns></returns>
        public StrQuaternions Normlize()
        {
            string norm = GetNorm();
            return new StrQuaternions(T+"/"+norm, V / norm);
        }

        /// <summary>
        /// 共轭四元数
        /// </summary>
        /// <returns></returns>
        public StrQuaternions Conjugate()
        {
            return new StrQuaternions(T, -V);
        }

        /// <summary>
        /// 四元数的逆
        /// </summary>
        /// <returns></returns>
        public StrQuaternions Inverse()
        {
            return Conjugate() / GetNorm() / GetNorm();
        }

        /// <summary>
        /// 形如t+Xi+Yj+Zk的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + T + ";" + V.X + "," + V.Y + "," + V.Z + ")";
        }

        /// <summary>
        /// 四元数的和
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static StrQuaternions operator +(StrQuaternions q1, StrQuaternions q2)
        {
            StrQuaternions q = new StrQuaternions();
            if (q1.T=="0")
            {
            }
            return new StrQuaternions("("+q1.T +"+"+ q2.T+")", q1.V + q2.V);
        }

        /// <summary>
        /// 四元数的积
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static StrQuaternions operator *(StrQuaternions q1, StrQuaternions q2)
        {
            StrQuaternions q = new StrQuaternions();
            q.T = "("+q1.T+"*"+q2.T +"-"+ q1.V * q2.V+")";
            q.V = q1.T * q2.V + q2.T * q1.V + q1.V.XMultpy(q2.V);
            return q;
        }

        /// <summary>
        /// 同除以一个实数
        /// </summary>
        /// <param name="q"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static StrQuaternions operator /(StrQuaternions q, string f)
        {
            StrQuaternions t = new StrQuaternions();
            t.T = q.T+"/"+f;
            t.V = q.V / f;
            return t;
        }
    }
}
