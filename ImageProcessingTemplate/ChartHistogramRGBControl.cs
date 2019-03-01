using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace FiFractalFormControl
{
    public partial class ChartHistogramRGBControl : UserControl
    {
        public int nbin { set; get; }

        public float xmin;
        public float xmax;
        public float ymin;
        public float ymax;

        public bool IsLog = false;

        public ContextMenuStrip contextMenuStrip;
        public Dictionary<string, float[]> PointsStack;

        public ChartHistogramRGBControl()
        {
            InitializeComponent();



            this.BackColor = Color.FromArgb(37, 53, 73);

            // コンテキストメニューを生成します
            this.contextMenuStrip = new ContextMenuStrip();
            this.contextMenuStrip.Items.Add("Log/Linear", null, this.ContextMenu1_Click);
            this.contextMenuStrip.Items.Add("メニュー2", null, this.ContextMenu2_Click);
            this.contextMenuStrip.Items.Add("メニュー3", null, ContextMenu3_Click);

            // このフォームのコンテキストメニューとして登録しておきます
            this.ContextMenuStrip = this.contextMenuStrip;

            // プロット用データ
            this.PointsStack = new Dictionary<string, float[]>();

            // 設定値
            this.nbin = 256;

            // 設定値
            this.xmin = -0.5f;
            this.xmax = 256f;
            this.ymin = -0.01f;
            this.ymax = 1.0f;

            // 設定値
            this.IsLog = true;
        }

        // 右クリック

        internal void ContextMenu1_Click(object sender, EventArgs e)
        {
            if(IsLog)
            {
                IsLog = false;
            }
            else
            {
                IsLog = true;
            }
            this.Refresh();
        }


        internal void ContextMenu2_Click(object sender, EventArgs e)
        {
        }

        internal void ContextMenu3_Click(object sender, EventArgs e)
        {
           
        }

        // 座標変換

        internal float Nx(float x)
        {
            return (float)(x - xmin) / (xmax - xmin);
        }

        internal float Ny(float y)
        {
            if (IsLog)
            {
                if (y > 1.0) y = 1.0f;
                if (y <= 0.0) y = 0.000000001f;
                y = (float)Math.Log10( y );
                float _ymin = -5;
                float _ymax = 0;
                return (float)(y - _ymin) / (_ymax - _ymin);
            }

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


        internal void AddPoints(string StackName, float[] Points)
        {
            if (Points.Length != this.nbin)
            {
                throw new ArgumentException($"nbin != Points.length({Points.Length})");
            }

            if (this.PointsStack.ContainsKey(StackName))
            {
                this.PointsStack.Remove(StackName);
            }
            this.PointsStack.Add(StackName, Points);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.PointsStack == null) return;
            if (this.PointsStack.Count() <= 0) return;

            Graphics g = e.Graphics;


            foreach (KeyValuePair<string, float[]> KeyValues in this.PointsStack)
            {
                Point[] ps = new Point[this.nbin];

                string Stackname = KeyValues.Key;

                Pen pen = Pens.Black;

            
                switch (Stackname)
                {
                    case "R":
                        pen = Pens.Red;
                        pen = new Pen(Color.FromArgb(255, 80, 30, 120));
                        pen = new Pen(Color.FromArgb(255, 200, 30, 120));
                        break;
                    case "G":
                        pen = Pens.Green;
                        pen = new Pen(Color.FromArgb(255, 30,  120, 81));
                        pen = new Pen(Color.FromArgb(255, 30,  220, 81));
                        break;
                    case "B":
                        pen = Pens.Blue;
                        pen = new Pen(Color.FromArgb(255, 74, 106,  147));
                        pen = new Pen(Color.FromArgb(255, 74, 106,  220));

                        break;
                    default:
                        pen = Pens.Black;
                        break;
                }

                float[] hist = KeyValues.Value;

                hist[0] = 0;

                for (int i = 0; i < hist.Length; i++)
                {
                    int xi = Xi(i);
                    int yi = Yi(hist[i]);
                    ps[i] = new Point(xi, yi);
                }

                // 戻り点
                ps[hist.Length - 1] = new Point(Xi(xmax), Yi(ymin));
                // ------------ 

                // ------------ 塗りつぶし
                Color gC1 = Color.FromArgb(1, pen.Color);
                float h1 = pen.Color.GetHue() / 360f;
                float s1 = pen.Color.GetSaturation();
                float v1 = pen.Color.GetBrightness();
                s1 = 1.0f;
                v1 = 0.5f;
                Color GradColor1 = Color.FromArgb(64, ImageColorProc.HSVtoRGB(h1, s1, v1));


                Color gC2 = Color.FromArgb(200, pen.Color);
                float h2 = pen.Color.GetHue() / 360f;
                float s2 = pen.Color.GetSaturation();
                float v2 = pen.Color.GetBrightness();
                s2 = 0.3f;
                v2 = 1.0f;
                Color GradColor2 = Color.FromArgb(128, ImageColorProc.HSVtoRGB(h2, s2, v2));


                LinearGradientBrush gBrush = new LinearGradientBrush(this.ClientRectangle, GradColor1, GradColor2, LinearGradientMode.Vertical);

                // 塗りつぶし
                g.FillPolygon(gBrush, ps, FillMode.Winding);

                // ------------ 折れ線を引く
                g.DrawLines(pen, ps);


                // 後処理
                gBrush.Dispose();

            }
        }
    }
}
