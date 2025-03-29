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

            List<List<Pixel>> pixels = Image.MatToPixelList(image);
            Image.Show(image);
        }
    }
}
