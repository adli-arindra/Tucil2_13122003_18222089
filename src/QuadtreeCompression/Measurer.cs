using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuadtreeCompression
{
    internal static class Measurer
    {
        public static double Variance(List<List<Pixel>> image)
        {
            int rows = image.Count;
            int cols = image[0].Count;
            int totalPixels = rows * cols;

            double sumR = 0.0, sumG = 0.0, sumB = 0.0;
            double sumSqR = 0.0, sumSqG = 0.0, sumSqB = 0.0;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Pixel pixel = image[y][x];

                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;

                    sumSqR += pixel.R * pixel.R;
                    sumSqG += pixel.G * pixel.G;
                    sumSqB += pixel.B * pixel.B;
                }
            }

            double meanR = sumR / totalPixels;
            double meanG = sumG / totalPixels;
            double meanB = sumB / totalPixels;

            double varR = (sumSqR / totalPixels) - (meanR * meanR);
            double varG = (sumSqG / totalPixels) - (meanG * meanG);
            double varB = (sumSqB / totalPixels) - (meanB * meanB);

            return (varR + varG + varB) / 3.0;
        }

        public static double MeanAbsoluteDeviation(List<List<Pixel>> image)
        {
            return 0.0;
        }

        public static double MaxPixelDifference(List<List<Pixel>> image)
        {
            return 0.0;
        }
        public static double Entropy(List<List<Pixel>> image)
        {
            return 0.0;
        }
        public static double SSIM(List<List<Pixel>> image)
        {
            return 0.0;
        }
    }
}
