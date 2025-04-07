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
            int rows = image.Count;
            int cols = image[0].Count;
            int totalPixels = rows * cols;

            double sumR = 0.0, sumG = 0.0, sumB = 0.0;

            foreach (var row in image)
            {
                foreach (var pixel in row)
                {
                    sumR += pixel.R;
                    sumG += pixel.G;
                    sumB += pixel.B;
                }
            }

            double meanR = sumR / totalPixels;
            double meanG = sumG / totalPixels;
            double meanB = sumB / totalPixels;

            double madR = 0.0, madG = 0.0, madB = 0.0;

            foreach (var row in image)
            {
                foreach (var pixel in row)
                {
                    madR += Math.Abs(pixel.R - meanR);
                    madG += Math.Abs(pixel.G - meanG);
                    madB += Math.Abs(pixel.B - meanB);
                }
            }

            madR /= totalPixels;
            madG /= totalPixels;
            madB /= totalPixels;

            return (madR + madG + madB) / 3.0;
        }

        public static double MaxPixelDifference(List<List<Pixel>> image)
        {
            int maxR = int.MinValue, maxG = int.MinValue, maxB = int.MinValue;
            int minR = int.MaxValue, minG = int.MaxValue, minB = int.MaxValue;

            foreach (var row in image)
            {
                foreach (var pixel in row)
                {
                    maxR = Math.Max(maxR, pixel.R);
                    minR = Math.Min(minR, pixel.R);

                    maxG = Math.Max(maxG, pixel.G);
                    minG = Math.Min(minG, pixel.G);

                    maxB = Math.Max(maxB, pixel.B);
                    minB = Math.Min(minB, pixel.B);
                }
            }

            double diffR = maxR - minR;
            double diffG = maxG - minG;
            double diffB = maxB - minB;

            return (diffR + diffG + diffB) / 3.0;
        }

        public static double Entropy(List<List<Pixel>> image)
        {
            int totalPixels = image.Count * image[0].Count;

            double EntropyForChannel(Func<Pixel, int> channelSelector)
            {
                var histogram = new int[256];
                foreach (var row in image)
                {
                    foreach (var pixel in row)
                    {
                        histogram[channelSelector(pixel)]++;
                    }
                }

                double entropy = 0.0;
                foreach (var count in histogram)
                {
                    if (count == 0) continue;
                    double p = (double)count / totalPixels;
                    entropy -= p * Math.Log2(p);
                }

                return entropy;
            }

            double entropyR = EntropyForChannel(p => p.R);
            double entropyG = EntropyForChannel(p => p.G);
            double entropyB = EntropyForChannel(p => p.B);

            return (entropyR + entropyG + entropyB) / 3.0;
        }

        public static double SSIM(List<List<Pixel>> image)
        {
            return 0.0;
            //int rows = img1.Count;
            //int cols = img1[0].Count;
            //int total = rows * cols;

            //double C1 = 6.5025, C2 = 58.5225;

        //    double SSIMChannel(Func<Pixel, int> selector)
        //    {
        //        double muX = 0.0, muY = 0.0;
        //        for (int y = 0; y < rows; y++)
        //        {
        //            for (int x = 0; x < cols; x++)
        //            {
        //                muX += selector(img1[y][x]);
        //                muY += selector(img2[y][x]);
        //            }
        //        }
        //        muX /= total;
        //        muY /= total;

        //        double sigmaX = 0.0, sigmaY = 0.0, sigmaXY = 0.0;
        //        for (int y = 0; y < rows; y++)
        //        {
        //            for (int x = 0; x < cols; x++)
        //            {
        //                double px = selector(img1[y][x]) - muX;
        //                double py = selector(img2[y][x]) - muY;

        //                sigmaX += px * px;
        //                sigmaY += py * py;
        //                sigmaXY += px * py;
        //            }
        //        }

        //        sigmaX /= total;
        //        sigmaY /= total;
        //        sigmaXY /= total;

        //        double numerator = (2 * muX * muY + C1) * (2 * sigmaXY + C2);
        //        double denominator = (muX * muX + muY * muY + C1) * (sigmaX + sigmaY + C2);

        //        return numerator / denominator;
        //    }

        //    double ssimR = SSIMChannel(p => p.R);
        //    double ssimG = SSIMChannel(p => p.G);
        //    double ssimB = SSIMChannel(p => p.B);

        //    return (ssimR + ssimG + ssimB) / 3.0;
        }
    }

}

