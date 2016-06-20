using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTsUtility
{
    /// <summary>
    /// 等待框
    /// </summary>
    public partial class Waiting : UserControl
    {
        /// <summary>
        /// 工作信息
        /// </summary>
        public string WorkMessage { get; set; }
        int i = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        public Waiting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workMessage">工作信息</param>
        public Waiting(string workMessage)
        {
            InitializeComponent();
            WorkMessage = workMessage;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (i == 3)
            {
                label1.Text = label1.Text.Substring(0, label1.Text.Length - 3);
                i = 0;
            }
            else
            {
                label1.Text += ".";
                ++i;
            }
        }

        private void Waiting_Load(object sender, EventArgs e)
        {
            if(WorkMessage!=string.Empty&&WorkMessage!=null)
               label1.Text = WorkMessage;
            timer1.Start();
        }
    }
}
