using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Dnn;

namespace QuadtreeCompression
{
    internal class Utils
    {
        public static long GetFileSize(string absolutePath)
        {
            FileInfo fileInfo = new FileInfo(absolutePath);
            if (fileInfo.Exists)
            {
                return fileInfo.Length;
            }
            else
            {
                throw new FileNotFoundException("File not found.", absolutePath);
            }
        }
    }
}
