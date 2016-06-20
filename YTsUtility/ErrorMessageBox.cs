using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace YTsUtility
{
    /// <summary>
    /// 错误对话框
    /// </summary>
    public class ErrorMessageBox:Component
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { set; get; }

        /// <summary>
        /// 构造函数，添加为.net组件
        /// </summary>
        /// <param name="container"></param>
        public ErrorMessageBox(IContainer container)
        {
            container.Add(this);
            ErrorMessage = "数据有误!";
        }
        /// <summary>
        /// 显示错误框
        /// </summary>
        /// <returns></returns>
        public DialogResult Show()
        {
            return MessageBox.Show(ErrorMessage, "错误");
        }

        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="errormsg">错误信息</param>
        /// <returns></returns>
        public DialogResult Show(string errormsg)
        {
            return MessageBox.Show(ErrorMessage=errormsg, "错误!");
        }
    }
}
