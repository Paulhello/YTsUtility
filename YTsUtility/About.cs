using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTsUtility
{
    /// <summary>
    /// 关于对话框
    /// </summary>
    public partial class About :Form
    {
        /// <summary>
        /// 构造
        /// </summary>
        public About()
        { 
            InitializeComponent();

        }
        private void About_Load(object sender, EventArgs e)
        {
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="product">产品名</param>
        /// <param name="writer">作者</param>
        /// <param name="date">日期</param>
        public void Show(string product, string writer, string date)
        {
            label1.Text = product;
            label2.Text = writer;
            label3.Text = date;
            this.ShowDialog();
        }
    }
}
