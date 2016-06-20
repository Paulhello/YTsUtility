using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 二维向量
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// X
        /// </summary>
        public double X { set; get; }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { set; get; }
        /// <summary>
        /// 通过数对构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(double x,double y)
        {
            X = x; Y = y;
        }
        /// <summary>
        /// 默认构造
        /// </summary>
        public Vector2() { }

        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector2 operator+(Vector2 v1,Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// 数量积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator *(Vector2 v1,Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /// <summary>
        /// 形如(x,y)的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "("+X+","+Y+")";
        }

        /// <summary>
        /// 向量相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator==(Vector2 v1,Vector2 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        /// <summary>
        /// 向量不等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2 v1,Vector2 v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// 重载equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is Vector2&&obj as Vector2==this)
               return true;
            return false;
        }

        /// <summary>
        /// 重载GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode()+Y.GetHashCode(); ;
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 operator-(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        /// <summary>
        /// 向量模
        /// </summary>
        /// <returns></returns>
        public virtual double GetNorm()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        /// <summary>
        /// 复制该向量
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            return new Vector2(X, Y);
        }
    }

    /// <summary>
    /// 三维向量
    /// </summary>
    public class Vector3:Vector2
    {
        /// <summary>
        /// Z
        /// </summary>
        public double Z { set; get; }

        /// <summary>
        /// 通过有序数对构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(double x,double y,double z):base(x,y)
        {
            Z = z;
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public Vector3() { }

       
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static Vector3 operator+(Vector3 v1,Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y,v1.Z+v2.Z);
        }

        /// <summary>
        /// 数量积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static double operator*(Vector3 v1,Vector3 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y+v1.Z*v2.Z;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 v, double f)
        {
            Vector3 t = new Vector3();
            t.X = v.X * f;
            t.Y = v.Y * f;
            t.Z = v.Z * f;
            return t;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Vector3 operator *(double f, Vector3 v)
        {
            Vector3 t = new Vector3();
            t.X = v.X * f;
            t.Y = v.Y * f;
            t.Z = v.Z * f;
            return t;
        }

        /// <summary>
        /// 同时除以一个实数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 v,double f)
        {
            Vector3 t = new Vector3();
            t.X = v.X / f;
            t.Y = v.Y / f;
            t.Z = v.Z / f;
            return t;
        }

        /// <summary>
        /// 向量相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y&&v1.Z==v2.Z;
        }

        /// <summary>
        /// 向量不等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector3 operator-(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        /// <summary>
        /// 向量积
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 XMultpy(Vector3 v)
        {
            double x, y, z;
            x = Y * v.Z - Z * v.Y;
            y = Z * v.X - X * v.Z;
            z = X * v.Y - Y * v.X;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 形如(x,y,z)的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + ","+Z+")";
        }

        /// <summary>
        /// 向量模
        /// </summary>
        /// <returns></returns>
        public override double GetNorm()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summary>
        /// 复制该向量
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Vector3(X,Y,Z);
        }

        /// <summary>
        /// 重载equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Vector3 && obj as Vector3 == this)
                return true;
            return false;
        }

        /// <summary>
        /// 重载GetHashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode()+Y.GetHashCode()+Z.GetHashCode();
        }
    }

    /// <summary>
    /// 字符向量
    /// </summary>
    public class StrVector3
    {
        /// <summary>
        /// X
        /// </summary>
        public string X{set;get;}
        /// <summary>
        /// Y
        /// </summary>
        public string Y{set;get;}
         /// <summary>
        /// Z
        /// </summary>
        public string Z { set; get; }

        /// <summary>
        /// 通过有序数对构造
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public StrVector3(string x,string y,string z)
        {
            X=x;
            Y=y;
            Z = z;
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public StrVector3()
        { 
            X = "0";
            Y = "0"; 
            Z = "0"; 
        }

       
        /// <summary>
        /// 向量加法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static StrVector3 operator+(StrVector3 v1,StrVector3 v2)
        {
            return new StrVector3("("+v1.X +"+"+ v2.X+")", "("+v1.Y +"+"+ v2.Y+")","("+v1.Z+"+"+v2.Z+")");
        }

        /// <summary>
        /// 数量积
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static string operator*(StrVector3 v1,StrVector3 v2)
        {
            return "("+v1.X +"*"+ v2.X +"+"+ v1.Y +"*"+ v2.Y+"+"+v1.Z+"*"+v2.Z+")";
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static StrVector3 operator *(StrVector3 v, string f)
        {
            if (f == "0") return new StrVector3();
            StrVector3 t = new StrVector3();
            t.X = f + "*" + v.X ;
            t.Y = f + "*" + v.Y;
            t.Z = f + "*" + v.Z;
            return t;
        }

        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static StrVector3 operator *(string f, StrVector3 v)
        {
            if (f == "0") return new StrVector3();

            StrVector3 t = new StrVector3();
            t.X = f + "*" + v.X;
            t.Y = f + "*" + v.Y;
            t.Z = f + "*" + v.Z;
            return t;
        }

        /// <summary>
        /// 同时除以一个实数
        /// </summary>
        /// <param name="v"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static StrVector3 operator /(StrVector3 v,string f)
        {
            StrVector3 t = new StrVector3();
            t.X = v.X +"/"+ f;
            t.Y = v.Y + "/" + f;
            t.Z = v.Z + "/" + f;
            return t;
        }

        /// <summary>
        /// 向量取负
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static StrVector3 operator-(StrVector3 v)
        {
            return new StrVector3("(-"+v.X+")", "(-"+v.Y+")", "(-"+v.Z+")");
        }

        /// <summary>
        /// 向量减法
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static StrVector3 operator -(StrVector3 v1, StrVector3 v2)
        {
            return new StrVector3("("+v1.X + "-" + v2.X+")", "("+v1.Y + "-" + v2.Y+")", "("+v1.Z + "-" + v2.Z+")");
        }

        /// <summary>
        /// 向量积
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public StrVector3 XMultpy(StrVector3 v)
        {
            string x, y, z;
            x = "(" + Y +"*"+ v.Z +"-"+ Z +"*"+ v.Y + ")";
            y = "(" + Z + "*" + v.X + "-" + X + "*" + v.Z + ")";
            z = "(" + X + "*" + v.Y + "-" + Y + "*" + v.X + ")";
            return new StrVector3(x, y, z);
        }

        /// <summary>
        /// 形如(x,y,z)的字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + X + "," + Y + ","+Z+")";
        }

        /// <summary>
        /// 向量模
        /// </summary>
        /// <returns></returns>
        public string GetNorm()
        {
            return "√("+X +"*"+ X +"+"+ Y +"*"+ Y +"+"+ Z +"*"+ Z+")";
        }
    }

}
