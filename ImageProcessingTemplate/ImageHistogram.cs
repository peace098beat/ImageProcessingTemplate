using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingTemplate
{
    public class ImageHistogram
    {
        public int N_BINS;

        public Histogram R;
        public Histogram G;
        public Histogram B;

        public Histogram H;
        public Histogram S;
        public Histogram V;

        public ImageHistogram(ref Bitmap img)
        {
            this.N_BINS = 256;

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



            this.R = new Histogram(N_BINS);
            this.B = new Histogram(N_BINS);
            this.G = new Histogram(N_BINS);

            this.H = new Histogram(N_BINS);
            this.S = new Histogram(N_BINS);
            this.V = new Histogram(N_BINS);

            unsafe
            {
                byte* pixelPtr = (byte*)bmpData.Scan0;
                for (int y = 0; y < bmpData.Height; y++)
                {
                    for (int x = 0; x < bmpData.Width; x++)
                    {
                        //ピクセルデータでのピクセル(x,y)の開始位置を計算する
                        int pos = y * bmpData.Stride + x * pixelSize;

                        byte B = pixelPtr[pos];
                        byte G = pixelPtr[pos + 1];
                        byte R = pixelPtr[pos + 2];

                        this.R.Add(R);
                        this.G.Add(G);
                        this.B.Add(B);

                        Color c = Color.FromArgb(R, G, B);
                        byte H = (byte)(255 * c.GetHue()/360.0);
                        byte S = (byte)(255 * c.GetSaturation());
                        byte V = (byte)(255 * c.GetBrightness());

                        this.H.Add(H);
                        this.S.Add(S);
                        this.V.Add(V);

                    }
                }
            }


            // ---------- ロックを解除する
            img.UnlockBits(bmpData);


        }
    }

    public class Histogram
    {
        public int[] values;
        private int wbin;
        private int nbin;

        public Histogram(int nbin)
        {
            this.nbin = nbin;
            values = new int[nbin];
            wbin = 256 / nbin;
        }

        public void Add(byte value)
        {
            if (value < 0 || 255 < value)
            {
                throw new ArgumentException("valueは0-255にしてください");
            }

            int index = value / wbin;

            values[index]++;
        }

        public int TotalNumber
        {
            get
            {
                int sum = 0;
                for (int i = 0; i < values.Length; i++)
                {
                    sum += values[i];
                }
                return sum;
            }
        }
        public float[] GetNorm
        {
            get
            {
                float[] norm = new float[nbin];
                int N = this.TotalNumber;
                for (int i = 0; i < norm.Length; i++)
                {
                    norm[i] = (float)values[i] / N;
                }
                return norm;
            }
        }
    }
}
