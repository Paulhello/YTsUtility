using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace YTsUtility
{
    
    /// <summary>
    /// 二维坐标类
    /// </summary>
    [Serializable]
    public class _2D_Point
    {
        //数据格式

        /// <summary>
        /// X坐标
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { set; get; }

        /// <summary>
        /// 原点
        /// </summary>
        public static _2D_Point Origin=new _2D_Point(0,0);

        /// <summary>
        /// 构造函数
        /// </summary>
        public _2D_Point() { }

        /// <summary>
        /// 构造一个实例
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public _2D_Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 对应相加
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static _2D_Point operator +(_2D_Point p1, _2D_Point p2)
        {
            _2D_Point t = new _2D_Point();
            t.X = p1.X + p2.X;
            t.Y = p2.Y + p1.Y;
            return t;
        }

        /// <summary>
        /// 对应相减
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static _2D_Point operator -(_2D_Point p1, _2D_Point p2)
        {
            _2D_Point t = new _2D_Point();
            t.X = p1.X - p2.X;
            t.Y = p1.Y - p2.Y;
            return t;
        }

        /// <summary>
        /// 均取相反数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static _2D_Point operator -(_2D_Point p)
        {
            _2D_Point t = new _2D_Point();
            t.X = -p.X;
            t.Y = -p.Y;
            return t;
        }

        /// <summary>
        /// 以原为起点的两向量数量积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double operator *(_2D_Point p1, _2D_Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static _2D_Point operator *(_2D_Point p, double f)
        {
            _2D_Point t = new _2D_Point();
            t.X = p.X * f;
            t.Y = p.Y * f;
            return t;
        }

        /// <summary>
        /// 判断是否均大于某一数的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool operator >=(_2D_Point p, double f)
        {
            if (p.X > f && p.Y > f)
                return true;
            else return false;
        }

        /// <summary>
        /// 判断是否均小于某一数的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool operator <=(_2D_Point p, double f)
        {
            if (p.X < f && p.Y < f)
                return true;
            else return false;
        }
        /// <summary>
        /// 重载ToString,转换为每个元素的字符串相加
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X.ToString() + "," + Y;
        }
    }

    
    /// <summary>
    /// 三维坐标
    /// </summary>
    [Serializable]
    public class _3D_Point
    {
        //数据格式

        /// <summary>
        /// X坐标
        /// </summary>
        public double X { set; get; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { set; get; }

        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z { set; get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public _3D_Point() { }

        /// <summary>
        /// 构造一个实例
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public _3D_Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// 对应相加
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static _3D_Point operator +(_3D_Point p1, _3D_Point p2)
        {
            _3D_Point t = new _3D_Point();
            t.X = p1.X + p2.X;
            t.Y = p2.Y + p1.Y;
            t.Z = p1.Z + p2.Z;
            return t;
        }

        /// <summary>
        /// 对应相减
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static _3D_Point operator -(_3D_Point p1, _3D_Point p2)
        {
            _3D_Point t = new _3D_Point();
            t.X = p1.X - p2.X;
            t.Y = p1.Y - p2.Y;
            t.Z = p1.Z - p2.Z;
            return t;
        }

        /// <summary>
        /// 均取相反数
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static _3D_Point operator -(_3D_Point p)
        {
            _3D_Point t = new _3D_Point();
            t.X = -p.X;
            t.Y = -p.Y;
            t.Z = -p.Z;
            return t;
        }

        /// <summary>
        /// 以原为起点的两向量数量积
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double operator *(_3D_Point p1, _3D_Point p2)
        {
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static _3D_Point operator *(_3D_Point p, double f)
        {
            _3D_Point t = new _3D_Point();
            t.X = p.X * f;
            t.Y = p.Y * f;
            t.Z = p.Z * f;
            return t;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static _3D_Point operator *(double f, _3D_Point p)
        {
            _3D_Point t = new _3D_Point();
            t.X = p.X * f;
            t.Y = p.Y * f;
            t.Z = p.Z * f;
            return t;
        }

        /// <summary>
        /// 坐标分量除以同一个数
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static _3D_Point operator /(_3D_Point p, double f)
        {
            _3D_Point t = new _3D_Point();
            t.X = p.X / f;
            t.Y = p.Y / f;
            t.Z = p.Z / f;
            return t;
        }

        /// <summary>
        /// 判断是否有大于某一数的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool operator >=(_3D_Point p, double f)
        {
            if (p.X > f || p.Y > f || p.Z > f)
                return true;
            else return false;
        }

        /// <summary>
        /// 判断是否有小于某一数的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static bool operator <=(_3D_Point p, double f)
        {
            if (p.X < f && p.Y < f && p.Z < f)
                return true;
            else return false;
        }

        /// <summary>
        /// 两点间距离或向量模
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Get_Norm(_3D_Point p1, _3D_Point p2)
        {
            return Math.Sqrt((p1 - p2).X * (p1 - p2).X + (p1 - p2).Y * (p1 - p2).Y + (p1 - p2).Z * (p1 - p2).Z);
        }

        /// <summary>
        /// 返回绝对值
        /// </summary>
        /// <returns></returns>
        public _3D_Point Abs()
        {
            _3D_Point t = new _3D_Point();
            t.X = Math.Abs(X);
            t.Y = Math.Abs(Y);
            t.Z = Math.Abs(Z);
            return t;
        }


        /// <summary>
        /// 重载ToString,转换为每个元素的字符串相加
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X.ToString() + "," + Y + "," + Z;
        }
    }

}

