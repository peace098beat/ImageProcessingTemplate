using System;
using System.Diagnostics;
using System.Drawing;

namespace FiFractal
{
    public class BoxCounting
    {
        public double D;
        public LeastMeanSquare LMS;
        public Stopwatch sw;

        // カウント結果
        public double[] X { get; private set; }
        public double[] Y { get; private set; }
        public double[] Log10X { get; private set; }
        public double[] Log10Y { get; private set; }
        public double[] LogX { get; private set; }
        public double[] LogY { get; private set; }
        public double[] Log2X { get; private set; }
        public double[] Log2Y { get; private set; }

        public BoxCounting(Bitmap image, byte Thr)
        {
            sw = Stopwatch.StartNew();

            // 二値化
            Bitmap BinalyBitmap = (Bitmap)image.Clone();
            //FiFractal.BitmapConverter.ToneInverse(ref BinalyBitmap);
            FiFractal.BitmapConverter.Binalize(ref BinalyBitmap, Thr);

            // byte[,]配列化
            byte[,] BinalyArray = FiFractal.BitmapConverter.BitmapToByte2D(BinalyBitmap);

            // エラー処理
            if (BinalyArray.GetLength(0) != BinalyArray.GetLength(1)) throw new ArgumentException("BitmapサイズはWxHおなじにしてください");

            // ボックスカウント 2^0 - 2^n < Width
            BoxCount(BinalyArray);

            // 最小二乗近似
            LMS = new FiFractal.LeastMeanSquare(this.Log2X, this.Log2Y);
            //LMS = new FiFractal.LeastMeanSquare(this.LogX, this.LogY);
            //LMS = new FiFractal.LeastMeanSquare(this.Log10X, this.Log10Y);


            // フラクタル次元Dを算出
            this.D = (-1) * LMS.a;

            // 時間計測完了
            sw.Stop();

        }

        private void BoxCount(byte[,] image)
        {

            // 2^0 ～ 2^MaxKernelまで. 総数はMaxKernel + 1
            int MaxKernelIndex = (int)Math.Floor(Math.Log(image.GetLength(0), 2));

            int N = MaxKernelIndex + 1;

            this.X = new double[N];
            this.Y = new double[N];

            for (int i = 0; i < N; i++)
            {
                uint KernelSize = (uint)Math.Pow(2, i);

                int c = BinalyCount(KernelSize, image);

                if (c == 0) Debug.WriteLine("カウントが0でした。Logがnullになります");

                this.X[i] = (int)KernelSize;
                this.Y[i] = c;
            }


            // Log10(N(r))/Log10(K)
            this.Log10X = new double[N];
            this.Log10Y = new double[N];
            for (int i = 0; i < N; i++)
            {
                this.Log10X[i] = Math.Log(this.X[i], 10);
                this.Log10Y[i] = Math.Log(this.Y[i], 10);
            }

            // Log(N(r))/Log(K)
            this.LogX = new double[N];
            this.LogY = new double[N];
            for (int i = 0; i < N; i++)
            {
                this.LogX[i] = Math.Log(this.X[i]);
                this.LogY[i] = Math.Log(this.Y[i]);
            }

            // Log2(N(r))/Log2(K)
            this.Log2X = new double[N];
            this.Log2Y = new double[N];
            for (int i = 0; i < N; i++)
            {
                this.Log2X[i] = Math.Log(this.X[i], 2);
                this.Log2Y[i] = Math.Log(this.Y[i], 2);
            }

        }

        /// <summary>
        /// 二値化画像に対して、カウントする. 
        /// [TODO] ストライドしません
        /// [TODO] 画像の上下は同じほうがいいかも
        /// </summary>
        private static int BinalyCount(uint KernelSize, byte[,] image)
        {
            int W = image.GetLength(0);
            int H = image.GetLength(1);

            int Ksize = (int)KernelSize;

            int Nw = (int)Math.Floor((double)W / Ksize);
            int Nh = (int)Math.Floor((double)H / Ksize);

            int sum = 0;

            for (int i = 0; i < Nw; i++)
            {
                int x = i * Ksize;

                for (int j = 0; j < Nh; j++)
                {
                    int y = j * Ksize;

                    int exist = PixelExist(Ksize, x, y, image);

                    sum += exist;
                }
            }

            return sum;
        }

        /// <summary>
        /// 1pixelでも存在したら1カウント
        /// </summary>
        private static int PixelExist(int Ksize, int x0, int y0, byte[,] image)
        {
            // カーネル操作
            for (int ki = 0; ki < Ksize; ki++)
            {
                for (int kj = 0; kj < Ksize; kj++)
                {
                    byte v = image[x0 + ki, y0 + kj];

                    // 高速化
                    if (0 < v) return 1;
                }
            }

            // 存在判定 無し
            return 0;

        }

        /// <summary>
        /// CSVで吐き出しー
        /// </summary>
        public void WriteCsv(string path)
        {
            if(System.IO.Path.GetExtension(path) != ".csv")
            {
                path = path + ".csv";
            }

            try
            {
                // appendをtrueにすると，既存のファイルに追記 falseにすると，ファイルを新規作成する
                var append = false;

                // 出力用のファイルを開く
                using (var sw = new System.IO.StreamWriter(path, append))
                {
                    sw.WriteLine("#Date, {0}", DateTime.Now);
                    sw.WriteLine("#Fractal D, {0}", this.D);
                    sw.WriteLine("#LMS.loss, {0}", this.LMS.loss);
                    sw.WriteLine("#LMS.a, {0}", this.LMS.a);
                    sw.WriteLine("#LMS.b, {0}", this.LMS.b);
                    sw.WriteLine("");

                    sw.WriteLine("X,Y,LogX,LogY,Log2X,Log2Y,Log10X,Log10Y");

                    for (int i = 0; i < X.Length; ++i)
                    {
                        sw.WriteLine("{0},{1},{2},{3},{4},{5},{6},{7}", X[i], Y[i], LogX[i], LogY[i], Log2X[i], Log2Y[i], Log10X[i], Log10Y[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 書式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[BoxCount] D:{0:F3}, LMS.a:{1:F2}, .b:{2:F2}, .loss:{3}, time:{4}", D, LMS.a, LMS.b, LMS.loss, sw.Elapsed);
        }

    }
}
