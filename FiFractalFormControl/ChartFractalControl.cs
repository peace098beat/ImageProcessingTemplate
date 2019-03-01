using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FiFractalFormControl
{

    /// <summary>
    /// log(N(e)) - log2(e) グラフ
    /// LogYXチャート
    /// </summary>
    public partial class ChartFractalControl : UserControl
    {
        public float xmin;
        public float xmax;
        public float ymin;
        public float ymax;

        public List<double> PlotDataX;
        public List<double> PlotDataY;

        public ChartFractalControl()
        {
            InitializeComponent();

            // サイズ
            this.Size = new Size(149, 82);

            // 背景色
            this.BackColor = Color.FromArgb(37, 53, 73);

            // 設定値
            this.xmin = 0f; // log2(2^0) = 0
            this.xmax = 5f; // log2(2^5) = 5
            this.ymin = -0.01f;
            this.ymax = 1000f;

            // データ
            this.PlotDataX = new List<double>();
            this.PlotDataY = new List<double>();
        }


        // 座標変換

        internal float Nx(float x)
        {
            return (float)(x - xmin) / (xmax - xmin);
        }

        internal float Ny(float y)
        {
            return (float)(y - ymin) / (ymax - ymin);
        }

        internal int Xi(float x)
        {
            return (int)(Width * Nx(x));
        }

        internal int Yi(float y)
        {
            return Height - (int)(Height * Ny(y));
        }

        /// <summary>
        /// データの追加. X=e, Y=N(e)
        /// </summary>
        internal void AddPoint(int e, double Ne)
        {
            // -- 仕様 --
            // eは2の階乗のみ可能
            if ((e & (e - 1)) != 0)
            {
                throw new ArgumentException($"eは2のべき乗にしてください : e={e}");
            }

            if (Ne == 0) Ne = 0.000001;


            double log2_e = Math.Log(e, 2);
            double log2_Ne = Math.Log(Ne, 2);


            this.PlotDataX.Add(log2_e);
            this.PlotDataY.Add(log2_Ne);
        }

        internal void Reset()
        {
            // データ
            this.PlotDataX = new List<double>();
            this.PlotDataY = new List<double>();
        }

        public double GetFractalNumber()
        {
            if (this.PlotDataX == null) throw new ArgumentException();
            if (this.PlotDataY == null) throw new ArgumentException();
            if (this.PlotDataX.Count == 0 || this.PlotDataY.Count == 0) throw new ArgumentException();
            if (this.PlotDataX.Count != this.PlotDataY.Count) throw new ArgumentException($"Plotdataの長さが違うよ");


            // 変数
            int N = this.PlotDataX.Count;
            double[] X = this.PlotDataX.ToArray<double>();
            double[] Y = this.PlotDataY.ToArray<double>();

            // 平均値
            double X_ave = this.PlotDataX.Average();
            double Y_ave = this.PlotDataY.Average();

            // 共分散Sxy
            double Sxy = 0;
            for (int i = 0; i < N; i++)
            {
                Sxy += (X[i] - X_ave) * (Y[i] - Y_ave) / N;
            }

            // 分散
            double Sxx = 0;
            for (int i = 0; i < N; i++)
            {
                Sxx += (X[i] - X_ave) * (X[i] - X_ave) / N;
            }

            // 傾き
            double a = Sxy / Sxx ;

            return a;
        }

        /// <summary>
        /// this.Reflesh()で再描画される
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.PlotDataX == null) throw new ArgumentException();
            if (this.PlotDataY == null) throw new ArgumentException();

            if (this.PlotDataX.Count == 0 || this.PlotDataY.Count == 0)
            {
                Debug.WriteLine($"長さが0です. {PlotDataX.Count}:{PlotDataY.Count}");
                return;
            }

            if (this.PlotDataX.Count != this.PlotDataY.Count) throw new ArgumentException($"Plotdataの長さが違うよ");


            Graphics g = e.Graphics;

            this.xmin = (float)this.PlotDataX.Min();
            this.xmax = (float)this.PlotDataX.Max();
            this.ymin = (float)this.PlotDataY.Min();
            this.ymax = (float)this.PlotDataY.Max();



            int Np = this.PlotDataX.Count;

            Point[] ps = new Point[Np];

            for (int i = 0; i < Np; i++)
            {
                int xi = Xi((float)this.PlotDataX[i]);
                int yi = Yi((float)this.PlotDataY[i]);

                ps[i] = new Point(xi, yi);

            }

            // -------- 折れ線を引く
            Pen pen = Pens.White;
            g.DrawLines(pen, ps);


            // -------- ドット円を引く
            for (int i = 0; i < Np; i++)
            {
                g.FillEllipse(Brushes.Red, ps[i].X, ps[i].Y, 5, 5);
            }

            // -------- 文字を引く
            double D = this.GetFractalNumber();
            string drawString = $"D={D.ToString("F3")}";
            using (Font fnt = new Font("ＭＳ ゴシック", 10))
            {
                int w = this.Width;
                int h = this.Height;
                int sw = 75;
                int sh = 20;
                RectangleF rect = new RectangleF(w-sw, 5, sw, sh);

                g.DrawString(drawString, fnt, Brushes.Red, rect);
            }
               
            // -------- 後処理

        }


    }
}
