using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher.Core.Common
{
    public class FileHelper
    {
        public static string GetExtention(string filename)
        {
            if (filename.EndsWith("."))
                return string.Empty;

            int pos = filename.LastIndexOf(".");
            if (pos >= 0)
                return filename.Substring(pos).ToLower();
            else
                return string.Empty;
        }

        /// <summary>
        /// 返回当前系统临时文件夹下的随机文件名路径
        /// </summary>
        /// <param name="extension">扩展名</param>
        public static string GetRandomTempFile(string extension = null)
        {
            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
                extension = "." + extension;

            var tempPath = Path.GetTempPath();
            var randomName = Path.GetRandomFileName();
            return Path.Combine(tempPath, randomName, extension);
        }
    }
}
