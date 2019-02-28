using System;
using System.Diagnostics;
using System.Drawing;

namespace FiFractal
{
    public class PixelCounting
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


        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="GrayScale">グレースケール化したBitmap</param>
        public PixelCounting(Bitmap GrayScale)
        {
            sw = Stopwatch.StartNew();

            // byte[,]配列化
            byte[,] ByteArray = FiFractal.BitmapConverter.BitmapToByte2D(GrayScale);

            // エラー処理
            if (ByteArray.GetLength(0) != ByteArray.GetLength(1)) throw new ArgumentException("BitmapサイズはWxHおなじにしてください");

            // ピクセルカウント 2^0 - 2^n < Width
            PixelCount(ByteArray);

            // 最小二乗近似
            LMS = new FiFractal.LeastMeanSquare(this.Log2X, this.Log2Y);

            // フラクタル次元Dを算出
            this.D = (-1) * LMS.a;

            // 時間計測完了
            sw.Stop();

        }

        /// <summary>
        /// カウントします　
        /// </summary>
        /// <param name="image"></param>
        private void PixelCount(byte[,] image)
        {
            // 2^ 2, 3, 4, 5, 6, 7
            // =  4, 8,16,32,64,128
            byte[] Threshs = new byte[] { 4, 8, 16, 32, 64, 128 };
            int N = Threshs.Length;

            this.X = new double[N];
            this.Y = new double[N];

            for (byte i = 0; i < N; i++)
            {
                byte thr = Threshs[i];

                // THR以上の数をカウント
                int count = BinalyCount(thr, image);

                if (count == 0) Debug.WriteLine("カウントが0でした。Logがnullになります");

                this.X[i] = (int)thr;
                this.Y[i] = count;
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
        /// 二値化画像に対して、総数カウントする. 
        /// </summary>
        private static int BinalyCount(byte thr, byte[,] image)
        {
            int W = image.GetLength(0);
            int H = image.GetLength(1);

            int sum = 0;

            for (int x = 0; x < W; x++)
            {
                for (int y = 0; y < H; y++)
                {

                    byte b = image[x, y];

                    if (thr < b)
                    {
                        sum += 1;
                    }


                }
            }
            return sum;
        }


        /// <summary>
        /// CSVで吐き出しー
        /// </summary>
        public void WriteCsv(string path)
        {
            if (System.IO.Path.GetExtension(path) != ".csv")
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
            return String.Format("[PixelCount] D:{0:F3}, LMS.a:{1:F2}, .b:{2:F2}, .loss:{3}, time:{4}", D, LMS.a, LMS.b, LMS.loss, sw.Elapsed);
        }

    }
}
