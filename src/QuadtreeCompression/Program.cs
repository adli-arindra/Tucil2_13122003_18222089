using Emgu.CV.CvEnum;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Util;
using System.Diagnostics;
using System.IO;
using Emgu.CV.Structure;

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
            nodecount = 0,
            depth = 0;
        static double compressionTarget;
        static List<List<Pixel>> image;
        static long compressedPixels = 1,
            currentWidth = -1;
        static double initialError,
            totalPixels;



        static void Main(string[] args)
        {
            GetInput();

            Mat mat = CvInvoke.Imread(inputPath, ImreadModes.Color);
            image = Image.MatToPixels(mat);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Loading.Start();
            if (compressionTarget <= 0)
            {
                Console.WriteLine("Menggunakan threshold dan minBlockSize untuk kompresi");
                Solve(0, 0, image[0].Count, image.Count);
            }
            else
            {
                Console.WriteLine("Menggunakan target kompresi untuk kompresi");
                SolveTarget();
            }
            Loading.Stop();

            stopwatch.Stop();
            Mat output = Image.PixelsToMat(image);
            Image.ExportMat(output, outputPath);
            Console.WriteLine($"Waktu eksekusi: {stopwatch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Ukuran gambar sebelum: {Utils.GetFileSize(inputPath)} bytes");
            Console.WriteLine($"Ukuran gambar setelah: {Utils.GetFileSize(outputPath)} bytes");
            Console.WriteLine($"Presentase gambar terkompresi: {100 - (Utils.GetFileSize(outputPath) * 100 / Utils.GetFileSize(inputPath))}%");
            Console.WriteLine($"Kedalaman pohon: {depth}");
            Console.WriteLine($"Banyak simpul: {nodecount}");
            Image.Show(image);
        }


        static void GetInput()
        {
            Console.Write("Masukkan absolute path dari gambar\n> ");
            inputPath = Console.ReadLine();
            Console.Write("Masukkan nomor dari metode perhitungan error:\n" +
                "1. Variansi\n2. Mean Absolute Difference\n3.Maximum Pixel Difference\n" +
                "4. Entropy\n> ");
            methodIdx = int.Parse(Console.ReadLine());
            Console.Write("Masukkan ambang batas\n> ");
            threshold = int.Parse(Console.ReadLine());
            Console.Write("Masukkan ukuran blok minimum\n> ");
            minBlockSize = int.Parse(Console.ReadLine());
            Console.Write("Masukkan target kompresi (masukkan 0 untuk tidak memakai target, masukkan persentase (0..1) untuk memakai target dan mengabaikan threshold & minblocksize: \n> ");
            compressionTarget = double.Parse(Console.ReadLine());
            Console.Write("Masukkan absolute path untuk menyimpan gambar\n> ");
            outputPath = Console.ReadLine();
        }
        static void Solve(int startX, int startY, int width, int height)
        {
            double error = GetError(image, startX, startY, width, height);
            double area = width * height;
            compressedPixels += 4;

            if (width != currentWidth)
            {
                currentWidth = width;
                depth++;
            }

            if (area < minBlockSize || error < threshold)
            {
                Image.NormalizePixels(image, startX, startY, width, height);
                nodecount++;
                return;
            }

            int halfWidth = width / 2;
            int halfHeight = height / 2;

            Solve(startX, startY, halfWidth, halfHeight);
            Solve(startX + halfWidth, startY, halfWidth, halfHeight);
            Solve(startX, startY + halfHeight, halfWidth, halfHeight);
            Solve(startX + halfWidth, startY + halfHeight, halfWidth, halfHeight);
        }

        static void SolveTarget()
        {
            Queue<(int startX, int startY, int width, int height)> queue = new();
            queue.Enqueue((0, 0, image[0].Count, image.Count));
            methodIdx = 4;
            initialError = GetError(image, 0, 0, image[0].Count, image.Count);
            totalPixels = image.Count * image[0].Count * initialError;
            int currentCompressedPixels = 1;
            bool merge = false;

            while (queue.Count > 0)
            {
                var (startX, startY, width, height) = queue.Dequeue();
                double error = GetError(image, startX, startY, width, height);
                double area = width * height;

                if (currentWidth != width)
                {
                    currentWidth = width;
                    depth++;
                    if (!merge)
                    {
                        currentCompressedPixels *= 4;
                        compressedPixels = 0;
                    }
                }

                double currentPixelsEstimated = compressedPixels * Math.Pow(compressionTarget, 8);

                if ((1 - currentPixelsEstimated / totalPixels) < compressionTarget)
                    merge = true;

                if (merge)
                {
                    Image.NormalizePixels(image, startX, startY, width, height);
                    nodecount++;
                    continue;
                }

                int halfWidth = width / 2;
                int halfHeight = height / 2;

                if (halfWidth == 0 || halfHeight == 0)
                {
                    Image.NormalizePixels(image, startX, startY, width, height);
                    nodecount++;
                    continue;
                }

                compressedPixels += currentCompressedPixels;
                queue.Enqueue((startX, startY, halfWidth, halfHeight));
                queue.Enqueue((startX + halfWidth, startY, halfWidth, halfHeight));
                queue.Enqueue((startX, startY + halfHeight, halfWidth, halfHeight));
                queue.Enqueue((startX + halfWidth, startY + halfHeight, halfWidth, halfHeight));
            }
        }
        static double GetError(List<List<Pixel>> image, int startX, int startY, int width, int height)
        {
            List<List<Pixel>> subImage = Image.ExtractSubImage(image, startX, startY, width, height);

            switch (methodIdx)
            {
                case 1:
                    return Measurer.Variance(subImage);
                case 2:
                    return Measurer.MeanAbsoluteDeviation(subImage);
                case 3:
                    return Measurer.MaxPixelDifference(subImage);
                case 4:
                    return Measurer.Entropy(subImage);
                default:
                    return 0.0;
            }
        }

    }
}
