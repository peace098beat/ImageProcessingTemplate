using System;
using System.Drawing;
using System.Drawing.Imaging;


namespace FiFractal
{
    public partial class BitmapConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="index">0:Blue, 1:Green, 2:Red</param>
        /// <returns></returns>
        static public byte[,] BitmapToByte2D(in Bitmap img, int index=0)
        {
            if (0 < index && index <= 2) throw new ArgumentException("indexが範囲外. 0:Blue, 1:Green, 2:Red");

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


            // 返却先
            byte[,] ResultArray = new byte[bmpData.Width, bmpData.Height];


            // 配列操作
            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;

                    //青、緑、赤の色を変更する
                    //pixels[pos + 0] = (byte)(255 - pixels[pos + 0]);
                    //pixels[pos + 1] = (byte)(255 - pixels[pos + 1]);
                    //pixels[pos + 2] = (byte)(255 - pixels[pos + 2]);

                    ResultArray[x, y] = pixels[pos + index];
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);
            // ************************************************************* //
            img.UnlockBits(bmpData);


            // 返却
            return ResultArray;



        }
    }
}
