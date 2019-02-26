using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FiFractal
{
    public partial class BitmapConverter
    {
        /// <summary>
        /// 二値化 ※ 内部にて1/3グレースケール化する
        /// </summary>
        /// <param name="img"></param>
        /// <param name="thr"></param>
        static public void Binalize(ref Bitmap img, byte thr)
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

            //または次のように元の画像とは異なるPixelFormatでLockBitsすることも可能
            //この場合、UnlockBitsで元のPixelFormatに戻る
            //ただし、元のPixelFormatとLockBits時のPixelFormatが異なる場合は、
            //変更した色とは異なる色になる可能性がある
            //pixelFormat = PixelFormat.Format32bppArgb;
            //pixelSize = 4;

            // ************************************************************* //
            // Bitmapをロックする
            // ************************************************************* //
            BitmapData bmpData = img.LockBits(
                new Rectangle(0, 0, img.Width, img.Height),
                ImageLockMode.ReadWrite,
                pixelFormat);

            if (bmpData.Stride < 0)
            {
                img.UnlockBits(bmpData);
                throw new ArgumentException(
                    "ボトムアップ形式のイメージには対応していません。",
                    "img");
            }

            // ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * img.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

            // 配列操作
            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;

                    byte B = pixels[pos + 0];
                    byte G = pixels[pos + 1];
                    byte R = pixels[pos + 2];

                    byte Gray = (byte)((R + G + B) / 3);

                    if (Gray < thr)
                    {
                        Gray = 0;
                    }
                    else
                    {
                        Gray = 255;
                    }

                    pixels[pos + 0] = Gray; // B
                    pixels[pos + 1] = Gray; // G
                    pixels[pos + 2] = Gray; // R
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);
            // ************************************************************* //
            img.UnlockBits(bmpData);


        }
    }
}
