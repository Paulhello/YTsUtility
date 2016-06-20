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
        /// 元素
        /// </summary>
        public struct ElpElement
        {
            /// <summary>
            /// 椭球元素
            /// </summary>
            public double a, b, c, e_2, e2_2, alph;
        }
        /// <summary>
        /// 椭球类
        /// </summary>
        public class Ellipsoid
        {
            /// <summary>
            /// 椭球元素
            /// </summary>
            public ElpElement data;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="a">椭球元素a</param>
            /// <param name="b">椭球元素b</param>
            public Ellipsoid(double a, double b)
            {
                data.a = a;
                data.b = b;
                data.c = a * a / b;
                data.alph = (a - b) / a;
                data.e_2 = Math.Sqrt(a * a - b * b) / a;
                data.e2_2 = Math.Sqrt(a * a - b * b) / a;
            }

            /// <summary>
            /// 无参构造函数，默认为克氏椭球
            /// </summary>
            public Ellipsoid()
            {
                Klinefelter_ellipsoid();
            }
            /// <summary>
            /// 克氏椭球元素
            /// </summary>
            public void Klinefelter_ellipsoid()
            {
                data =
                new ElpElement
                {
                    a = 6378245,
                    b = 6356863.0187730473,
                    c = 6399698.9017827110,
                    alph = 1 / 298.3,
                    e_2 = 0.006693421622966,
                    e2_2 = 0.006738525414683
                };
            }


            /// <summary>
            /// 1975国际椭球元素
            /// </summary>
            public void International_ellipsoid1975()
            {
                data =
                new ElpElement
                {
                    a = 6378140,
                    b = 6356755.2881575287,
                    c = 6399596.6519880105,
                    alph = 1 / 298.257,
                    e_2 = 0.006694384999588,
                    e2_2 = 0.006739501819473
                };
            }


            /// <summary>
            /// WGS-84椭球元素
            /// </summary>
            public void WGS84_ellipsoid()
            {
                data =
                new ElpElement
                {
                    a = 6378137,
                    b = 6356752.3142,
                    c = 6399593.6258,
                    alph = 1 / 298.257223563,
                    e_2 = 0.00669437999013,
                    e2_2 = 0.00673949674227
                };
            }


            /// <summary>
            /// 中国大地坐标系
            /// </summary>
            public void CGCS2000_ellipsoid()
            {
                data =
                new ElpElement
                {
                    a = 6378137,
                    b = 6356752.3141,
                    c = 6399593.6259,
                    alph = 1 / 298.257222,
                    e_2 = 0.00669438002290,
                    e2_2 = 0.00673949677548
                };
            }

        }
    }
}
