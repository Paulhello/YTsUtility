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
        /// 辅助量计算类
        /// </summary>
        public static class Auxiliary
        {
            /// <summary>
            /// 计算辅助量W
            /// </summary>
            /// <param name="e_2">椭球元素e^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double GetW(double e_2, double B)
            {
                return Math.Sqrt(1 - e_2 * Math.Sin(B) * Math.Sin(B));
            }

            /// <summary>
            /// 计算辅助量V
            /// </summary>
            /// <param name="e2_2">椭球元素e'^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double GetV(double e2_2, double B)
            {
                return Math.Sqrt(1 + e2_2 * Math.Cos(B) * Math.Cos(B));
            }

            /// <summary>
            /// 计算N
            /// </summary>
            /// <param name="a">椭球元素a</param>
            /// <param name="e_2">椭球元素e^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double GetN(double a,double e_2,double B)
            {
                return a / GetW(e_2, B);
            }

            /// <summary>
            /// 计算M
            /// </summary>
            /// <param name="a">椭球元素a</param>
            /// <param name="e_2">椭球元素e^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double GetM(double a,double e_2,double B)
            {
                return a * (1 - e_2) * Math.Pow(1 - e_2 * Math.Sin(B) * Math.Sin(B), -1.5);
            }
            /// <summary>
            /// 计算η的平方
            /// </summary>
            /// <param name="e2_2">椭球元素e'^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double GetYita2(double e2_2,double B)
            {
                return e2_2 * Math.Cos(B) * Math.Cos(B);
            }

            /// <summary>
            /// 计算t,即tan(B)
            /// </summary>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns></returns>
            public static double Get_t(double B)
            {
                return Math.Tan(B);
            }

            /// <summary>
            /// 求子午线弧长
            /// </summary>
            /// <param name="a">椭球元素a</param>
            /// <param name="e_2">椭球元素e^2</param>
            /// <param name="B">大地纬度(弧度)</param>
            /// <returns>赤道起的子午线弧长</returns>
            public static double Get_X(double a,double e_2,double B)
            {
                double m0 = a * (1 - e_2),
                       m2 = e_2 * m0 * 3/ 2,
                       m4 = e_2 * m2 * 5/ 4,
                       m6 = e_2 * m4 * 7/ 6,
                       m8 = e_2 * m6 * 9/ 8 ;
                double a0 = m0 + m2 / 2 + m4 * 3 / 8 + m6 * 5 / 16 + m8 * 35 / 128,
                       a2 = m2 / 2 + m4 / 2 + m6 * 15 / 32 + m8 * 7 / 16,
                       a4 = m4 / 8 + m6 * 3 / 16 + m8 * 7 / 32,
                       a6 = m6 / 32 + m8 / 16,
                       a8 = m8 / 128;
                double sinB=Math.Sin(B);
                return a0 * B - sinB*Math.Cos(B)*
                       ((a2-a4+a6)+(2*a4-a6*16/3)*sinB*sinB+(a6*16/3)*sinB*sinB*sinB*sinB);

            }

            /// <summary>
            /// 获取底点纬度
            /// </summary>
            /// <param name="a">椭球元素a</param>
            /// <param name="e_2">椭球元素e^2</param>
            /// <param name="x">高斯投影横坐标</param>
            /// <param name="mode">0表示迭代解法，其他表示直接解法</param>
            /// <returns></returns>
            public static double Get_Bf(double a,double e_2,double x,int mode=0)
            {
                double m0 = a * (1 - e_2),
                       m2 = e_2 * m0 * 3 / 2,
                       m4 = e_2 * m2 * 5 / 4,
                       m6 = e_2 * m4 * 7 / 6,
                       m8 = e_2 * m6 * 9 / 8;
                double a0 = m0 + m2 / 2 + m4 * 3 / 8 + m6 * 5 / 16 + m8 * 35 / 128,
                       a2 = m2 / 2 + m4 / 2 + m6 * 15 / 32 + m8 * 7 / 16,
                       a4 = m4 / 8 + m6 * 3 / 16 + m8 * 7 / 32,
                       a6 = m6 / 32 + m8 / 16,
                       a8 = m8 / 128;
                
                //if(mode==0)
                //{
                    double Bf1 = x / a0, Bf2 = Bf1;
                    do
                    {
                        Bf1 = Bf2;
                        double sinBf = Math.Sin(Bf1);
                        double FB = -sinBf * Math.Cos(Bf1) *
                           ((a2 - a4 + a6) + (2 * a4 - a6 * 16 / 3) * sinBf * sinBf + (a6 * 16 / 3) * sinBf * sinBf * sinBf * sinBf);
                        Bf2 = (x - FB) / a0;
                    } while (Math.Abs(Bf1 - Bf2) * Methods.ρ > 0.0001);
                    return Bf2;
                //}
                //else
                //{
                //    double P2 = -a2 / 2,
                //           P4 = a4 / 4,
                //           P6 = -a6 / 6,
                //           q2 = -P2 - P2 * P4 + P2 * P2 * P2 / 2,
                //           q4 = -P4 + P2 * P2 - 2 * P2 * P6 + 4 * P2 * P2 * P4,
                //           q6 = -P6 + 3 * P2 * P4 - 3 * P2 * P2 * P2 / 2;
                    
                //}
                
            }

            /// <summary>
            /// 通过x,y坐标获取经度
            /// </summary>
            /// <param name="X">大地X坐标</param>
            /// <param name="Y">大地Y坐标</param>
            /// <returns>弧度表示的经度</returns>
            public static double Get_L(double X,double Y)
            {
                return Math.Atan2(Y, X);
            }
        }

        /// <summary>
        /// 大地解算类
        /// </summary>
        public class GeodeticSolution
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
            public GeodeticSolution(Ellipse elp,double a=0,double b=0)
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
            //public GeodeticSolution(double a, double b)
            //{
            //    ep = new Ellipsoid(a, b);
            //}

            /// <summary>
            /// 高斯平均引数法正算
            /// </summary>
            /// <param name="ge">已知条件</param>
            public void GaussPositive(GeodeticElement ge)
            {
               double e2_2 = ep.data.e2_2,
                      c = ep.data.c,
                      Vm = Auxiliary.GetV(e2_2, ge.Br1),
                      Mm = c / Vm / Vm / Vm,
                      Nm = c / Vm,
                      delta_B0 = Methods.ρ / Mm * ge.Sr * Math.Cos(ge.Ar12),
                      delta_L0 = Methods.ρ / Nm * ge.Sr * Math.Sin(ge.Ar12) / Math.Cos(ge.Br1),
                      delta_A0 = delta_L0 * Math.Sin(ge.Br1),
                      delta_B1=delta_B0,
                      delta_L1=delta_L0,
                      delta_A1=delta_A0,
                      Bm,
                      Am,
                      tm,
                      yita_2;
               #region 迭代法求解
               do
               {
                   delta_B0=delta_B1;
                   delta_L0=delta_L1;
                   delta_A0=delta_A1;
                   Bm=ge.Br1+delta_B0/2/Methods.ρ;
                   Am=ge.Ar12+delta_A0/2/Methods.ρ;
                   Vm = Auxiliary.GetV(e2_2, Bm);
                   Nm = c / Vm;
                   tm=Auxiliary.Get_t(Bm);
                   yita_2=Auxiliary.GetYita2(e2_2,Bm);

                   delta_B1 = Vm * Vm / Nm * Methods.ρ * ge.Sr * Math.Cos(Am) *
                           (1 + ge.Sr * ge.Sr / 24 / Nm / Nm *
                              (Math.Sin(Am) * Math.Sin(Am) * (2 + 3 * tm * tm + 3 * yita_2*tm*tm) +
                                3 * yita_2 * Math.Cos(Am) * Math.Cos(Am) * (-1 +tm*tm- yita_2 - 4 * yita_2 * tm * tm)));

                   delta_L1 = Methods.ρ / Nm * ge.Sr * Math.Sin(Am) /Math.Cos(Bm)*
                           (1 + ge.Sr * ge.Sr / 24 / Nm / Nm *
                              (Math.Sin(Am) * Math.Sin(Am)  * tm * tm -
                                 Math.Cos(Am) * Math.Cos(Am) * (1 + yita_2 - 9 * yita_2 * tm * tm+yita_2*yita_2)));

                   delta_A1 = Methods.ρ / Nm * ge.Sr * Math.Sin(Am) *tm*
                           (1 + ge.Sr * ge.Sr / 24 / Nm / Nm *
                              (Math.Cos(Am) * Math.Cos(Am) * (2 + 7*yita_2 + 9 * yita_2 * tm * tm+5*yita_2*yita_2) +
                                 Math.Sin(Am) * Math.Sin(Am) * (2 + tm*tm + 2 * yita_2)));
      
               }while(Math.Abs(delta_B1-delta_B0)>0.0001||
                      Math.Abs(delta_L1-delta_L0)>0.0001||
                      Math.Abs(delta_A1-delta_A0)>0.0001);
               #endregion

               ge.Br2 = ge.Br1 + delta_B1 / Methods.ρ;
               ge.B2 = Angle.ChangeToAngle(ge.Br2);
               ge.Lr2 = ge.Lr1 + delta_L1 / Methods.ρ;
               ge.L2 = Angle.ChangeToAngle(ge.Lr2);
               if(ge.Ar12+delta_A1/Methods.ρ>=Math.PI)
               {
                   ge.Ar21 = ge.Ar12 + delta_A1 / Methods.ρ - Math.PI;
               }
                else
                   ge.Ar21 = ge.Ar12 + delta_A1 / Methods.ρ + Math.PI;
               ge.A21 = Angle.ChangeToAngle(ge.Ar21);
            }

            /// <summary>
            /// 贝塞尔正算
            /// </summary>
            /// <param name="ge">已知元素</param>
            public void BesselPositive(GeodeticElement ge)
            {
                double W1 = Auxiliary.GetW(ep.data.e_2, ge.Br1),
                       sin_u1 = Math.Sin(ge.Br1) * Math.Sqrt(1 - ep.data.e_2) / W1,
                       cos_u1 = Math.Cos(ge.Br1) / W1,
                       sinA1=Math.Sin(ge.Ar12),
                       cosA1=Math.Cos(ge.Ar12);

                double sinA0 = cos_u1 * sinA1,
                       cot_xigema1 = cos_u1 * cosA1 / sin_u1,
                       sin_2xigema1 = 2 * cot_xigema1 / (cot_xigema1 * cot_xigema1 + 1),
                       cos_2xigema1 = (cot_xigema1 * cot_xigema1 - 1) / (cot_xigema1 * cot_xigema1 + 1);

                double cos2A0 = 1 - sinA0 * sinA0,
                       k_2 = ep.data.e2_2 * cos2A0,
                       k_4 = k_2 * k_2,
                       k_6 = k_2 * k_4,
                       b = ep.data.b,
                       e_2 = ep.data.e_2,
                       e_4 = e_2 * e_2,
                       e_6 = e_2 * e_4,
                       A = b * (1 + k_2 / 4 - 3 * k_4 / 64 + 5 * k_6 / 256),
                       B = b * (k_2 / 8 - k_4 / 32 + 15 * k_6 / 1024),
                       C = b * (k_4 / 128 - 3 * k_6 / 512),
                       Alph = e_2 / 2 + e_4 / 8 + e_6 / 8 - (e_4 / 16 + e_6 / 16) * cos2A0 + 3 * e_6 / 128 * cos2A0 * cos2A0,
                       Beta = (e_4 / 32 + e_6 / 32) * cos2A0 - e_6 / 64 * cos2A0 * cos2A0;

                double xigema0 = (ge.Sr - (B + C * cos_2xigema1) * sin_2xigema1) / A,
                       cos_2xigema0 = Math.Cos(2 * xigema0),
                       sin_2xigema0 = Math.Sin(2 * xigema0),
                       sin_2xigema10 = sin_2xigema1 * cos_2xigema0 + cos_2xigema1 * sin_2xigema0,
                       cos_2xigema10 = cos_2xigema1 * cos_2xigema0 - sin_2xigema0 * sin_2xigema1,
                       xigema = xigema0 + (B + 5 * C * cos_2xigema10) * sin_2xigema10 / A;

                double delta = (Alph * xigema + Beta * (sin_2xigema1 * Math.Cos(2 * xigema) + cos_2xigema1 * Math.Sin(2 * xigema) - sin_2xigema1)) * sinA0;

                double sin_u2 = sin_u1 * Math.Cos(xigema) + cos_u1 * cosA1 * Math.Sin(xigema),
                       B2 = Math.Atan(sin_u2 / Math.Sqrt((1 - e_2) * (1 - sin_u2 * sin_u2))),
                       lamda = Math.Atan2(sinA1, (cos_u1 * Math.Cos(xigema) - sin_u1 * Math.Sin(xigema) * cosA1) / Math.Sin(xigema)),
                       L2 = ge.Lr1 + lamda - delta;
                var A2 = _2D_Point.Get_PAngle(new _2D_Point(0, 0),
                   new _2D_Point(-(Math.Cos(xigema) * cos_u1 * cosA1 - sin_u1 * Math.Sin(xigema)) / cos_u1, -sinA1));

                ge.Br2 = B2; ge.Lr2 = L2;
                ge.B2 = Angle.ChangeToAngle(B2); ge.L2 = Angle.ChangeToAngle(L2);
                ge.Ar21 = A2.ChangeToRad(); ge.A21 =A2 ;
            }

            /// <summary>
            /// 高斯引数法反算
            /// </summary>
            /// <param name="ge">已知元素</param>
            public void GaussInverse(GeodeticElement ge)
            {
                double Bm = Methods.Average(ge.Br1, ge.Br2),
                       delta_B = ge.Br2 - ge.Br1,
                       delta_L = ge.Lr2 - ge.Lr1,
                       Vm = Auxiliary.GetV(ep.data.e2_2, Bm),
                       Nm = Auxiliary.GetN(ep.data.a, ep.data.e_2, Bm),
                       tm = Auxiliary.Get_t(Bm),
                       tm2=tm*tm,
                       yita_2 = Auxiliary.GetYita2(ep.data.e2_2, Bm),
                       cos_Bm=Math.Cos(Bm),
                       cos_Bm2=cos_Bm*cos_Bm;

                double r01 = Nm * cos_Bm,
                       r21 = r01 / 24 / Math.Pow(Vm, 4) * (1 + yita_2 - 9 * yita_2 * tm2 + yita_2 * yita_2),
                       r03 = -r01 / 24 * cos_Bm2 * tm2;

                double s10 = Nm / Vm / Vm,
                       s12 = -s10 * cos_Bm2 / 24 * (2 + 3 * tm2 + 2 * yita_2*tm2),
                       s30 = Nm /Math.Pow(Vm,6)/ 8 * (yita_2- tm2 * yita_2+yita_2*yita_2);

                double t01 = tm * cos_Bm,
                       t21 = t01 / 24 / Math.Pow(Vm, 4) * (2 + 7 * yita_2 +9*tm2*yita_2 + 5 * yita_2 * yita_2),
                       t03 = t01 * cos_Bm2 / 24 * (2+tm2 +2* yita_2);

                double S_sinAm = r01 * delta_L + r21 * delta_B * delta_B * delta_L + r03 * Math.Pow(delta_L, 3),
                       S_cosAm = s10 * delta_B + s12 * delta_B * delta_L * delta_L + s30 * Math.Pow(delta_B, 3),
                       delta_A = t01 * delta_L + t21 * delta_B * delta_B * delta_L + t03 * Math.Pow(delta_L, 3);
                       //S_sinAm1=S_sinAm,S_cosAm1=S_cosAm,delta_A1=delta_A;

                double Am = _2D_Point.Get_PAngle(new _2D_Point(0, 0), new _2D_Point(S_cosAm, S_sinAm)).ChangeToRad();
                       //Am1

                //do
                //{
                //    S_sinAm = S_sinAm1;
                //    S_cosAm = S_cosAm1;
                //    Am = Am1;

                //    S_sinAm1 = delta_L * Nm * cos_Bm - S_sinAm / 24 / Nm / Nm * (S_sinAm * S_sinAm * tm2 - S_cosAm * S_cosAm * (1 + yita_2 - 9 * yita_2 * tm2 + yita_2 * yita_2));
                //    S_cosAm1 = delta_B * Nm / Vm / Vm - S_cosAm1 / 24 / Nm / Nm * (S_sinAm1 * S_sinAm1 * (2 + 3 * tm2 + 2 * yita_2 * tm2) + 3 * yita_2 * S_cosAm * S_cosAm1 * (1 + yita_2 - tm2 + 4 * tm2 * yita_2));
                //    Am1 = _2D_Point.Get_PAngle(new _2D_Point(0, 0), new _2D_Point(S_cosAm1, S_sinAm1)).ChangeToRad();
                //} while (Math.Abs(Angle.ChangeToAngle(Am1 - Am).Miao) > 0.0001);

                ge.S = ge.Sr = S_sinAm / Math.Sin(Am);
                ge.Ar12 = Am - delta_A / 2;
                ge.A12 = Angle.ChangeToAngle(ge.Ar12);
                if (Am + delta_A >= Math.PI)
                {
                    ge.Ar21 = Am + delta_A/2 - Math.PI;
                }
                else
                    ge.Ar21 = Am + delta_A/2 + Math.PI;
                ge.A21 = Angle.ChangeToAngle(ge.Ar21);
            }

            /// <summary>
            /// 贝塞尔反算
            /// </summary>
            /// <param name="ge">已知大地元素</param>
            public void BesselInverse(GeodeticElement ge)
            {
                double W1 = Auxiliary.GetW(ep.data.e_2, ge.Br1),
                       W2 = Auxiliary.GetW(ep.data.e_2, ge.Br2),
                       sin_u1 = Math.Sin(ge.Br1) * Math.Sqrt(1 - ep.data.e_2) / W1,
                       sin_u2 = Math.Sin(ge.Br2) * Math.Sqrt(1 - ep.data.e_2) / W2,
                       cos_u1 = Math.Cos(ge.Br1) / W1,
                       cos_u2 = Math.Cos(ge.Br2) / W2,
                       L = ge.Lr2 - ge.Lr1,
                       a1 = sin_u1 * sin_u2,
                       a2 = cos_u1 * cos_u2,
                       b1 = cos_u1 * sin_u2,
                       b2 = sin_u1 * cos_u2,
                       delta0=0,
                       delta1=delta0,
                       lamda,
                       A1,
                       xigema,
                       sin_xigema,
                       cos_xigema,
                       sinA0,
                       cos2A0,
                       x;
                do
                {
                    delta0 = delta1;
                    lamda = L + delta0;
                    
                    double sinlamda = Math.Sin(lamda),
                            coslamda = Math.Cos(lamda);

                    double p = cos_u2 * sinlamda,
                           q = b1 - b2 * coslamda;

                    
                    A1 = _2D_Point.Get_PAngle(_2D_Point.Origin, new _2D_Point(q, p)).ChangeToRad();

                    sin_xigema = p * Math.Sin(A1) + q * Math.Cos(A1);
                    cos_xigema = a1 + a2 * Math.Cos(lamda);
                    xigema = Math.Atan(sin_xigema / cos_xigema);
                    if (cos_xigema < 0) xigema = Math.PI - Math.Abs(xigema);
                    else xigema = Math.Abs(xigema);
                    sinA0 = cos_u1 * Math.Sin(A1);

                    double sinA1 = Math.Sin(A1),
                        cosA1 = Math.Cos(A1);

                    cos2A0 = 1 - sinA0 * sinA0;
                    double e_2 = ep.data.e_2,
                           e_4 = e_2 * e_2,
                           e_6 = e_2 * e_4,
                           Alph = e_2 / 2 + e_4 / 8 + e_6 / 8 - (e_4 / 16 + e_6 / 16) * cos2A0 + 3 * e_6 / 128 * cos2A0 * cos2A0,
                           Beta = (e_4 / 32 + e_6 / 32) * cos2A0 - e_6 / 64 * cos2A0 * cos2A0,
                           Beta1 = 2 * Beta/cos2A0;

                    x = 2 * a1 - cos2A0 * cos_xigema;
                    delta1 =(Alph * xigema - Beta1*x*sin_xigema) * sinA0;
                } while (Methods.ρ * Math.Abs(delta0 - delta1) > 0.0001);
                double k_2 = ep.data.e2_2 * cos2A0,
                       k_4 = k_2 * k_2,
                       k_6 = k_2 * k_4,
                       b = ep.data.b,
                       A = b * (1 + k_2 / 4 - 3 * k_4 / 64 + 5 * k_6 / 256),
                       B = b * (k_2 / 8 - k_4 / 32 + 15 * k_6 / 1024),
                       C = b * (k_4 / 128 - 3 * k_6 / 512),
                       B_pp = 2 * B / cos2A0,
                       C_pp = 2 * C / cos2A0 / cos2A0;

                double y = (cos2A0 * cos2A0 - 2 * x * x) * cos_xigema,
                       S = A * xigema + (B_pp*x + C_pp * y) *sin_xigema,
                       p1 = cos_u1 * Math.Sin(lamda),
                       q1 = b1 * Math.Cos(lamda) - b2;
                var A21 = _2D_Point.Get_PAngle(_2D_Point.Origin, new _2D_Point(-q1,- p1));

                ge.S = ge.Sr = S;
                ge.A21 = A21; ge.Ar21 = A21.ChangeToRad();
                ge.Ar12 = A1; ge.A12 = Angle.ChangeToAngle(A1);
            }

            /// <summary>
            /// 贝塞尔反算1
            /// </summary>
            /// <param name="ge"></param>
            public void BesselInverse1(GeodeticElement ge)
            {
                double W1 = Auxiliary.GetW(ep.data.e_2, ge.Br1),
                       W2 = Auxiliary.GetW(ep.data.e_2, ge.Br2),
                       sin_u1 = Math.Sin(ge.Br1) * Math.Sqrt(1 - ep.data.e_2) / W1,
                       sin_u2 = Math.Sin(ge.Br2) * Math.Sqrt(1 - ep.data.e_2) / W2,
                       cos_u1 = Math.Cos(ge.Br1) / W1,
                       cos_u2 = Math.Cos(ge.Br2) / W2,
                       L = ge.Lr2 - ge.Lr1,
                       a1 = sin_u1 * sin_u2,
                       a2 = cos_u1 * cos_u2,
                       b1 = cos_u1 * sin_u2,
                       b2 = sin_u1 * cos_u2,
                       delta0=0,
                       delta1=delta0,
                       lamda,
                       A1,
                       xigema,xigema1,xigema2,
                       sin_xigema,
                       cos_xigema,
                       sinA0,
                       cos2A0;
                do
                {
                    delta0 = delta1;
                    lamda = L + delta0;

                    double sinlamda = Math.Sin(lamda),
                            coslamda = Math.Cos(lamda);

                    double p = cos_u2 * sinlamda,
                            q = b1 - b2 * coslamda;


                    A1 = _2D_Point.Get_PAngle(_2D_Point.Origin, new _2D_Point(q, p)).ChangeToRad();

                    sin_xigema = p * Math.Sin(A1) + q * Math.Cos(A1);
                    cos_xigema = a1 + a2 * Math.Cos(lamda);
                    xigema = Math.Atan(sin_xigema / cos_xigema);
                    if (cos_xigema < 0) xigema = Math.PI - Math.Abs(xigema);
                    else xigema = Math.Abs(xigema);
                    sinA0 = cos_u1 * Math.Sin(A1);

                    double sinA1 = Math.Sin(A1),
                           cosA1 = Math.Cos(A1),
                           tan_xigema1 = sin_u1 / cos_u1 / cosA1;
                    xigema1 = Math.Atan(tan_xigema1);
                    xigema2 = xigema + xigema1;

                    cos2A0 = 1 - sinA0 * sinA0;
                    double e_2 = ep.data.e_2,
                            e_4 = e_2 * e_2,
                            e_6 = e_2 * e_4,
                            Alph = e_2 / 2 + e_4 / 8 + e_6 / 8 - (e_4 / 16 + e_6 / 16) * cos2A0 + 3 * e_6 / 128 * cos2A0 * cos2A0,
                            Beta = (e_4 / 32 + e_6 / 32) * cos2A0 - e_6 / 64 * cos2A0 * cos2A0,
                            Beta1 = 2 * Beta / cos2A0;
                    delta1 = sinA0 * (Alph * xigema + Beta * (Math.Sin(2 * xigema2) - Math.Sin(2 * xigema1)));

                } while (Math.Abs(delta0 - delta1) * Methods.ρ > 0.0001);

                double k_2 = ep.data.e2_2 * cos2A0,
                       k_4 = k_2 * k_2,
                       k_6 = k_2 * k_4,
                       b = ep.data.b,
                       A = b * (1 + k_2 / 4 - 3 * k_4 / 64 + 5 * k_6 / 256),
                       B = b * (k_2 / 8 - k_4 / 32 + 15 * k_6 / 1024),
                       C = b * (k_4 / 128 - 3 * k_6 / 512),
                       B_pp = 2 * B / cos2A0,
                       C_pp = 2 * C / cos2A0 / cos2A0;

                double S = A * xigema + (B+C*Math.Cos(2*xigema1)) * Math.Sin(2*xigema1)-Math.Sin(2*xigema2)*(B+C*Math.Cos(2*xigema2)),
                       p1 = cos_u1 * Math.Sin(lamda),
                       q1 = b1 * Math.Cos(lamda) - b2;
                var A21 = _2D_Point.Get_PAngle(_2D_Point.Origin, new _2D_Point(-q1, -p1));

                ge.S = ge.Sr = S;
                ge.A21 = A21; ge.Ar21 = A21.ChangeToRad();
                ge.Ar12 = A1; ge.A12 = Angle.ChangeToAngle(A1);
            }

            /// <summary>
            /// 批量高斯正算
            /// </summary>
            /// <param name="ges">已知点表</param>
            public void GaussPositive(List<GeodeticElement> ges)
            {
                ges.ForEach(g => GaussPositive(g));
            }

            /// <summary>
            /// 批量贝塞尔正算
            /// </summary>
            /// <param name="ges">已知点表</param>
            public void BesselPositive(List<GeodeticElement> ges)
            {
                ges.ForEach(g => BesselPositive(g));
            }

            /// <summary>
            /// 批量高斯反算
            /// </summary>
            /// <param name="ges">已知点表</param>
            public void GaussInverse(List<GeodeticElement> ges)
            {
                ges.ForEach(g => GaussInverse(g));
            }

            /// <summary>
            /// 批量贝塞尔反算
            /// </summary>
            /// <param name="ges">已知点表</param>
            public void BesselInverse(List<GeodeticElement> ges)
            {
                ges.ForEach(g => BesselInverse(g));
            }
        }
    }
}
