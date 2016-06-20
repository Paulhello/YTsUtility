using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        string sha;
        string salt = Guid.NewGuid().ToString();
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { set; get; }
        /// <summary>
        /// 哈希值
        /// </summary>
        public string pwdSha
        {
            set 
            {
                sha = GetSaltSha1(value,salt);
            }
            private get { return sha; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        public UserInfo(string name,string pwd)
        {
            UserName = name;
            pwdSha = pwd;
        }

        /// <summary>
        /// 输入书否正确
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool isRight(string name,string pwd)
        {
            string n = GetSaltSha1(pwd,salt);
            if (name == UserName && pwdSha == GetSaltSha1(pwd,salt))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newpwd"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool ChangePwd(string newpwd,string path)
        {
            sha = GetSaltSha1(newpwd, salt);
            Save(this, path);
            return true;
        }

        private static string GetSaltSha1(string pwd,string salt)
        {
            
            byte[] passwordAndSaltBytes = System.Text.Encoding.UTF8.GetBytes(pwd + salt);
            byte[] hashBytes = new System.Security.Cryptography.SHA256Managed().ComputeHash(passwordAndSaltBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="path"></param>
        public static void Save(UserInfo ui,string path)
        {
            ui.Serialize(path);
        }

        /// <summary>
        /// 读取信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static UserInfo ReadInfo(string path)
        {

            return Methods.Deserialize<UserInfo>(path);
        }
    }
}
