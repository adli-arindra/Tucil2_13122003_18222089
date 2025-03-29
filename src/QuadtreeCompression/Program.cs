using Emgu.CV.CvEnum;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Util;

namespace QuadtreeCompression
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string imagePath = "C:\\Users\\Adli\\Desktop\\ilegil\\semester 6\\stima\\Tucil2_13122003_18222089\\src\\QuadtreeCompression\\images\\rose.webp";

            Mat image = CvInvoke.Imread(imagePath, ImreadModes.Color);
            Mat red, green, blue;
            Image.Extract(image, out blue, out green, out red);

            List<List<int>> RedInt = Image.MatToList(red);
        }
    }
}
