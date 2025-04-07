using Emgu.CV.CvEnum;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Util;

namespace QuadtreeCompression
{
    internal class Program
    {
        static string defaultInputPath = "D:\\semester 6\\stima\\Tucil2_13122003_18222089\\src\\QuadtreeCompression\\images\\rose.webp";
        static string defaultOutputPath = "D:\\semester 6\\stima\\Tucil2_13122003_18222089\\src\\QuadtreeCompression\\output.png";

        static string 
            inputPath = defaultInputPath, 
            outputPath = defaultOutputPath;
        static int 
            methodIdx = 0, 
            threshold = 20,
            minBlockSize = 10,
            iterations = 0;
        static double compressionTarget;

        static void Main(string[] args)
        {
            Console.WriteLine("sebutin methodIdx");
            methodIdx = int.Parse(Console.ReadLine());
            Mat mat = CvInvoke.Imread(inputPath, ImreadModes.Color);
            List<List<Pixel>> image = Image.MatToPixels(mat);

            Console.WriteLine("Solving...");
            Solve(image, 0, 0, image[0].Count, image.Count);
            Image.Show(image);
            Console.WriteLine(iterations);
        }

        static void Solve(List<List<Pixel>> image, int startX, int startY, int width, int height)
        {
            iterations++;
            double error = GetError(image, startX, startY, width, height);
            double area = width * height;

            if (area < minBlockSize || error < threshold)
            {
                Image.NormalizePixels(image, startX, startY, width, height);
                return;
            }

            int halfWidth = width / 2;
            int halfHeight = height / 2;

            Solve(image, startX, startY, halfWidth, halfHeight);
            Solve(image, startX + halfWidth, startY, halfWidth, halfHeight);
            Solve(image, startX, startY + halfHeight, halfWidth, halfHeight);
            Solve(image, startX + halfWidth, startY + halfHeight, halfWidth, halfHeight);
        }


        static double GetError(List<List<Pixel>> image, int startX, int startY, int width, int height)
        {
            List<List<Pixel>> subImage = Image.ExtractSubImage(image, startX, startY, width, height);

            switch (methodIdx)
            {
                case 0:
                    return Measurer.Variance(subImage);
                case 1:
                    return Measurer.MeanAbsoluteDeviation(subImage);
                case 2:
                    return Measurer.MaxPixelDifference(subImage);
                case 3:
                    return Measurer.Entropy(subImage);
                case 4:
                    return Measurer.SSIM(subImage);
                default:
                    return 0.0;
            }
        }

    }
}
