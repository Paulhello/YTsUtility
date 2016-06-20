using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 角度类
    /// </summary>
    [Serializable]
    public class Angle//:ICloneable
    {
        //数据格式DDDFFMM
        /// <summary>
        /// 度
        /// </summary>
        public int Du {private set; get; }
        /// <summary>
        /// 分
        /// </summary>
        public int Fen {private set; get; }
        /// <summary>
        /// 秒
        /// </summary>
        public double Miao {private set; get; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Du">度</param>
        /// <param name="Fen">分</param>
        /// <param name="Miao">秒</param>
        /// <param name="IsPositive">是否为正</param>
        public Angle(int Du, int Fen, double Miao,bool IsPositive=true)
        {
            if(IsPositive)
            {
                this.Du = Du;
                this.Fen = Fen;
                this.Miao = Miao;
            }
            else
            {
                this.Du = -Du;
                this.Fen = -Fen;
                this.Miao = -Miao;
            }
            
        }

        /// <summary>
        /// 通过DDFFMM.MM构造角度
        /// </summary>
        /// <param name="f"></param>
        public Angle(double f)
        {
            Du = (int)f / 10000;
            Fen = (int)(f - Du * 10000) / 100;
            Miao = f - Du * 10000 - Fen * 100;
        }
        /// <summary>
        /// 构造一个实例
        /// </summary>
        public Angle() { }


        /// <summary>
        /// 角度相加
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public static Angle operator +(Angle a1, Angle a2)
        {
            //Angle t = new Angle();
            //t.Du = a1.Du + a2.Du;
            //t.Fen = a1.Fen + a2.Fen;
            //t.Miao = a1.Miao + a2.Miao;
            //return Angle.Carry(t);
             return Angle.MChangeToAngle(a1.ChangeToMiao() + a2.ChangeToMiao());
        }

        /// <summary>
        /// 角度相减
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public static Angle operator -(Angle a1, Angle a2)
        {
            
            //Angle t = new Angle();
            //t.Du = a1.Du - a2.Du;
            //t.Fen = a1.Fen - a2.Fen;
            //t.Miao = a1.Miao - a2.Miao;
            //if (t.ChangeToRad()<0)
            //{
            //    t = -t;
            //    t.Du = -t.Du;
            //    return Angle.Back(t);
            //}           
            return Angle.MChangeToAngle(a1.ChangeToMiao() - a2.ChangeToMiao());
        }

        ///// <summary>
        /////角度的相反数
        ///// </summary>
        ///// <param name="a"></param>
        ///// <returns></returns>
        //public static Angle operator -(Angle a)
        //{
        //    Angle t = new Angle();
        //    t.Du = -a.Du;
        //    t.Fen = -a.Fen;
        //    t.Miao = -a.Miao;
        //    return t;
        //}

        /// <summary>
        /// 角度求余
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Angle operator %(Angle a, Angle b)
        {
            return new Angle(a.ChangeToDouble() % b.ChangeToInt());
        }

        /// <summary>
        /// 进位
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static Angle Carry(Angle a)
        {
            if (a.Miao >= 60)
            {
                a.Miao %= 60;
                a.Fen++;
            }
            if (a.Fen >= 60)
            {
                a.Fen %= 60;
                a.Du++;
            }
            return a;

        }

        /// <summary>
        /// 退位
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        static Angle Back(Angle a)
        {
            if (a.Miao < 0)
            {
                a.Miao += 60;
                a.Fen--;
            }
            if (a.Fen < 0)
            {
                a.Fen += 60;
                a.Du--;
            }
            return a;
        }

        /// <summary>
        /// 转换成浮点型ddffmm.mm
        /// </summary>
        /// <returns></returns>
        public double ChangeToDouble()
        {
            return this.Du * 10000 + this.Fen * 100 + this.Miao;
        }

        /// <summary>
        /// 转换成秒
        /// </summary>
        /// <returns></returns>
        public double ChangeToMiao()
        {
            return Du * 3600 + Fen * 60 + Miao;
        }

        /// <summary>
        /// 将秒转换成角度类
        /// </summary>
        /// <param name="miao"></param>
        /// <returns></returns>
        public static Angle MChangeToAngle(double miao)
        {
            Angle a = new Angle();
            a.Du = (int)miao / 3600;
            a.Fen = (int)(miao - a.Du * 3600) / 60;
            a.Miao = miao - a.Du * 3600 - a.Fen * 60;
            return a;
        }
        /// <summary>
        /// 转换为整型ddffmm
        /// </summary>
        /// <returns></returns>
        public int ChangeToInt()
        {
            return this.Du * 10000 + this.Fen * 100 + (int)this.Miao;
        }

        /// <summary>
        /// 转换为弧度
        /// </summary>
        /// <returns></returns>
        public double ChangeToRad()
        {
            return ((this.Du + this.Fen / 60.0 + this.Miao / 3600.0) / 180 * Math.PI);
        }

        /// <summary>
        /// 秒精确到小数点后几位
        /// </summary>
        /// <param name="n"></param>
        public void Set_Accurate(int n)
        {
            Miao = Miao.SetAccurate(n);
        }

        /// <summary>
        /// 重载ToString,转换为每个元素的字符串相加
        /// </summary>
        /// <returns>角度</returns>
        public override string ToString()
        {
            if(Miao>=0)
                return Du + "°" + Fen+ "′" + Miao + "″";
            else
                return Du + "°" + (-Fen) + "′" + (-Miao) + "″";
        }

        /// <summary>
        /// 转换为字符串，保留到小数点后n位
        /// </summary>
        /// <param name="n">保留到小数点后n位</param>
        /// <returns></returns>
        public string ToFormatString(int n)
        {
            if (Miao >= 0)
                return Du.ToString() + "°" + Fen.ToString() + "′" + Miao.ToFormatString(n) + "″";
            else
                return Du.ToString() + "°" + (-Fen).ToString() + "′" + (-Miao).ToFormatString(n) + "″";
        }

        /// <summary>
        /// 对外提供一个复制能力
        /// </summary>
        /// <returns></returns>
        public Angle Clone()
        {
            return new Angle(Du,Fen,Miao);
        }

        /// <summary>
        /// 角度化弧度
        /// </summary>
        /// <param name="Angle">DDFFMM.MMM</param>
        /// <returns></returns>
        public static double ChangeToRad(double Angle)
        {
            return (int)Angle / 10000 + ((int)Angle % 10000) / 100 / 60 + Angle % 10000 % 100 / 3600;
        }

        /// <summary>
        /// 转换成度
        /// </summary>
        /// <returns></returns>
        public double ChangeToDu()
        {
            return Du + Fen / 60.0 + Miao / 3600;
        }

        /// <summary>
        /// 弧度转换成角度类
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Angle ChangeToAngle(double rad)
        {
            double miao = rad * 180 / Math.PI * 3600;
            int du = (int)miao / 3600;
            int fen = (int)(miao - du * 3600) / 60;
            miao -= (du * 3600 + fen * 60);
            Angle a = new Angle(du, fen, miao);
            return a;
        }

        /// <summary>
        /// 计算方位角
        /// </summary>
        /// <param name="PAngle0">已知方位角</param>
        /// <param name="ObservedAngle">观测角</param>
        /// <param name="direction">角方向</param>
        /// <returns></returns>
        public static Angle GetPangle(Angle PAngle0, Angle ObservedAngle, Methods.Direction direction)
        {
            Angle PAngle, a0 = new Angle(180, 0, 0), b0 = new Angle(360, 0, 0);
            if (direction == Methods.Direction.left)
            {
                PAngle = (PAngle0 + ObservedAngle - a0 + b0) % b0;
            }
            else
            {
                PAngle = (PAngle0 - ObservedAngle + a0 + b0) % b0;
            }
            return PAngle;
        }

        /// <summary>
        /// 计算夹角
        /// </summary>
        /// <param name="P_pre"></param>
        /// <param name="P_pos"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Angle Get_IncludedAngle(Angle P_pre, Angle P_pos, Methods. Direction direction)
        {
            Angle a = new Angle(180, 0, 0);
            Angle b = new Angle(360, 0, 0);
            if (direction == Methods.Direction.left)
            {
                return (P_pos - P_pre + a + b) % b;
            }
            else
            {
                return (P_pre - P_pos + a + b) % b;
            }
        }
    }
}
