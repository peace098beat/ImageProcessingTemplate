﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

//http://www.fit.ac.jp/elec/lab/lulab/data/c_image_s.pdf


namespace ImageProcessingTemplate
{
    public partial class Form1 : Form
    {

        Bitmap TargetBitmap;
        private Stopwatch sw;

        public Form1()
        {
            InitializeComponent();

            string file_path = @"C:\Users\Public\Pictures\Sample Pictures\kurage.jpg";
            TargetBitmap = (Bitmap)Bitmap.FromFile(file_path);
            pictureBox1.Image = TargetBitmap;
            pictureBox1.Invalidate();


        }

        /****************************/
        /* Fuction コールバック */
        /****************************/
        void SwStart()
        {
            sw = Stopwatch.StartNew();
        }
        void SwStop(string title = "")
        {
            // 計測
            long time = sw.ElapsedMilliseconds;
            this.Text = title + " " + time.ToString() + "[ms]" + Environment.NewLine;
        }

        /****************************/
        /* UIコールバック */
        /****************************/

        /// <summary>
        /// Pictureboxの更新処理
        /// </summary>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (TargetBitmap == null) return;

            // 拡大縮小
            Bitmap scaled_bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(scaled_bmp))
            {
                using (Bitmap origin_bmp = (Bitmap)this.TargetBitmap.Clone())
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                    g.DrawImage(origin_bmp, 0, 0, scaled_bmp.Width, scaled_bmp.Height);
                }

            }
            pictureBox1.Image = scaled_bmp;
        }

        /// <summary>
        /// 画像ファイルを開く
        /// </summary>
        private void button_FileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file_path = ofd.FileName;

                TargetBitmap = (Bitmap)Bitmap.FromFile(file_path);
                pictureBox1.Image = TargetBitmap;
                pictureBox1.Invalidate();
            }
        }

        /// <summary>
        /// 解析
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            if (TargetBitmap == null) return;

            var format = TargetBitmap.PixelFormat;
            textBox_debug.Text = "";
            textBox_debug.Text += format.ToString() + Environment.NewLine;
            textBox_debug.Text += $"W:{TargetBitmap.Width}, H:{TargetBitmap.Height}" + Environment.NewLine;

            SwStart();

            // 画像処理
            var hist = new ImageHistogram(ref TargetBitmap);

            // 計測
            SwStop("hist");

            float[] R_Norm = hist.R.GetNorm;

            this.chartHistogramControl1.AddPoints("R", hist.R.GetNorm);
            this.chartHistogramControl1.AddPoints("G", hist.G.GetNorm);
            this.chartHistogramControl1.AddPoints("B", hist.B.GetNorm);
            this.chartHistogramControl1.Refresh();

            this.chartHistogramHSVControl1.AddPoints("H", hist.H.GetNorm);
            this.chartHistogramHSVControl1.AddPoints("S", hist.S.GetNorm);
            this.chartHistogramHSVControl1.AddPoints("V", hist.V.GetNorm);
            this.chartHistogramHSVControl1.Refresh();



            for (int i = 0; i < hist.N_BINS; i++)
            {
                float ri = hist.R.GetNorm[i];
                float gi = hist.G.GetNorm[i];
                float bi = hist.B.GetNorm[i];
                float hi = hist.H.GetNorm[i];
                float si = hist.S.GetNorm[i];
                float vi = hist.V.GetNorm[i];
                //textBox_debug.Text += $"[{i}] {ri.ToString("F2")} {gi.ToString("F2")} {bi.ToString("F2")}, {hi.ToString("F2")} {si.ToString("F2")} {vi.ToString("F2")}" + Environment.NewLine;
            }

            //pictureBox1.Invalidate();

        }


        /// <summary>
        /// グラデーション画像生成
        /// </summary>
        private void button_Gradation_Click(object sender, EventArgs e)
        {
            // 色選択
            if (comboBox_GradationType.SelectedItem == null) return;
            string C_Flg = (string)comboBox_GradationType.SelectedItem.ToString();
            if (C_Flg == null || C_Flg == "") return;

            // 画像処理
            SwStart();
            ImageSampleCreator.CreateDotPaturn(out TargetBitmap, C_Flg.ToUpper());
            SwStop("Dot");

            // 再描画
            pictureBox1.Invalidate();

        }

        /// <summary>
        /// 名前を付けて画像を保存
        /// </summary>
        private void button_BmpSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName == "") return;

            using (System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile())
            {

                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        TargetBitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        TargetBitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        TargetBitmap.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
            }

        }

        /// <summary>
        /// HSV平面表示
        /// </summary>
        private void button_HSV_Click(object sender, EventArgs e)
        {
            // 色選択


            SwStart();
            // 画像処理

            ImageSampleCreator.CreateHSV(out TargetBitmap);

            SwStop("Dot");

            // 再描画
            pictureBox1.Invalidate();
        }
    }

}

