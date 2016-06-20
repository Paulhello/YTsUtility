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
        /// 高斯投影类
        /// </summary>
        public class GaussProjection
        {
            /// <summary>
            /// 椭球
            /// </summary>
            public Ellipsoid ep { get; set; }

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="elp">常用椭球类型</param>
            /// <param name="a">椭球参数a(如果为自定义的椭球)</param>
            /// <param name="b">椭球参数b(如果为自定义的椭球)</param>
            public GaussProjection(Ellipse elp, double a = 0, double b = 0)
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

            private Angle Get_L0(Angle L,int bandwidth)
            {
                double l = L.ChangeToDu();
                int L0;
                if (l < 0)
                    l = 360- Math.Abs(l);
                if(bandwidth==3)
                {
                    int n = (int)(l / 3 + 0.5);
                    L0 = 3 * n;
                }
                else
                {
                    int n = (int)(l / 6) + 1;
                    L0 = 6 * n - 3;
                }
                if (L0 <= 180)
                    return new Angle(L0, 0, 0);
                else
                    return new Angle(360 - L0, 0, 0, false);
            }

            /// <summary>
            /// 高斯投影正算
            /// </summary>
            /// <param name="point">经纬度坐标</param>
            /// <param name="L0">中央经线经度</param>
            /// <returns>高斯平面坐标</returns>
            public _2D_Point GaussPositive(Geo2Point point, Angle L0)
            {
                double X = Auxiliary.Get_X(ep.data.a, ep.data.e_2, point.Br),
                       N = Auxiliary.GetN(ep.data.a, ep.data.e_2, point.Br),
                       sinB = Math.Sin(point.Br),
                       cosB = Math.Cos(point.Br),
                       cosB_3=cosB*cosB*cosB,
                       cosB_5=cosB_3*cosB*cosB,
                       t = Auxiliary.Get_t(point.Br),
                       t_2 = t * t,
                       t_4 = t_2 * t_2,
                       yita_2 = Auxiliary.GetYita2(ep.data.e2_2, point.Br),
                       yita_4 = yita_2 * yita_2,
                       l = (point.L -L0).ChangeToRad(),
                       l_2=l*l,
                       l_3=l_2*l,
                       l_4=l_3*l;

                double x = X + N * sinB * cosB * l_2 / 2 +
                         N * sinB * cosB_3 * (5 - t_2 + 9 * yita_2 + 4 * yita_4) * l_4 / 24 +
                         N * sinB * cosB_5 * (61 - 58 * t_2 + t_4) * l_2 * l_4 / 720,
                       y = N * cosB * l + N * cosB_3 * (1 - t_2 + yita_2) * l_3 / 6 +
                         N * cosB_5 * (5 - 18 * t_2 + t_4 + 14 * yita_2 - 58 * yita_2 * t_2) * l_4 * l / 120;
                return new _2D_Point(x, y);
            }

            /// <summary>
            /// 高斯投影反算
            /// </summary>
            /// <param name="point">高斯平面坐标</param>
            /// <param name="L0">中央经线经度</param>
            /// <returns>经纬度坐标</returns>
            public Geo2Point GaussInverse(_2D_Point point,Angle L0)
            {
                double Bf = Auxiliary.Get_Bf(ep.data.a, ep.data.e_2, point.X),
                       Mf = Auxiliary.GetM(ep.data.a, ep.data.e_2, Bf),
                       Nf = Auxiliary.GetN(ep.data.a, ep.data.e_2, Bf),
                       Nf_2=Nf*Nf,
                       Nf_3=Nf*Nf_2,
                       tf = Auxiliary.Get_t(Bf),
                       tf_2 = tf * tf,
                       yitaf_2 = Auxiliary.GetYita2(ep.data.e2_2, Bf),
                       y=point.Y,
                       y_2=y*y,
                       y_3=y*y_2,
                       cosBf=Math.Cos(Bf);

                double B = Bf - tf * y_2 / 2 / Mf / Nf +
                       tf * (5 + 3 * tf_2 + yitaf_2 - 9 * yitaf_2 * tf_2) * y_2 * y_2 / 24 / Mf / Nf_3 -
                       tf * (61 + 90 * tf_2 + 45 * tf_2 * tf_2) * y_3 * y_3 / 720 / Mf / Nf_3 / Nf_2,

                       l = y / Nf / cosBf - (1 + 2 * tf_2 + yitaf_2) * y_3 / 6 / Nf_3 / cosBf +
                         (5 + 28 * tf_2 + 24 * tf_2 * tf_2 + 6 * yitaf_2 + 8 * yitaf_2 * tf_2) *
                         y_3 * y_2 / 120 / Nf_2 / Nf_3 / cosBf;
                return new Geo2Point(Angle.ChangeToAngle(B), L0 + Angle.ChangeToAngle(l));
            }

            /// <summary>
            /// 高斯邻带换算
            /// </summary>
            /// <param name="point">高斯平面上的点</param>
            /// <param name="L0">原中央经线经度</param>
            /// <param name="L1">新带中央经线经度</param>
            /// <returns>新带的高斯平面坐标</returns>
            public _2D_Point GaussBandConvert(_2D_Point point,Angle L0,Angle L1)
            {
                var bl = GaussInverse(point, L0);
                return GaussPositive(bl,L1);
            }

            /// <summary>
            /// 同一投影带上的点集的高斯邻带换算
            /// </summary>
            /// <param name="points">高斯平面上的点集</param>
            /// <param name="L0">原中央经线经度</param>
            /// <param name="L1">新带中央经线经度</param>
            /// <returns>新带的高斯平面坐标点集</returns>
            public List<_2D_Point> GaussBandConvert(List<_2D_Point> points,Angle L0,Angle L1)
            {
                List<_2D_Point> point2 = new List<_2D_Point>();
                points.ForEach(p =>point2.Add( GaussBandConvert(p, L0, L1)));
                return point2;
            }

            /// <summary>
            /// 批量高斯投影正算
            /// </summary>
            /// <param name="points">经纬度坐标表</param>
            /// <param name="L0">中央经线经度</param>
            /// <returns>高斯平面坐标表</returns>
            public List<_2D_Point> GaussPositive(List<Geo2Point> points, Angle L0)
            {
                List<_2D_Point> point2 = new List<_2D_Point>();
                points.ForEach(p => point2.Add(GaussPositive(p, L0)));
                return point2;
            }

            /// <summary>
            /// 批量高斯投影反算
            /// </summary>
            /// <param name="points">高斯平面坐标表</param>
            /// <param name="L0">中央经线经度</param>
            /// <returns>经纬度坐标表</returns>
            public List<Geo2Point> GaussInverse(List<_2D_Point> points,Angle L0)
            {
                List<Geo2Point> point2 = new List<Geo2Point>();
                points.ForEach(p => point2.Add(GaussInverse(p, L0)));
                return point2;
            }


        }
    }
}
