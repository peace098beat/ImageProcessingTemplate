using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageProcessingTemplate.Fractal
{
    public class PixelCounting
    {

        // Common
        private class ImageArray
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

        // Common
        private static ImageArray _ToArray(in Bitmap img)
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

                    double v = 0;
                    v += pixels[pos + 0];
                    v += pixels[pos + 1];
                    v += pixels[pos + 2];
                    v /= 3.0;

                    byte g = (byte)v;

                    ArrayB[x, y] = g;
                    ArrayB[x, y] = g;
                    ArrayB[x, y] = g;

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
        /// <param name="ImgGray"></param>
        public static double Count(uint KernelSize, byte ThreshHold, in Bitmap ImgGray)
        {
            // [TODO] 画像-> 配列の手順を整理する

            // img -> Array
            // Rしか使わない.
            // グレースケールなので、R=G=B
            ImageArray imageArray = _ToArray(in ImgGray);

            // 画像サイズ
            int W = ImgGray.Width;
            int H = ImgGray.Height;


            // グレースケール化
            int[,] BinalyImage = new int[W, H];

            for (int i = 0; i < W; i++)
            {
                for (int j = 0; j < H; j++)
                {
                    if (imageArray.R[i, j] != imageArray.G[i, j]) throw new ArgumentException(); // debug => after remove
                    if (imageArray.R[i, j] != imageArray.B[i, j]) throw new ArgumentException(); // debug => after remove 

                    byte value = imageArray.R[i, j];

                    if (value < ThreshHold)
                    {
                        BinalyImage[i, j] = 0;
                    }
                    else
                    {
                        BinalyImage[i, j] = 1;
                    }
                }
            }


            //if (img.Width != img.Height) throw new ArgumentException("img.width != height");
            //if (KernelSize > img.Width / 2) throw new ArgumentException("KernelSize < img.Width / 2");
            //if (2 > KernelSize) throw new ArgumentException("2 <= KernelSize");

            int Ksize = (int)KernelSize;
            int Stride = Ksize;

            // サーチ幅
            int Nw = (int)Math.Floor((double)W / Ksize);
            int Nh = (int)Math.Floor((double)H / Ksize);

            double sum = 0;

            for (int i = 0; i < Nw; i++)
            {
                int x = i * Ksize;

                for (int j = 0; j < Nh; j++)
                {
                    int y = j * Ksize;

                    double v = Sum(Ksize, x, y, BinalyImage);

                    // [みそ] クリップする
                    if (0 < v) v = 1;

                    sum += v;
                }
            }


            return sum;

        }

        /// <summary>
        /// SUM
        /// </summary>
        /// <param name="Ksize"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="BinalyImage"></param>
        /// <returns></returns>
        private static int Sum(int Ksize, int x0, int y0, in int[,] BinalyImage)
        {
            // 畳み込みカーネル All 1/(Ksize*Ksize)
            int sum = 0;

            for (int ki = 0; ki < Ksize; ki++)
            {
                for (int kj = 0; kj < Ksize; kj++)
                {
                    // 0 or 1
                    int value = BinalyImage[x0 + ki, y0 + kj];

                    // sum
                    sum += value;
                }
            }

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
