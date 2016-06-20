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
        /// 大地元素类
        /// </summary>
        public class GeodeticElement
        {
            /// <summary>
            /// 起点大地纬度
            /// </summary>
            public Angle B1 { set; get; }
            /// <summary>
            /// 起点大地经度
            /// </summary>
            public Angle L1 { set; get; }
            /// <summary>
            /// 终点大地纬度
            /// </summary>
            public Angle B2 { set; get; }
            /// <summary>
            /// 终点大地经度
            /// </summary>
            public Angle L2 { set; get; }
            /// <summary>
            /// 正方位角
            /// </summary>
            public Angle A12 { set; get; }
            /// <summary>
            /// 反方位角
            /// </summary>
            public Angle A21 { set; get; }
            /// <summary>
            /// 大地线长度
            /// </summary>
            public double S { set; get; }
            /// <summary>
            /// 弧度表示的起点纬度(用于计算)
            /// </summary>
            public double Br1 { set; get; }
            /// <summary>
            /// 弧度表示的起点经度(用于计算)
            /// </summary>
            public double Lr1 { set; get; }
            /// <summary>
            /// 弧度表示的终点纬度(用于计算)
            /// </summary>
            public double Br2 { set; get; }
            /// <summary>
            /// 弧度表示的终点点经度(用于计算)
            /// </summary>
            public double Lr2 { set; get; }
            /// <summary>
            /// 弧度表示的正方位角(用于计算)
            /// </summary>
            public double Ar12 { set; get; }
            /// <summary>
            /// 弧度表示的反方位角(用于计算)
            /// </summary>
            public double Ar21 { set; get; }
            /// <summary>
            /// 大地线长度(用于计算)
            /// </summary>
            public double Sr { set; get; }
            /// <summary>
            /// 构造一个默认实例
            /// </summary>
            public GeodeticElement()
            {
                B1 = new Angle();
                L1 = new Angle();
                B2 = new Angle();
                L2 = new Angle();
            }

            /// <summary>
            /// 通过起点经纬度、大地线长度和方位角构造
            /// </summary>
            /// <param name="B1">大地纬度</param>
            /// <param name="L1">大地经度</param>
            /// <param name="A12">正方位角</param>
            /// <param name="S">大地线长度</param>
            public GeodeticElement(Angle B1, Angle L1, Angle A12, double S)
            {
                this.B1 = B1;
                this.L1 = L1;
                this.S = Sr = S;
                this.A12 = A12;
                Br1 = B1.ChangeToRad();
                Lr1 = L1.ChangeToRad();
                Ar12 = A12.ChangeToRad();
            }

            /// <summary>
            /// 通过起点经纬度、方位角和大地线长度构造
            /// </summary>
            /// <param name="bl_mode">角度表示形式，true代表ddffmm.mmm,false代表弧度</param>
            /// <param name="B1">起点大地纬度(ddffmm.mmm或弧度)</param>
            /// <param name="L1">起点大地经度(ddffmm.mmm或弧度)</param>
            /// <param name="A12">正方位角(ddffmm.mmm或弧度)</param>
            /// <param name="S">大地线长度</param>
            public GeodeticElement(bool bl_mode, double B1, double L1, double A12, double S)
            {
                if (bl_mode)
                {
                    this.B1 = new Angle(B1);
                    this.L1 = new Angle(L1);
                    this.A12 = new Angle(A12);
                    this.S = Sr = S;
                    Br1 = this.B1.ChangeToRad();
                    Lr1 = this.L1.ChangeToRad();
                    Ar12 = this.A12.ChangeToRad();
                }
                else
                {
                    Br1 = B1; Lr1 = L1; Ar12 = A12;
                    this.B1 = Angle.ChangeToAngle(B1);
                    this.L1 = Angle.ChangeToAngle(L1);
                    this.A12 = Angle.ChangeToAngle(A12);
                    this.S = Sr = S;
                }
            }

            /// <summary>
            /// 通过两点经纬度构造一个实例
            /// </summary>
            /// <param name="B1">起点纬度</param>
            /// <param name="L1">起点经度</param>
            /// <param name="B2">终点纬度</param>
            /// <param name="L2">终点经度</param>
            public GeodeticElement(Angle B1, Angle L1, Angle B2, Angle L2)
            {
                this.B1 = B1;
                this.L1 = L1;
                this.B2 = B2;
                this.L2 = L2;
                Br1 = B1.ChangeToRad();
                Lr1 = L1.ChangeToRad();
                Br2 = B2.ChangeToRad();
                Lr2 = L2.ChangeToRad();
            }

            /// <summary>
            /// 通过弧度表示的两点经纬度构造实例(通过mode确定参数形式)
            /// </summary>
            /// <param name="bl_mode">角度表示形式，true代表ddffmm.mmm,false代表弧度</param>
            /// <param name="B1">起点大地纬度(ddffmm.mmm或弧度)</param>
            /// <param name="L1">起点大地经度(ddffmm.mmm或弧度)</param>
            /// <param name="B2">终点大地纬度(ddffmm.mmm或弧度)</param>
            /// <param name="L2">终点大地经度(ddffmm.mmm或弧度)</param>
            public GeodeticElement(double B1, double L1, double B2, double L2, bool bl_mode)
            {
                if (bl_mode)
                {
                    this.B1 = new Angle(B1);
                    this.L1 = new Angle(L1);
                    this.B2 = new Angle(B2);
                    this.L2 = new Angle(L2);
                    Br1 = this.B1.ChangeToRad();
                    Lr1 = this.L1.ChangeToRad();
                    Br2 = this.B2.ChangeToRad();
                    Lr2 = this.L2.ChangeToRad();
                }
                else
                {
                    Br1 = B1; Lr1 = L1; Br2 = B2; Lr2 = L2;
                    this.B1 = Angle.ChangeToAngle(B1);
                    this.L1 = Angle.ChangeToAngle(L1);
                    this.B2 = Angle.ChangeToAngle(B2);
                    this.L2 = Angle.ChangeToAngle(L2);
                }
            }

            /// <summary>
            /// 设置精度
            /// </summary>
            /// <param name="n1">角度保留到小数点后的位数</param>
            /// <param name="n2">方位角保留到小数点后的位数</param>
            /// <param name="n3">大地线长度保留到小数点后的位数</param>
            public void SetAccurate(int n1, int n2,int n3)
            {
                B1.Set_Accurate(n1);
                L1.Set_Accurate(n1);
                A12.Set_Accurate(n2);
                B2.Set_Accurate(n1);
                L2.Set_Accurate(n1);
                A21.Set_Accurate(n2);
                S = Methods.Set_Accurate(S, n3);
            }

            /// <summary>
            /// 转换成B1,B2,L1,L2,A12,A21,S
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return B1 + "," + L1 + "," + B2 + "," + L2 + "," + A12 + "," + A21 + "," + S;
            }
        }
    }
}
