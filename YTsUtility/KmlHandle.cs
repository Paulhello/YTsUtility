using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YTsUtility
{
    /// <summary>
    /// kml文件处理类
    /// </summary>
    public class KmlHandle
    {
        /// <summary>
        /// xml文件路径
        /// </summary>
        public string FilePath {private set; get; }
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

        public void GetAttribute(string attribute)
        {

        }
    }
}
