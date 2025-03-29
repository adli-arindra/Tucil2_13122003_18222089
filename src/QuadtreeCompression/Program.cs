using Emgu.CV.CvEnum;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Util;

namespace QuadtreeCompression
{
    internal class Program
    {
        static string defaultPath = "C:\\Users\\Adli\\Desktop\\ilegil\\semester 6\\stima\\Tucil2_13122003_18222089\\src\\QuadtreeCompression\\images\\rose.webp";

        static string inputPath, outputPath;
        static int methodIdx, threshold, minBlockSize;
        static double compressionTarget;

        static void Main(string[] args)
        {
            inputPath = defaultPath;


            Mat mat = CvInvoke.Imread(inputPath, ImreadModes.Color);
            List<List<Pixel>> image = Image.MatToPixels(mat);

            Console.WriteLine("Variance: " + Measurer.Variance(image));
        }

        static void Solve(List<List<Pixel>> image)
        {

        }
    }
}
