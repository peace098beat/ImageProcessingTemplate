using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiFractal
{
    /// <summary>
    /// 画像の基本処理
    /// </summary>
    static public class Images
    {
        /// <summary>
        /// 開く
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static public Bitmap FromFile(string path)
        {
            return (Bitmap)Bitmap.FromFile(path);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="path"></param>
        /// <param name="format"></param>
        static public void Save(Bitmap bitmap, string path, System.Drawing.Imaging.ImageFormat format)
        {
            using (System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate))
            {
                bitmap.Save(fs, format);
            }
        }

        /// <summary>
        /// 拡大縮小. アスペクト比は維持しない.
        /// </summary>
        static public Bitmap Scale(Bitmap OriginBitmap, int Width, int Height)
        {
            // 変換後のbitmap
            Bitmap ScaledBitmap = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(ScaledBitmap))
            {
                using (Bitmap ClonedOriginBitmap = (Bitmap)OriginBitmap.Clone())
                {
                    // アルゴリズム
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    // 貼り付け
                    g.DrawImage(ClonedOriginBitmap, 0, 0, ScaledBitmap.Width, ScaledBitmap.Height);
                }
            }

            return ScaledBitmap;
        }


        /// <summary>
        /// 正方形でクリップ
        /// </summary>
        static public Bitmap SqureClip(in Bitmap OriginBitmap)
        {

            // Clipされた画像のサイズ
            Rectangle ClipRect;

            if (OriginBitmap.Width < OriginBitmap.Height)
            {
                // 縦長
                // ---
                // | |
                // --- <- Width
                // | |
                // | |
                // | |
                // ---
                ClipRect = new Rectangle(0, 0, OriginBitmap.Width, OriginBitmap.Width);

            }
            else
            {
                // 横長
                //  ---------------
                // |   |          |
                //  ---------------
                //     ^-Height
                ClipRect = new Rectangle(0, 0, OriginBitmap.Height, OriginBitmap.Height);
            }

            // クリップ領域をクローン!!
            Bitmap ClipedBitmap = OriginBitmap.Clone(ClipRect, OriginBitmap.PixelFormat);


            // もしスケーリングするなら..
            return ClipedBitmap;

        }
    }
}
