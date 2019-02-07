using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ImageProcessingTemplate
{
    public partial class ChartHistogramHSVControl : ChartHistogramRGBControl
    {

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.PointsStack == null) return;
            if (this.PointsStack.Count() <= 0) return;

            Graphics g = e.Graphics;



            foreach (KeyValuePair<string, float[]> KeyValues in this.PointsStack)
            {
                Debug.WriteLine(KeyValues.ToString());

                Point[] ps = new Point[this.nbin];

                string Stackname = KeyValues.Key;

                Pen pen = Pens.White;


                switch (Stackname)
                {
                    case "H":
                        pen = null;
                        break;
                    case "S":
                        pen = Pens.Yellow;
                        break;
                    case "V":
                        pen = Pens.White;
                        break;
                    default:
                        pen = Pens.White;
                        break;
                }

                float[] hist = KeyValues.Value;

                hist[0] = 0;
                xmin = -1;
                xmax = hist.Length + 1;

                for (int i = 0; i < hist.Length; i++)
                {
                    int xi = Xi(i);
                    int yi = Yi(hist[i]);
                    ps[i] = new Point(xi, yi);

                    // H の場合の処理
                    if (Stackname == "H")
                    {
                        Point p0 = new Point(xi, Yi(0));
                        float h = (float)i / hist.Length;
                        Color col = Color.FromArgb(128, ImageColorProc.HSVtoRGB(h, 1, 1));
                        pen = new Pen(col);
                        g.DrawLine(pen, p0, ps[i]);
                    }

                }

                // ------------ 折れ線を引く

                // H の場合の処理
                if (Stackname == "H"){
                    g.DrawLines(Pens.Gray, ps);
                    continue;
                }

                // 折れ線を引く
                g.DrawLines(pen, ps);


            }
        }
    }
}
