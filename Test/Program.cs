using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using YTsUtility;
using Dlt=DLT.DLT ;
using DLT;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            CData data = new CData();
            data.ReadData("p1.txt", "p2.txt");//读取数据
            Dlt dlt = new Dlt(data);
            dlt.Adjustment();//平差
            Console.Write(dlt.OutToString());//输出结果
            Console.WriteLine("结果保存在result.txt中...");
            File.WriteAllText("result.txt", dlt.OutToString(), Encoding.Default);//写入文件
            Console.ReadKey();
        }
    }
}
