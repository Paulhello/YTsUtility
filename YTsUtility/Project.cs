using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTsUtility
{
    
    /// <summary>
    /// 工程类
    /// </summary>
    [Serializable]
    public class Project
    {
        /// <summary>
        /// 工程创建时间
        /// </summary>
        public DateTime CreateTime {set; get; }
        /// <summary>
        /// 工程名
        /// </summary>
        public string Name {set; get; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User {set; get; }

        /// <summary>
        /// 工程文件夹路径
        /// </summary>
        public string FolderPath {set; get; }

        /// <summary>
        /// 构造一个工程
        /// </summary>
        /// <param name="folder">工程文件夹路径</param>
        /// <param name="name">工程名</param>
        public Project(string folder,string name)
        {
            CreateTime = DateTime.Now;
            Name = name;
            User = Environment.UserName;
            FolderPath = folder;
        }

        /// <summary>
        /// 无参构造函数(可序列化)
        /// </summary>
        public Project()
        {

        }
    }
}
