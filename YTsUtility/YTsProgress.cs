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
    /// 进度条
    /// </summary>
    public partial class YTsProgress : UserControl
    {
        /// <summary>
        /// 进度条Size
        /// </summary>
        public Size pSize
        {
            set { progressBar1.Size = value; }
            get { return progressBar1.Size; }
        }

        /// <summary>
        /// 进度条步进值
        /// </summary>
        public int pStep
        {
            set { progressBar1.Step = value; }
            get { return progressBar1.Step; }
        }

        /// <summary>
        /// 进度条最大值
        /// </summary>
        public int pMaximum
        {
            set { progressBar1.Maximum = value; }
            get { return progressBar1.Maximum; }
        }

        /// <summary>
        /// 进度条当前值
        /// </summary>
        public int pValue
        {
            set { progressBar1.Value = value; }
            get { return progressBar1.Value; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        public YTsProgress()
        {
            InitializeComponent();
        }
    }

}
