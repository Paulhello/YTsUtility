using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTsUtility;

namespace YTsUtility.User
{
    public partial class UserReg : Form
    {
        UserInfo ui;
        public UserReg()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(System.IO.File.Exists("C:\\ProgramData\\YT\\FileHandle\\user.dat"))
            {
                MessageBox.Show("已存在注册用户!请登录");
                Close();
                return;
            }
            if(string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请输入正确的用户名!");
                return;
            }
            if(string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("密码不能为空!");
                return;
            }
            if(textBox3.Text!=textBox2.Text)
            {
                MessageBox.Show("两次输入的密码不一致!");
                return;
            }
            ui = new UserInfo(textBox1.Text, textBox2.Text);
            UserInfo.Save(ui, "user.dat");
            MessageBox.Show("注册成功!");
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
