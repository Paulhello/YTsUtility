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
        /// 两点间距离或向量模
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static double Get_Norm(_2D_Point p1, _2D_Point p2)
        {
            return Math.Sqrt((p1 - p2).X * (p1 - p2).X + (p1 - p2).Y * (p1 - p2).Y);
        }

        /// <summary>
        /// 获取该点到原点的距离
        /// </summary>
        /// <returns></returns>
        public double Get_Norm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }
        /// <summary>
        /// 计算一条边的方位角
        /// </summary>
        /// <param name="pre"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static Angle Get_PAngle(_2D_Point pre, _2D_Point pos)
        {
            _2D_Point preDxy;
            preDxy = pos - pre;
            //posDxy = pos - cur;
            double a;
            Angle pAngle;
            a = Math.Atan(preDxy.Y / preDxy.X);
            //b = Math.Atan(posDxy.Y / posDxy.X);
            if (a >= 0)
            {
                if (preDxy.X > 0)
                {
                    pAngle = Angle.ChangeToAngle(a);
                }
                else
                {
                    pAngle = Angle.ChangeToAngle(a + Math.PI);
                }
            }
            else
                if (preDxy.X >= 0)
                {
                    pAngle =Angle.ChangeToAngle(a + Math.PI * 2);
                }
                else
                {
                    pAngle = Angle.ChangeToAngle(a + Math.PI);
                }
            return pAngle;
        }

        /// <summary>
        /// 返回绝对值
        /// </summary>
        /// <returns></returns>
        public _2D_Point Abs()
        {
            _2D_Point t = new _2D_Point();
            t.X = Math.Abs(this.X);
            t.Y = Math.Abs(this.Y);
            return t;
        }

        /// <summary>
        /// 设置精度
        /// </summary>
        /// <param name="n"></param>
        public void Set_Accurate(int n)
        {
            this.X = this.X.SetAccurate(n);
            this.Y = this.Y.SetAccurate(n);
        }

        /// <summary>
        /// 转换成整数，四舍五入
        /// </summary>
        /// <returns></returns>
        public _2D_Point ChangeToInt()
        {
            _2D_Point t = new _2D_Point();
            t.X = (int)(this.X + 0.5);
            t.Y = (int)(this.Y + 0.5);
            return t;
        }

        /// <summary>
        /// 重载ToString,转换为每个元素的字符串相加
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X.ToString() + "," + Y;
        }

        /// <summary>
        /// 转换为字符串，保留到小数点后n位
        /// </summary>
        /// <param name="n">保留到小数点后n位</param>
        /// <returns></returns>
        public string ToFormatString(int n)
        {
            return X.ToFormatString(n) + "," + Y.ToFormatString(n);
        }

        /// <summary>
        /// 输出点数组
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="p"></param>
        public static void Output(string filename, _2D_Point[] p)
        {
            StreamWriter writer = new StreamWriter(filename);
            for (int i = 0; i < p.Length; i++)
                writer.WriteLine(p[i].ToString());
            writer.Close();
        }
        /// <summary>
        /// 对外提供一个复制能力
        /// </summary>
        /// <returns></returns>
        public _2D_Point Clone()
        {
            return new _2D_Point(X,Y);
        }

        /// <summary>
        /// 转换为列矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix ToColumnMatrix()
        {
            double []a=new double[]{X,Y};
            return new Matrix(2, 1, a);
        }

        /// <summary>
        /// 转换成XY排列的列矩阵
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Matrix ToColumnMatrix(List<_2D_Point> points)
        {
            Matrix t = new Matrix(2 * points.Count, 1);
            for (int i = 0; i < points.Count; i++)
            {
                t[2 * i, 0] = points[i].X;
                t[2 * i + 1, 0] = points[i].Y;
            }
            return t;
        }

        /// <summary>
        /// 从文件读取二维坐标,每行一个坐标，同逗号或者空格隔开
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns>坐标表</returns>
        public static List<_2D_Point> ReadPoint(string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            List<_2D_Point> pl = new List<_2D_Point>();
            while(sr.Peek()!=-1)
            {
                var s = Regex.Split(sr.ReadLine(), ",| +");
                pl.Add(new _2D_Point(double.Parse(s[0]), double.Parse(s[1])));
            }
            sr.Close();
            return pl;
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
        public static _3D_Point operator *(double f,_3D_Point p )
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
        public static _3D_Point operator /(_3D_Point p,double f)
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
        /// 设置精度
        /// </summary>
        /// <param name="n"></param>
        public void Set_Accurate(int n)
        {
            this.X = Methods.Set_Accurate(X, n);
            this.Y = Methods.Set_Accurate(Y, n);
            this.Z = Methods.Set_Accurate(Z, n);
        }

        /// <summary>
        /// 取整，四舍五入
        /// </summary>
        /// <returns></returns>
        public _3D_Point ChangeToInt()
        {
            _3D_Point t = new _3D_Point();
            t.X = (int)(X + 0.5);
            t.Y = (int)(Y + 0.5);
            t.Z = (int)(Y+0.5);
            return t;
        }

        /// <summary>
        /// 重载ToString,转换为每个元素的字符串相加
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return X.ToString() + "," + Y+","+Z;
        }

        /// <summary>
        /// 转换为字符串，保留到小数点后n位
        /// </summary>
        /// <param name="n">保留到小数点后n位</param>
        /// <returns></returns>
        public string ToFormatString(int n)
        {
            return X.ToFormatString(n) + "," + Y.ToFormatString(n)+","+Z.ToFormatString(n);
        }
        ///// <summary>
        ///// 转换为BLH型
        ///// </summary>
        ///// <param name="accurate">保留到小数点后几位</param>
        ///// <returns></returns>
        //public string ToBLHString(int accurate)
        //{
        //    Angle B = new Angle(X);
        //    Angle L = new Angle(Y);
        //    B.Set_Accurate(accurate);
        //    L.Set_Accurate(accurate);
        //    Z = Methods.Set_Accurate(Z, accurate);
        //    return B.ToString() + "," + L + "," + Z;
        //}

        /// <summary>
        /// 对外提供一个复制能力
        /// </summary>
        /// <returns></returns>
        public _3D_Point Clone()
        {
            return new _3D_Point(X,Y,Z);
        }

        /// <summary>
        /// 转换为列矩阵
        /// </summary>
        /// <returns></returns>
        public Matrix ToColumnMatrix()
        {
            double[] a = new double[] { X, Y ,Z};
            return new Matrix(3, 1, a);
        }

        /// <summary>
        /// 转换成XYZ排列的列矩阵
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Matrix ToColumnMatrix(List<_3D_Point> points)
        {
            Matrix t = new Matrix(3*points.Count,1);
            for(int i=0;i<points.Count;i++)
            {
                t[3 * i, 0] = points[i].X;
                t[3 * i+1, 0] = points[i].Y;
                t[3 * i+2, 0] = points[i].Z;
            }
            return t;
        }
        /// <summary>
        /// 输出三维坐标
        /// </summary>
        /// <param name="points">点List</param>
        /// <param name="path">保存路径</param>
        public static void OutPut(List<_3D_Point> points,string path)
        {
            StreamWriter sw = new StreamWriter(path,false, Encoding.Default);
            points.ForEach(p => sw.WriteLine(p));
            sw.Close();
        }

    }

    namespace GeodeticSurveying
    {

        /// <summary>
        /// 大地坐标类
        /// </summary>
       [Serializable]
        public class Geodetic_Point
        {
            /// <summary>
            /// 大地纬度
            /// </summary>
            public Angle B { set; get; }
            /// <summary>
            /// 大地经度
            /// </summary>
            public Angle L { set; get; }
            /// <summary>
            /// 大地高
            /// </summary>
            public double H { set; get; }
            /// <summary>
            /// 弧度表示的纬度(用于计算)
            /// </summary>
            public double Br { set; get; }
            /// <summary>
            /// 弧度表示的经度(用于计算)
            /// </summary>
            public double Lr { set; get; }
            /// <summary>
            /// 高度(用于计算)
            /// </summary>
            public double Hr { set; get; }
            /// <summary>
            /// 构造一个默认实例
            /// </summary>
            public Geodetic_Point()
            {
                B = new Angle();
                L = new Angle();
            }

            /// <summary>
            /// 通过经纬度和大地高构造
            /// </summary>
            /// <param name="B">大地纬度</param>
            /// <param name="L">大地经度</param>
            /// <param name="H">大地高</param>
            public Geodetic_Point(Angle B, Angle L, double H)
            {
                this.B = B;
                this.L = L;
                this.H = Hr = H;
                Br = B.ChangeToRad();
                Lr = L.ChangeToRad();
            }

            /// <summary>
            /// 通过经纬度(ddffmm.mmm或弧度)和大地高构造
            /// </summary>
            /// <param name="B">大地纬度(ddffmm.mmm或弧度)</param>
            /// <param name="L">大地经度(ddffmm.mmm或弧度)</param>
            /// <param name="H">大地高</param>
            /// <param name="bl_mode">true代表经纬度为ddffmm.mmm形式，false表示经纬度为弧度</param>
            public Geodetic_Point(bool bl_mode, double B, double L, double H)
            {
                if (bl_mode)
                {
                    this.B = new Angle(B);
                    this.L = new Angle(L);
                    this.H = Hr = H;
                    Br = this.B.ChangeToRad();
                    Lr = this.L.ChangeToRad();
                }
                else
                {
                    Br = B; Lr = L;
                    this.B = Angle.ChangeToAngle(B);
                    this.L = Angle.ChangeToAngle(L);
                    this.H = Hr = H;
                }
            }

            ///// <summary>
            ///// 通过经纬度(弧度)和大地高构造
            ///// </summary>
            ///// <param name="B">大地纬度(弧度)</param>
            ///// <param name="L">大地经度(弧度)</param>
            ///// <param name="H">大地高</param>
            //public Geodetic_Point(double B,double L,double H)
            //{
            //    Br = B; Lr = L;
            //    this.B = Angle.ChangeToAngle(B);
            //    this.L = Angle.ChangeToAngle(L);
            //    this.H = H;
            //}

            /// <summary>
            /// 转换为三维坐标，经纬度用弧度表示
            /// </summary>
            /// <returns></returns>
            public _3D_Point To3D()
            {
                return new _3D_Point(Br, Lr, Hr);
            }
            /// <summary>
            /// 设置经纬度和大地高精度
            /// </summary>
            /// <param name="n1">经纬度保留小数点后位数</param>
            /// <param name="n2">大地高保留小数点后位数</param>
            public void SetAccurate(int n1, int n2)
            {
                B.Set_Accurate(n1);
                L.Set_Accurate(n1);
                H = Methods.Set_Accurate(H, n2);
            }

            /// <summary>
            /// 转换为字符串
            /// </summary>
            /// <param name="mode">0表示返回度分秒,1表示返回ddffmm.mmm,其他返回表示弧度</param>
            /// <returns></returns>
            public string ToString(int mode=0)
            {
                if (mode == 0)
                    return B + "," + L + "," + H;
                else
                    if (mode == 1)
                        return B.ChangeToDouble() + "," + L.ChangeToDouble() + "," + H;
                    else
                        return Br + "," + Lr + "," + H;
            }

            /// <summary>
            /// 转换为字符串，保留到小数点后n位
            /// </summary>
            /// <param name="n">保留到小数点后n位</param>
            /// <param name="mode">0表示返回度分秒,1表示返回ddffmm.mmm,其他返回表示弧度</param>
            /// <returns></returns>
            public string ToFormatString(int n,int mode=0)
            {
                if (mode == 0)
                    return B.ToFormatString(n) + "," + L.ToFormatString(n) + "," + H.ToFormatString(n);
                else
                    if (mode == 1)
                        return B.ChangeToDouble().ToFormatString(n) + "," + L.ChangeToDouble().ToFormatString(n) + "," + H.ToFormatString(n);
                    else
                        return Br.ToFormatString(n) + "," + Lr.ToFormatString(n) + "," + H.ToFormatString(n);
            }

            /// <summary>
            /// 输出大地坐标数组
            /// </summary>
            /// <param name="points">坐标List</param>
            /// <param name="path">路径</param>
            /// <param name="mode">0表示返回度分秒,1表示返回ddffmm.mmm,其他返回表示弧度</param>
            public static void OutPut(List<Geodetic_Point> points, string path,int mode=0)
            {
                StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
                points.ForEach(p => sw.WriteLine(p.ToString(mode)));
                sw.Close();
            }

        }

        /// <summary>
        /// 点的经纬度坐标
        /// </summary>
        [Serializable]
        public class Geo2Point
        {
            /// <summary>
            /// 大地纬度
            /// </summary>
            public Angle B { set; get; }
            /// <summary>
            /// 大地经度
            /// </summary>
            public Angle L { set; get; }
            /// <summary>
            /// 弧度表示的纬度(用于计算)
            /// </summary>
            public double Br { set; get; }
            /// <summary>
            /// 弧度表示的经度(用于计算)
            /// </summary>
            public double Lr { set; get; }
            /// <summary>
            /// 构造一个默认实例
            /// </summary>
            public Geo2Point()
            {
                B = new Angle();
                L = new Angle();
            }

            /// <summary>
            /// 通过经纬度构造
            /// </summary>
            /// <param name="B">大地纬度</param>
            /// <param name="L">大地经度</param>
            public Geo2Point(Angle B, Angle L)
            {
                this.B = B;
                this.L = L;
                Br = B.ChangeToRad();
                Lr = L.ChangeToRad();
            }

            /// <summary>
            /// 通过经纬度(ddffmm.mmm或弧度)构造
            /// </summary>
            /// <param name="B">大地纬度(ddffmm.mmm或弧度)</param>
            /// <param name="L">大地经度(ddffmm.mmm或弧度)</param>
            /// <param name="bl_mode">true代表经纬度为ddffmm.mmm形式，false表示经纬度为弧度</param>
            public Geo2Point(bool bl_mode, double B, double L)
            {
                if (bl_mode)
                {
                    this.B = new Angle(B);
                    this.L = new Angle(L);
                    Br = this.B.ChangeToRad();
                    Lr = this.L.ChangeToRad();
                }
                else
                {
                    Br = B; Lr = L;
                    this.B = Angle.ChangeToAngle(B);
                    this.L = Angle.ChangeToAngle(L);
                }
            }

            /// <summary>
            /// 转换为二维坐标，经纬度用弧度表示
            /// </summary>
            /// <returns></returns>
            public _2D_Point To2D()
            {
                return new _2D_Point(Br, Lr);
            }
            /// <summary>
            /// 设置经纬度和大地高精度
            /// </summary>
            /// <param name="n">经纬度保留小数点后位数</param>
            public void SetAccurate(int n)
            {
                B.Set_Accurate(n);
                L.Set_Accurate(n);
            }

            /// <summary>
            /// 转换为字符串
            /// </summary>
            /// <param name="mode">0表示返回度分秒,1表示返回ddffmm.mmm,其他返回表示弧度</param>
            /// <returns></returns>
            public string ToString(int mode=0)
            {
                if (mode == 0)
                    return B + "," + L ;
                else
                    if (mode == 1)
                        return B.ChangeToDouble() + "," + L.ChangeToDouble();
                    else
                        return Br + "," + Lr ;
            }

            /// <summary>
            /// 返回字符串
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return ToString(0);
            }

            /// <summary>
            /// 转换为字符串，保留到小数点后n位
            /// </summary>
            /// <param name="n">保留到小数点后n位</param>
            /// <param name="mode">0表示返回度分秒,1表示返回ddffmm.mmm,其他返回表示弧度</param>
            /// <returns></returns>
            public string ToFormatString(int n, int mode = 0)
            {
                if (mode == 0)
                    return B.ToFormatString(n) + "," + L.ToFormatString(n) ;
                else
                    if (mode == 1)
                        return B.ChangeToDouble().ToFormatString(n) + "," + L.ChangeToDouble().ToFormatString(n);
                    else
                        return Br.ToFormatString(n) + "," + Lr.ToFormatString(n);
            }
            /// <summary>
            /// 输出经纬度坐标数组
            /// </summary>
            /// <param name="points">坐标List</param>
            /// <param name="path">路径</param>
            /// <param name="mode">0表示度分秒,1表示返回ddffmm.mmm,其他表示弧度</param>
            public static void OutPut(List<Geo2Point> points, string path,int mode=0)
            {
                StreamWriter sw = new StreamWriter(path, false, Encoding.Default);
                points.ForEach(p => sw.WriteLine(p.ToString(mode)));
                sw.Close();
            }
        }
    }

}

