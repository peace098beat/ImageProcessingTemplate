using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal
{
    class ImageArray
    {
        public byte[,] R;
        public byte[,] G;
        public byte[,] B;

        public ImageArray(byte[,] R, byte[,] G, byte[,] B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }

    public static class BoxCounting
    {
        private static ImageArray ToArray(in Bitmap img)
        {
            //1ピクセルあたりのバイト数を取得する
            PixelFormat pixelFormat = img.PixelFormat;
            int pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;
            if (pixelSize < 3 || 4 < pixelSize)
            {
                throw new ArgumentException(
                    "1ピクセルあたり24または32ビットの形式のイメージのみ有効です。",
                    "img");
            }

            //Bitmapをロックする
            BitmapData bmpData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, pixelFormat);

            if (bmpData.Stride < 0)
            {
                img.UnlockBits(bmpData);
                throw new ArgumentException("ボトムアップ形式のイメージには対応していません。", "img");
            }

            //ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * img.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

            // Copy先 [TODO] 高速化
            byte[,] ArrayR = new byte[img.Width, img.Height];
            byte[,] ArrayG = new byte[img.Width, img.Height];
            byte[,] ArrayB = new byte[img.Width, img.Height];

            //すべてのピクセルの色を取得
            for (int x = 0; x < bmpData.Width; x++)
            {
                for (int y = 0; y < bmpData.Height; y++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;


                    ArrayB[x, y] = pixels[pos + 0]; // Blue
                    ArrayG[x, y] = pixels[pos + 1]; // Green
                    ArrayR[x, y] = pixels[pos + 2]; // Red
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);

            //ロックを解除する
            img.UnlockBits(bmpData);

            return new ImageArray(ArrayR, ArrayG, ArrayB);

        }


        /// <summary>
        /// ボックスカウント Easy
        /// </summary>
        /// <param name="KernelSize">2</param>
        /// <param name="stride">0</param>
        /// <param name="img"></param>
        public static double StandardCount(uint KernelSize, uint stride, in Bitmap img)
        {
            // img -> Array
            ImageArray imageArray = BoxCounting.ToArray(in img);

            //if (img.Width != img.Height) throw new ArgumentException("img.width != height");
            //if (KernelSize > img.Width / 2) throw new ArgumentException("KernelSize < img.Width / 2");
            //if (2 > KernelSize) throw new ArgumentException("2 <= KernelSize");

            int W = img.Width;
            int H = img.Height;

            int Ksize = (int)KernelSize;
            int Stride = Ksize;

            int Nw = (int)Math.Floor((double)W / Ksize);
            int Nh = (int)Math.Floor((double)H / Ksize);

            double sum=0;

            for (int i = 0; i < Nw; i++)
            {
                int x = i * Ksize;

                for (int j = 0; j < Nh; j++)
                {
                    int y = j * Ksize;

                    double std = Std(Ksize, x, y, imageArray.R);


                    sum += std;
                }
            }

            // アンサンブル平均
            sum /= (double)(Nw * Nh);

            // スケーリング
            //sum /= Ksize;

            return sum;

        }

        private static double Std(int Ksize, int x0, int y0, in byte[,] img)
        {
            // 畳み込みカーネル All 1/(Ksize*Ksize)
            double ave = 0;
            double sumpow_xi = 0; // x_i^2


            int M = Ksize * Ksize;

            for (int ki = 0; ki < Ksize; ki++)
            {
                for (int kj = 0; kj < Ksize; kj++)
                {
                    double value = img[x0 + ki, y0 + kj];

                    ave += value / M;
                    sumpow_xi += Math.Pow(value, 2);

                }
            }

            double Var = (sumpow_xi - (ave * ave)) / M;
            double Std = Math.Sqrt(Var);

            return Std;

        }
    }
}
