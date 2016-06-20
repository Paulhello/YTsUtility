using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace YTsUtility.User
{
    public partial class UserChange : Form
    {
        public UserChange()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists("user.dat"))
            {
                MessageBox.Show("未在本机上注册!");
                return;
            }
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("密码不能为空!");
                return;
            }
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("两次输入的密码不一致!");
                return;
            }
            UserInfo ui = UserInfo.ReadInfo("user.dat");
            if (ui.isRight(textBox4.Text, textBox1.Text))
            {
                ui.ChangePwd(textBox2.Text,"user.dat");
                MessageBox.Show("修改成功!");
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("用户名或密码错误!");
                return;
            }
        }
    }
}
