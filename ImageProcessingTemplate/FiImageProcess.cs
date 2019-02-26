using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingTemplate
{
    static class FiImageProcess
    {
        public static void ChangeToNegativeImage(Bitmap img)
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

            //Bitmapをロックする
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

            //ピクセルデータをバイト型配列で取得する
            IntPtr ptr = bmpData.Scan0;
            byte[] pixels = new byte[bmpData.Stride * img.Height];
            System.Runtime.InteropServices.Marshal.Copy(ptr, pixels, 0, pixels.Length);

            //すべてのピクセルの色を反転させる
            for (int y = 0; y < bmpData.Height; y++)
            {
                for (int x = 0; x < bmpData.Width; x++)
                {
                    //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                    int pos = y * bmpData.Stride + x * pixelSize;
                    //青、緑、赤の色を変更する
                    pixels[pos] = (byte)(255 - pixels[pos]);
                    pixels[pos + 1] = (byte)(255 - pixels[pos + 1]);
                    pixels[pos + 2] = (byte)(255 - pixels[pos + 2]);
                }
            }

            //ピクセルデータを元に戻す
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptr, pixels.Length);

            //アンセーフコードを使うと、以下のようにもできる
            //unsafe
            //{
            //    byte* pixelPtr = (byte*)bmpData.Scan0;
            //    for (int y = 0; y < bmpData.Height; y++)
            //    {
            //        for (int x = 0; x < bmpData.Width; x++)
            //        {
            //            //ピクセルデータでのピクセル(x,y)の開始位置を計算する
            //            int pos = y * bmpData.Stride + x * pixelSize;
            //            //青、緑、赤の色を変更する
            //            pixelPtr[pos] = (byte)(255 - pixelPtr[pos]);
            //            pixelPtr[pos + 1] = (byte)(255 - pixelPtr[pos + 1]);
            //            pixelPtr[pos + 2] = (byte)(255 - pixelPtr[pos + 2]);
            //        }
            //    }
            //}

            //ロックを解除する
            img.UnlockBits(bmpData);
        }

        public static bool CheckPixelFormat(ref Bitmap img)
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
            else
            {
                return true;
            }

        }


        public static void GrayScale(ref Bitmap img)
        {
            // ---------- 1ピクセルあたりのバイト数を取得する
            PixelFormat pixelFormat = img.PixelFormat;

            // ---------- エラー処理
            int pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;
            if (pixelSize < 3 || 4 < pixelSize)
            {
                throw new ArgumentException(
                    "1ピクセルあたり24または32ビットの形式のイメージのみ有効です。", "img");
            }

            // ---------- ロック
            BitmapData bmpData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, pixelFormat);

            // ---------- エラー処理
            if (bmpData.Stride < 0)
            {
                img.UnlockBits(bmpData);
                throw new ArgumentException("ボトムアップ形式のイメージには対応していません。", "img");
            }


            unsafe
            {
                byte* pixelPtr = (byte*)bmpData.Scan0;
                for (int y = 0; y < bmpData.Height; y++)
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                        int pos = y * bmpData.Stride + x * pixelSize;


                        byte R = pixelPtr[pos];
                        byte G = pixelPtr[pos + 1];
                        byte B = pixelPtr[pos + 2];

                        //byte gray = (byte)((0.3 * R + 0.59 * G + 0.11 * B) / 3); // 平均値
                        byte gray = (byte)((R + G + B) / 3); // 平均値

                        pixelPtr[pos] = gray;
                        pixelPtr[pos + 1] = gray;
                        pixelPtr[pos + 2] = gray;

                    }
                }
            }


            // ---------- ロックを解除する
            img.UnlockBits(bmpData);


        }

        public static void Statistics(ref Bitmap img)
        {
            // ---------- 1ピクセルあたりのバイト数を取得する
            PixelFormat pixelFormat = img.PixelFormat;

            // ---------- エラー処理
            int pixelSize = Image.GetPixelFormatSize(pixelFormat) / 8;
            if (pixelSize < 3 || 4 < pixelSize)
            {
                throw new ArgumentException(
                    "1ピクセルあたり24または32ビットの形式のイメージのみ有効です。", "img");
            }


            // ---------- ロック
            BitmapData bmpData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, pixelFormat);

            // ---------- エラー処理
            if (bmpData.Stride < 0)
            {
                img.UnlockBits(bmpData);
                throw new ArgumentException("ボトムアップ形式のイメージには対応していません。", "img");
            }

            // 合計, 


            unsafe
            {
                byte* pixelPtr = (byte*)bmpData.Scan0;
                for (int y = 0; y < bmpData.Height; y++)
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                        int pos = y * bmpData.Stride + x * pixelSize;

                        byte R = pixelPtr[pos];
                        byte G = pixelPtr[pos + 1];
                        byte B = pixelPtr[pos + 2];

                        //c = Color.FromArgb(R, G, B);
                        //byte H = (byte)(255 * c.GetHue());
                        //byte S = (byte)(255 * c.GetSaturation());
                        //byte V = (byte)(255 * c.GetBrightness());

                    }
                }
            }


            // ---------- ロックを解除する
            img.UnlockBits(bmpData);


        }

    }
}
