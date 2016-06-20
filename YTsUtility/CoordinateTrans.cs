using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    namespace GeodeticSurveying
    {
        /// <summary>
        /// 椭球
        /// </summary>
        public enum Ellipse
        {
            /// <summary>
            /// 克氏椭球
            /// </summary>
            Klinefelter,
            /// <summary>
            /// 1975国际椭球
            /// </summary>
            International1975,
            /// <summary>
            /// WGS-84
            /// </summary>
            WGS84,
            /// <summary>
            /// 中国2000
            /// </summary>
            CGCS2000,
            /// <summary>
            /// 其他
            /// </summary>
            Other
        }
        /// <summary>
        /// 坐标转换类
        /// </summary>
        public class CoordinateTrans
        {
            /// <summary>
            /// 椭球
            /// </summary>
            public Ellipsoid ep { get; set; }
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="elp">椭球类型</param>
            /// <param name="a">椭球参数a(如果为自定义的椭球)</param>
            /// <param name="b">椭球参数b(如果为自定义的椭球)</param>
            public CoordinateTrans(Ellipse elp,double a=0,double b=0)
            {
                ep = new Ellipsoid();
                switch (elp)
                {
                    case Ellipse.International1975: ep.International_ellipsoid1975(); break;
                    case Ellipse.WGS84: ep.WGS84_ellipsoid(); break;
                    case Ellipse.CGCS2000: ep.CGCS2000_ellipsoid(); break;
                    case Ellipse.Other: ep = new Ellipsoid(a, b); break;
                }
            }

            ///// <summary>
            ///// 构造函数，自定义椭球
            ///// </summary>
            ///// <param name="a">椭球元素a</param>
            ///// <param name="b">椭球元素b</param>
           // public CoordinateTrans(double a, double b)
            //{
            //    ep = new Ellipsoid(a, b);
            //}

            /// <summary>
            /// 大地到子午面坐标系
            /// </summary>
            /// <param name="B">大地纬度</param>
            /// <returns></returns>
            public _2D_Point B2Xy(double B)
            {
                double x=ep.data.a*Math.Cos(B)/Auxiliary.GetW(ep.data.e_2,B),
                       y=ep.data.a/Auxiliary.GetV(ep.data.e2_2,B)*Math.Sin(B);
                return new _2D_Point(x, y);
            }

            /// <summary>
            /// 批量转换
            /// </summary>
            /// <param name="Bs">纬度的集合（弧度）</param>
            /// <returns></returns>
            public List<_2D_Point> Batch_B2Xy(List<double> Bs)
            {
                List<_2D_Point> Xy = new List<_2D_Point>();
                foreach (var b in Bs)
                {
                    Xy.Add(B2Xy(b));
                }
                return Xy;
            }

            /// <summary>
            /// 大地坐标到空间直角坐标
            /// </summary>
            /// <param name="BLH">大地坐标</param>
            /// <returns>直角坐标</returns>
            public _3D_Point Blh2Xyz(Geodetic_Point BLH)
            {
                double W = Auxiliary.GetW(ep.data.e_2, BLH.Br);
                double V = Auxiliary.GetV(ep.data.e2_2, BLH.Br);
                double N = ep.data.a / W;
                _3D_Point p = new _3D_Point();
                p.X = (N + BLH.H) * Math.Cos(BLH.Br) * Math.Cos(BLH.Lr);
                p.Y = (N + BLH.H) * Math.Cos(BLH.Br) * Math.Sin(BLH.Lr);
                p.Z = (N * (1 - ep.data.e_2) + BLH.H) * Math.Sin(BLH.Br);
                return p;
            }

            /// <summary>
            /// 批量大地坐标到空间直角坐标
            /// </summary>
            /// <param name="BLHs">大地坐标组</param>
            /// <returns>直角坐标组</returns>
            public List<_3D_Point> Batch_Blh2Xyz(List<Geodetic_Point> BLHs)
            {
                List<_3D_Point> XYZs = new List<_3D_Point>();
                foreach (var item in BLHs)
                {
                    XYZs.Add(Blh2Xyz(item));
                }
                return XYZs;
            }
            /// <summary>
            /// 空间直角坐标到大地坐标
            /// </summary>
            /// <param name="XYZ">直接坐标</param>
            /// <param name="mode">计算模式，默认为0（迭代法），其他为直接法</param>
            /// <returns>大地坐标</returns>
            public Geodetic_Point Xyz2Blh(_3D_Point XYZ, int mode = 0)
            {
                if (mode == 0)
                    return Iteration(XYZ);
                else
                    return Direct(XYZ);
            }

            /// <summary>
            /// 批量空间直角坐标到大地坐标
            /// </summary>
            /// <param name="XYZs">直角坐标组</param>
            /// <returns>大地坐标组</returns>
            public List<Geodetic_Point> Batch_Xyz2Blh(List<_3D_Point> XYZs)
            {
                List<Geodetic_Point> BLHs = new List<Geodetic_Point>();
                foreach (var item in XYZs)
                {
                    BLHs.Add(Xyz2Blh(item));
                }
                return BLHs;
            }

            private Geodetic_Point Iteration(_3D_Point XYZ)
            {
                _3D_Point blh = new _3D_Point();
                //var l = _2D_Point.Get_PAngle(new _2D_Point(0, 0), new _2D_Point(XYZ.X, XYZ.Y));
                blh.Y = Auxiliary.Get_L(XYZ.X,XYZ.Y);
                double dis = _2D_Point.Get_Norm(new _2D_Point(0, 0), new _2D_Point(XYZ.X, XYZ.Y));
                double t0 = XYZ.Z / dis,
                        p = ep.data.c * ep.data.e_2 / dis,
                        k = 1 + ep.data.e2_2,
                        t1 = t0,
                        t2 = t0;
                do
                {
                    t1 = t2;
                    t2 = t0 + p * t1 / Math.Sqrt(k + t1 * t1);
                }
                while (Math.Abs(Angle.ChangeToAngle(t2 - t1).Miao) > 0.0001);
                blh.X = Angle.ChangeToAngle(t2 = Math.Atan(t2)).ChangeToRad();
                double W = Auxiliary.GetW(ep.data.e_2, t2),
                       N = ep.data.a / W;
                double h1 = XYZ.Z - N * (1 - ep.data.e_2) * Math.Sin(t2),
                       h2 = dis - N * Math.Cos(t2);
                double h = Math.Sqrt(h1 * h1 + h2 * h2);
                if (h1 / Math.Sin(t2) < 0 || h2 / Math.Cos(t2) < 0)
                    h = -h;
                blh.Z = h;
                return new Geodetic_Point(false, blh.X, blh.Y, blh.Z);
            }

            private Geodetic_Point Direct(_3D_Point XYZ)
            {
                double e_2 = ep.data.e_2,
                       e2_2 = ep.data.e2_2,
                       a = ep.data.a,
                       b = ep.data.b;
                double r = _2D_Point.Get_Norm(new _2D_Point(0, 0), new _2D_Point(XYZ.X, XYZ.Y)),
                       u0 = Math.Atan(XYZ.Z / r * Math.Sqrt(1 + e2_2)),
                       tgB = (XYZ.Z + b * e2_2 * Math.Sin(u0) * Math.Sin(u0)) /
                             (r - a * e_2 * Math.Cos(u0) * Math.Cos(u0)),
                       B = Math.Atan(XYZ.Z / r + a * e_2 * tgB / r / Math.Sqrt(1 + (1 - e_2) * tgB * tgB));
                _3D_Point blh = new _3D_Point();
                //blh.X = Angle.ChangeToAngle(B).ChangeToDouble();
                blh.X = B;

                //var l = _2D_Point.Get_PAngle(new _2D_Point(0, 0), new _2D_Point(XYZ.X, XYZ.Y));
                //blh.Y = l.ChangeToDouble();
                blh.Y = Auxiliary.Get_L(XYZ.X,XYZ.Y);

                double W = Auxiliary.GetW(e_2, B),
                       N = a / W;
                double h1 = XYZ.Z - N * (1 - e_2) * Math.Sin(B),
                       h2 = r - N * Math.Cos(B);
                double h = Math.Sqrt(h1 * h1 + h2 * h2);
                if (h1 / Math.Sin(B) < 0 || h2 / Math.Cos(B) < 0)
                    h = -h;
                blh.Z = h;
                return new Geodetic_Point(false, blh.X, blh.Y, blh.Z);
            }
        }
    }
}
