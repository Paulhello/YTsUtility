using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace YTsUtility
{
    namespace PhotoGramMetry
    {
        /// <summary>
        /// 物方坐标类
        /// </summary>
        public class ObjectData
        {
            /// <summary>
            /// 点名
            /// </summary>
            public string Name;
            /// <summary>
            /// 坐标
            /// </summary>
            public _3D_Point pos;
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="name">点名</param>
            /// <param name="point">坐标</param>
            public ObjectData(string name, _3D_Point point)
            {
                Name = name;
                pos = point;
            }
            /// <summary>
            /// 默认构造函数(点名为空字符，坐标为原点)
            /// </summary>
            public ObjectData()
            {
                pos = new _3D_Point();
            }

            /// <summary>
            /// 形如 name,pos
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Name + "," + pos;
            }

            /// <summary>
            /// 输出坐标表
            /// </summary>
            /// <param name="odList"></param>
            /// <param name="filepath"></param>
            public static void OutPut(List<ObjectData> odList, string filepath)
            {
                StringBuilder sb = new StringBuilder();
                foreach(var od in odList)
                {
                    sb.AppendLine(od.ToString());
                }
                File.WriteAllText(filepath, sb.ToString(), Encoding.Default);
            }
        }

        /// <summary>
        /// 像方坐标类
        /// </summary>
        public class ImageData
        {
            /// <summary>
            /// 点名
            /// </summary>
            public string Name;
            /// <summary>
            /// 像方坐标
            /// </summary>
            public _2D_Point pos = new _2D_Point();
            /// <summary>
            /// 形如 name,pos
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return Name + "," + pos;
            }
        }

        /// <summary>
        /// 外方位元素
        /// </summary>
        public class OutsideElement
        {
            /// <summary>
            /// 摄影中心坐标(线元素)
            /// </summary>
            public _3D_Point Spos = new _3D_Point();

            /// <summary>
            /// 角元素
            /// </summary>
            public double phi, omega, kappa;

            /// <summary>
            /// 转换为四元数形式???
            /// </summary>
            /// <returns></returns>
            public OutElement4 ToOutElem4()
            {
                OutElement4 oe = new OutElement4();
                oe.Spos = Spos.Clone();
                double cosp = Math.Cos(GetRad(phi) / 2),
                       sinp = Math.Sin(GetRad(phi) / 2),
                       coso = Math.Cos(GetRad(omega) / 2),
                       sino = Math.Sin(GetRad(omega) / 2),
                       cosk = Math.Cos(GetRad(kappa) / 2),
                       sink = Math.Sin(GetRad(kappa) / 2);
                oe.q2 = sinp * coso * cosk + cosp * sino * sink;
                oe.q1 = cosp * sino * cosk - sinp * coso * sink;
                oe.q3= cosp * coso * sink + sinp * sino * cosk;
                oe.q0 = cosp * coso * cosk - sinp * sino * sink;
                return oe;
            }

            /// <summary>
            /// 旋转矩阵
            /// </summary>
            /// <returns></returns>
            public Matrix GetR()
            {
                DMath sin = Math.Sin, cos = Math.Cos;
                double[,] R = new double[3, 3];
                R[0, 0] = cos(phi) * cos(kappa) - sin(phi) * sin(omega) * sin(kappa);
                R[0, 1] = -cos(phi) * sin(kappa) - sin(phi) * sin(omega) * cos(kappa);
                R[0, 2] = -sin(phi) * cos(omega);
                R[1, 0] = cos(omega) * sin(kappa);
                R[1, 1] = cos(omega) * cos(kappa);
                R[1, 2] = -sin(omega);
                R[2, 0] = sin(phi) * cos(kappa) + cos(phi) * sin(omega) * sin(kappa);
                R[2, 1] = -sin(phi) * sin(kappa) + cos(phi) * sin(omega) * cos(kappa);
                R[2, 2] = cos(phi) * cos(omega);
                return new Matrix(R, 3, 3);
            }

            private double GetRad(double rad)
            {
                return rad > Math.PI ? rad - 2 * Math.PI : rad;
            }
        }

        /// <summary>
        /// 四元数表示的外方位元素
        /// </summary>
        public class OutElement4
        {
            /// <summary>
            /// 线元素
            /// </summary>
            public _3D_Point Spos = new _3D_Point();
            /// <summary>
            /// 实部
            /// </summary>
            public double q0;
            /// <summary>
            /// 虚部1
            /// </summary>
            public double q1;
            /// <summary>
            /// 虚部2
            /// </summary>
            public double q2;
            /// <summary>
            /// 虚部3
            /// </summary>
            public double q3;

            /// <summary>
            /// 转换为欧拉角表示形式的外方元素
            /// </summary>
            /// <returns></returns>
            public OutsideElement ToOutElem3()
            {
                OutsideElement oe = new OutsideElement();
                oe.Spos = Spos.Clone();
                oe.omega = Math.Asin(-2 * (q2 * q3 - q0 * q1));
                if (oe.omega< -Math.PI)
                {
                    oe.omega += 2 * Math.PI;
                }
                oe.phi = Math.Atan2(-2 * (q1 * q3 + q0 * q2) * Math.Cos(oe.omega),
                                   (q0 * q0 - q1 * q1 - q2 * q2 + q3 * q3) * Math.Cos(oe.omega));
                if (oe.phi< -Math.PI)
                {
                    oe.phi += 2 * Math.PI;
                }

                oe.kappa = Math.Atan2(2 * (q1 * q2 + q0 * q3) * Math.Cos(oe.omega),
                   (q0 * q0 - q1 * q1 + q2 * q2 - q3 * q3) * Math.Cos(oe.omega));
                if (oe.kappa < -Math.PI)
                {
                    oe.kappa += 2 * Math.PI;
                }
                return oe;
            }
        }

        /// <summary>
        /// 方向余弦表示的外方位元素
        /// </summary>
        public class OutElement9
        {
            /// <summary>
            /// 线元素
            /// </summary>
            public _3D_Point Spos = new _3D_Point();

            /// <summary>
            /// 方向余弦
            /// </summary>
            public double a1=1, a2, a3, b1, b2=1, b3, c1, c2, c3=1;

            /// <summary>
            /// 返回旋转矩阵
            /// </summary>
            /// <returns></returns>
            public Matrix GetR()
            {
                Matrix R = new Matrix(3, 3);
                R[0, 0] = a1;
                R[0, 1] = a2;
                R[0, 2] = a3;
                R[1, 0] = b1;
                R[1, 1] = b2;
                R[1, 2] = b3;
                R[2, 0] = c1;
                R[2, 1] = c2;
                R[2, 2] = c3;
                return R;
            }

            /// <summary>
            /// 返回结果
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                StringBuilder sb = new StringBuilder("X,Y,Z:");
                sb.Append(Spos.ToFormatString(6));
                sb.AppendLine();
                sb.AppendLine("R=");
                sb.Append(GetR().ToFormatString(6));
                return sb.ToString();
            }

            /// <summary>
            /// 转换为欧拉角表示形式的外方元素
            /// </summary>
            /// <returns></returns>
            public OutsideElement ToOutElem3()
            {
                OutsideElement oe = new OutsideElement { Spos = Spos };
                oe.phi = Math.Atan2(-a3 , c3);
                oe.omega = Math.Asin(-b3);
                oe.kappa = Math.Atan2(b1 , b2);
                return oe;
            }
        }

        /// <summary>
        /// 内方位元素
        /// </summary>
        public class InsideElement
        {
            /// <summary>
            /// 主点坐标
            /// </summary>
            public _2D_Point p0 = new _2D_Point();

            /// <summary>
            /// 主距
            /// </summary>
            public double f;
        }

        /// <summary>
        /// 畸变参数
        /// </summary>
        public struct DistortionParams
        {
            /// <summary>
            /// 畸变参数
            /// </summary>
            public double k1, k2, p1, p2, alph, beta;
        }

        /// <summary>
        /// 内外方位元素集合
        /// </summary>
        public class ParamData
        {
            /// <summary>
            /// 外方位元素
            /// </summary>
            public List<OutsideElement> outE = new List<OutsideElement>();
            /// <summary>
            /// 内方位元素
            /// </summary>
            public InsideElement inE = new InsideElement();
            /// <summary>
            /// 畸变系数
            /// </summary>
            public DistortionParams dParams = new DistortionParams();

            /// <summary>
            /// 通过初值文件构造
            /// </summary>
            /// <param name="originfile"></param>
            public ParamData(string originfile)
            {
                List<string> data = new List<string>();
                using (StreamReader sr = new StreamReader(originfile, Encoding.Default))
                {
                    while(sr.Peek()!=-1)
                    {
                        string s = sr.ReadLine().Trim();
                        if (!string.IsNullOrWhiteSpace(s))
                            data.Add(s);
                    }
                }
                if (data.Count == 0)
                    throw new Exception("初值文件数据为空或格式不正确");

                var ss=Regex.Split(data[0], " +|,");
                inE.p0.X = double.Parse(ss[0]);
                inE.p0.Y = double.Parse(ss[1]);
                inE.f = double.Parse(ss[2]);
                for (int i = 1; i < data.Count; i++)
                {
                    ss = Regex.Split(data[i], " +|,");
                    OutsideElement oe = new OutsideElement();
                    oe.Spos.X = double.Parse(ss[0]);
                    oe.Spos.Y = double.Parse(ss[1]);
                    oe.Spos.Z = double.Parse(ss[2]);
                    oe.phi = double.Parse(ss[3]);
                    oe.omega = double.Parse(ss[4]);
                    oe.kappa= double.Parse(ss[5]);
                    outE.Add(oe);
                }
            }
        }

        /// <summary>
        /// ???
        /// </summary>
        public class OData2IData
        {
            List<ObjectData> olist;
            List<List<ImageData>> ilist=new List<List<ImageData>>();
            ParamData pd;
            public OData2IData(List<ObjectData> odlist,ParamData pd)
            {
                olist = odlist;
                this.pd = pd;
            } 

            public List<List<ImageData>> Cal()
            {
                for (int i = 0; i < pd.outE.Count; i++)
                {
                    ilist.Add(new List<ImageData>());
                }
                return null;
            }
        }
    }
}
