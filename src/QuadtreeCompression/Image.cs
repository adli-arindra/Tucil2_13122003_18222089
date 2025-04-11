using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV;

namespace QuadtreeCompression
{
    public class Pixel
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public Pixel(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
    }

    internal static class Image
    {
        private static readonly int _width = 600;
        public static void Show(Mat image)
        {
            int newHeight = (image.Height * _width) / image.Width;

            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(_width, newHeight), 0, 0, Inter.Linear);

            CvInvoke.Imshow("Resized Image", resizedImage);
            CvInvoke.WaitKey(0);
        }

        public static void Show(List<List<Pixel>> image)
        {
            int rows = image.Count;
            int cols = image[0].Count;

            Mat ret = new Mat(rows, cols, DepthType.Cv8U, 3);

            byte[] imageData = new byte[rows * cols * 3];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int index = (y * cols + x) * 3;
                    Pixel pixel = image[y][x];

                    imageData[index] = (byte)pixel.B;
                    imageData[index + 1] = (byte)pixel.G;
                    imageData[index + 2] = (byte)pixel.R;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, ret.DataPointer, imageData.Length);

            int newHeight = (ret.Height * _width) / ret.Width;
            Mat resizedImage = new Mat();
            CvInvoke.Resize(ret, resizedImage, new Size(_width, newHeight), 0, 0, Inter.Linear);

            CvInvoke.Imshow("Resized Image", resizedImage);
            CvInvoke.WaitKey(0);
        }



        public static List<List<Pixel>> MatToPixels(Mat image)
        {
            List<List<Pixel>> result = new List<List<Pixel>>();

            int rows = image.Rows;
            int cols = image.Cols;
            byte[] imageData = new byte[rows * cols * 3]; 

            System.Runtime.InteropServices.Marshal.Copy(image.DataPointer, imageData, 0, imageData.Length);

            for (int y = 0; y < rows; y++)
            {
                List<Pixel> row = new List<Pixel>();
                for (int x = 0; x < cols; x++)
                {
                    int index = (y * cols + x) * 3;
                    int b = imageData[index];
                    int g = imageData[index + 1];
                    int r = imageData[index + 2];

                    row.Add(new Pixel(r, g, b));
                }
                result.Add(row);
            }

            return result;
        }

        public static Mat PixelsToMat(List<List<Pixel>> image)
        {
            int rows = image.Count;
            int cols = image[0].Count;
            Mat matImage = new Mat(rows, cols, DepthType.Cv8U, 3);

            byte[] imageData = new byte[rows * cols * 3];

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int index = (y * cols + x) * 3;
                    Pixel pixel = image[y][x];

                    imageData[index] = (byte)pixel.B;
                    imageData[index + 1] = (byte)pixel.G;
                    imageData[index + 2] = (byte)pixel.R;
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(imageData, 0, matImage.DataPointer, imageData.Length);
            return matImage;
        }

        public static void ExportMat(Mat image, string outputPath)
        {
            CvInvoke.Imwrite(outputPath, image);
        }

        public static void NormalizePixels(List<List<Pixel>> image, int startX, int startY, int width, int height)
        {
            long sumR = 0, sumG = 0, sumB = 0;
            int totalPixels = width * height;

            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    sumR += image[y][x].R;
                    sumG += image[y][x].G;
                    sumB += image[y][x].B;
                }
            }

            int meanR = (int)(sumR / totalPixels);
            int meanG = (int)(sumG / totalPixels);
            int meanB = (int)(sumB / totalPixels);

            for (int y = startY; y < startY + height; y++)
            {
                for (int x = startX; x < startX + width; x++)
                {
                    image[y][x] = new Pixel(meanR, meanG, meanB);
                }
            }
        }


        public static List<List<List<Pixel>>> SplitIntoQuarters(List<List<Pixel>> image)
        {
            int rows = image.Count / 2;
            int cols = image[0].Count / 2;

            List<List<Pixel>> topLeft = new List<List<Pixel>>();
            List<List<Pixel>> topRight = new List<List<Pixel>>();
            List<List<Pixel>> bottomLeft = new List<List<Pixel>>();
            List<List<Pixel>> bottomRight = new List<List<Pixel>>();

            for (int y = 0; y < rows; y++)
            {
                topLeft.Add(image[y].GetRange(0, cols));
                topRight.Add(image[y].GetRange(cols, cols));
            }

            for (int y = rows; y < rows * 2; y++)
            {
                bottomLeft.Add(image[y].GetRange(0, cols));
                bottomRight.Add(image[y].GetRange(cols, cols));
            }

            return new List<List<List<Pixel>>> { topLeft, topRight, bottomLeft, bottomRight };
        }

        public static List<List<Pixel>> ExtractSubImage(List<List<Pixel>> image, int startX, int startY, int width, int height)
        {
            List<List<Pixel>> subImage = new List<List<Pixel>>();

            for (int y = startY; y < startY + height; y++)
            {
                List<Pixel> row = new List<Pixel>();
                for (int x = startX; x < startX + width; x++)
                {
                    row.Add(image[y][x]);
                }
                subImage.Add(row);
            }

            return subImage;
        }
    }
}
