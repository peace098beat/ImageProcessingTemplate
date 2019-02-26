using System.Diagnostics;
using System.Drawing;
namespace FractalTests
{
    class Program
    {
        static void Main(string[] args)
        {
            // test
            BoxCountTest();

        }


        // ボックスカウントする
        static void BoxCountTest()
        {
            //string path = @"img\GaussNoise50per_x1024.png";
            string path = @"img\white2.png";
            //string path = @"img\black.png";

            // 画像をロード
            Bitmap OriginBitmap = FiFractal.Images.FromFile(path);


            // グレースケール化
            Bitmap GrayScaleBitmap = (Bitmap)OriginBitmap.Clone();
            FiFractal.BitmapConverter.GrayScale(ref GrayScaleBitmap);

            // byte配列に変形
            byte[,] bGrayArray = FiFractal.BitmapConverter.BitmapToByte2D(in GrayScaleBitmap);
            float[,] fGrayArray = FiFractal.BitmapConverter.BitmapToFloat2D(in GrayScaleBitmap);


            // 二値化
            byte thr = 32;
            Bitmap BinalyBitmap = (Bitmap)GrayScaleBitmap.Clone();
            FiFractal.BitmapConverter.Binalize(ref BinalyBitmap, thr);

            byte[,] b = FiFractal.BitmapConverter.BitmapToByte2D(in BinalyBitmap);


            // BoxCounting
            FiFractal.BoxCounting box = new FiFractal.BoxCounting(BinalyBitmap, thr);
            double D = box.D;

            Debug.WriteLine($"Fractal D={D}");
            box.WriteCsv(System.IO.Path.GetFileNameWithoutExtension(path) + ".csv");


            // Save
            FiFractal.Images.Save(OriginBitmap, "OriginBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            FiFractal.Images.Save(GrayScaleBitmap, "GrayScaleBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            FiFractal.Images.Save(BinalyBitmap, "BinalyBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);


            // 階調反転
            Bitmap OriginToneInverse = (Bitmap)OriginBitmap.Clone();
            FiFractal.BitmapConverter.ToneInverse(ref OriginToneInverse);
            FiFractal.Images.Save(OriginToneInverse, "ToneInverse.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

            // 階調反転
            Bitmap GrayToneInverse = (Bitmap)GrayScaleBitmap.Clone();
            FiFractal.BitmapConverter.ToneInverse(ref GrayToneInverse);
            FiFractal.Images.Save(GrayToneInverse, "GrayToneInverse.bmp", System.Drawing.Imaging.ImageFormat.Bmp);


        }


    }
}
