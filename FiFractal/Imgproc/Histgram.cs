using System;
using System.Diagnostics;

namespace FiFractal.Imgproc
{
    public class Histgram
    {
        public Stopwatch sw;

        public int[] Bins;
        public int[] Frequency;
        public double[] Density;

        public double Mean;
        private double Sum;
        public double Var;
        public double Std;
        public int TotalCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="bins">1～256</param>
        public Histgram(in byte[,] image)
        {
            sw = Stopwatch.StartNew();

            // 初期化
            this.Bins = new int[256];
            this.Frequency = new int[256];
            this.Density = new double[256];
            for (int i = 0; i <= 255; i++)
            {
                this.Frequency[i] = 0;
                this.Bins[i] = i;
                this.Density[i] = 0.0;
            }

            // カウント
            int Nx = image.GetLength(0);
            int Ny = image.GetLength(1);
            int N = Nx * Ny;

            // 総数
            this.TotalCount = N;

            // 平均
            Mean = 0;

            // SUM
            Sum = 0.0;

            // 度数
            for (int x = 0; x < Nx; x++)
            {
                for (int y = 0; y < Ny; y++)
                {
                    byte b = image[x, y];

                    this.Frequency[b]++;

                    this.Mean += (double)b / N;

                    this.Sum += b;
                }
            }

            // 分散
            Var = 0;

            for (int x = 0; x < Nx; x++)
            {
                for (int y = 0; y < Ny; y++)
                {
                    byte b = image[x, y];

                    this.Var = (b - Mean) * (b - Mean) / N;
                }
            }

            // 標準偏差 (RMS)
            Std = Math.Sqrt(Var);

            // 密度
            for (int i = 0; i <= 255; i++)
            {
                this.Density[i] = (double)this.Frequency[i] / N;
            }


            // 時間計測完了
            sw.Stop();
        }

        /// <summary>
        /// CSVで吐き出し
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
                    sw.WriteLine("#Mean, {0}", this.Mean);
                    sw.WriteLine("#Var, {0}", this.Var);
                    sw.WriteLine("#Std, {0}", this.Std);
                    sw.WriteLine("#Sum, {0}", this.Sum);
                    sw.WriteLine("#TotalCount, {0}", this.TotalCount);
                    sw.WriteLine("");

                    sw.WriteLine("Bins, Frequency, Density");
                    for (int i = 0; i < 256; ++i)
                    {
                        sw.WriteLine("{0},{1},{2}", Bins[i], Frequency[i], Density[i]);
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したときエラーメッセージを表示
                System.Diagnostics.Debug.WriteLine(e.Message);
                System.Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// 書式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("[Histgram] Mean:{0:F2}, Var:{1:F2}, Std:{2:F2}, Sum:{3}, TotalCount:{4}, time:{5}", Mean, Var, Std, Sum, TotalCount, sw.Elapsed);
        }

    }

}
