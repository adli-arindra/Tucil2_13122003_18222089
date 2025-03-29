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
    internal static class Image
    {
        public static void Show(Mat image, int width = 600)
        {
            int newHeight = (image.Height * width) / image.Width;

            Mat resizedImage = new Mat();
            CvInvoke.Resize(image, resizedImage, new Size(width, newHeight), 0, 0, Inter.Linear);

            CvInvoke.Imshow("Resized Image", resizedImage);
            CvInvoke.WaitKey(0);
        }

        public static void Extract(Mat image, out Mat blue, out Mat green, out Mat red)
        {
            VectorOfMat channels = new VectorOfMat();
            CvInvoke.Split(image, channels);

            blue = channels[0].Clone();
            green = channels[1].Clone();
            red = channels[2].Clone();

            channels.Dispose();
        }

        public static List<List<int>> MatToList(Mat channel)
        {
            List<List<int>> result = new List<List<int>>();

            byte[] imageData = new byte[channel.Rows * channel.Cols];
            System.Runtime.InteropServices.Marshal.Copy(channel.DataPointer, imageData, 0, imageData.Length);

            for (int y = 0; y < channel.Rows; y++)
            {
                List<int> row = new List<int>();
                for (int x = 0; x < channel.Cols; x++)
                {
                    row.Add(imageData[y * channel.Cols + x]);
                }
                result.Add(row);
            }

            return result;
        }
    }
}
