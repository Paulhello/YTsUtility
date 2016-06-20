using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace YTsUtility
{
    /// <summary>
    /// Excel类
    /// </summary>
    public class Excel
    {
        Application app = new Application();
        Workbooks books;
        Workbook book;
        Sheets sheets;
        _Worksheet sheet;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Excel()
        {
            books = app.Workbooks;
            book = books.Add();
            sheets = book.Worksheets;
            sheet = (_Worksheet)sheets.get_Item(1);
        }
        /// <summary>
        /// 显示一个列表的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="heads"></param>
        /// <param name="values"></param>
        public void Show<T>(List<string> heads,List<T> values)
        {
            var type = typeof(T);
            var members= type.GetFields();
            members[0].GetValue(values[0]);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            
        }
    }
}
