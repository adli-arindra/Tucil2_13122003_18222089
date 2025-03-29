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

    }
}
