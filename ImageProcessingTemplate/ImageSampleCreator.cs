using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingTemplate
{
    static public class ImageSampleCreator
    {
        static public void CreateDotPaturn(out Bitmap bmp, string ColorName = "R")
        {
            // 新規定義
            bmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);

            //1ピクセルあたりのバイト数を取得する
            PixelFormat pixelFormat = bmp.PixelFormat;
            int pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;
            if (pixelSize < 3 || 4 < pixelSize)
            {
                throw new ArgumentException(
                    "1ピクセルあたり24または32ビットの形式のイメージのみ有効です。",
                    "img");
            }

            //Bitmapをロックする
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadWrite,
                pixelFormat);

            if (bmpData.Stride < 0)
            {
                bmp.UnlockBits(bmpData);
                throw new ArgumentException("ボトムアップ形式のイメージには対応していません。", "img");
            }

            //ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * bmp.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);


            ColorName = ColorName.ToUpper();
            byte R_flg = 0;
            byte G_flg = 0;
            byte B_flg = 0;
            switch (ColorName)
            {
                case "R":
                    R_flg = 1;
                    break;
                case "G":
                    G_flg = 1;
                    break;
                case "B":
                    B_flg = 1;
                    break;
                default:
                    break;
            }

            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;

                    //青、緑、赤の色を変更する
                    pixels[pos + 0] = (byte)(B_flg * x);
                    pixels[pos + 1] = (byte)(G_flg * x);
                    pixels[pos + 2] = (byte)(R_flg * x);
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);
            //ロックを解除する
            bmp.UnlockBits(bmpData);
        }

        static public void SaveBitmap(ref Bitmap bmp)
        {


        }

        /// <summary>
        /// HSV平面を表示
        /// </summary>
        public static void CreateHSV(out Bitmap bmp)
        {
            // 新規定義
            bmp = new Bitmap(256, 256, PixelFormat.Format24bppRgb);

            //Bitmapをロックする
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                bmp.PixelFormat);

            //1ピクセルあたりのバイト数を取得する
            PixelFormat pixelFormat = bmp.PixelFormat;
            int pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;

            //ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * bmp.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

            // HSV -> RGB

            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;

                    float u = (x / (float)(bmpData.Width - 1));
                    float w = (y / (float)(bmpData.Height - 1));

                    Color c = HSVtoRGB(u, 1.0f, w);

                    //青、緑、赤の色を変更する
                    pixels[pos + 0] = (byte)(c.B);
                    pixels[pos + 1] = (byte)(c.G);
                    pixels[pos + 2] = (byte)(c.R);
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);
            //ロックを解除する
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// HSVtoRGB
        /// </summary>
        /// <param name="h">0-360</param>
        /// <param name="s">0-1</param>
        /// <param name="v">0-1</param>
        /// <returns></returns>
        private static Color HSVtoRGB(float h, float s, float v)
        {
            // (float h, float s, float v)
            float r = v;
            float g = v;
            float b = v;
            if (s > 0.0f)
            {
                h *= 6.0f;
                int i = (int)h;
                float f = h - (float)i;
                switch (i)
                {
                    default:
                    case 0:
                        g *= 1 - s * (1 - f);
                        b *= 1 - s;
                        break;
                    case 1:
                        r *= 1 - s * f;
                        b *= 1 - s;
                        break;
                    case 2:
                        r *= 1 - s;
                        b *= 1 - s * (1 - f);
                        break;
                    case 3:
                        r *= 1 - s;
                        g *= 1 - s * f;
                        break;
                    case 4:
                        r *= 1 - s * (1 - f);
                        g *= 1 - s;
                        break;
                    case 5:
                        g *= 1 - s;
                        b *= 1 - s * f;
                        break;
                }
            }
            byte br = (byte)(255 * r);
            byte bg = (byte)(255 * g);
            byte bb = (byte)(255 * b);
            return Color.FromArgb(br, bg, bb);
        }
    }
}
