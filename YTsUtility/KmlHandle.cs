using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace YTsUtility
{
    /// <summary>
    /// kml文件处理类
    /// </summary>
    public class KmlHandle
    {
        private string m_path;
        /// <summary>
        /// xml文件路径
        /// </summary>
        public string FilePath
        {
            set {
                if (File.Exists(value))
                    m_path = FilePath;
                else
                    throw new Exception("File not exist!");
            }
            get { return m_path; }
        }
        XmlDocument doc=new XmlDocument();
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="path">xml文件路径</param>
        public KmlHandle(string path)
        {
            doc.Load(path);
            FilePath = path;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="attribute"></param>
        public XmlNodeList GetAttribute(string attribute)
        {
            return doc.GetElementsByTagName(attribute);
        }
    }
}
