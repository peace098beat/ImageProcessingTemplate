using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiFractalFormControl
{
    public static class ImageColorProc
    {
        /// <summary>
        /// HSVtoRGB
        /// </summary>
        /// <param name="h">0-360</param>
        /// <param name="s">0-1</param>
        /// <param name="v">0-1</param>
        public static Color HSVtoRGB(int h, float s, float v)
        {
            float h_float = (float)h / 360f;

            return HSVtoRGB(h_float, s, v);
        }

        /// <summary>
        /// HSVtoRGB
        /// </summary>
        /// <param name="h">0-1 (0-360)</param>
        /// <param name="s">0-1</param>
        /// <param name="v">0-1</param>
        public static Color HSVtoRGB(float h, float s, float v)
        {
            if (h < 0 || 1 < h) throw new ArgumentException("hは0から1にしてね");
            // (float h, float s, float v)
            float r = v;
            float g = v;
            float b = v;
            if (s > 0.0f)
            {
                h *= 6.0f;
                int i = (int)h;
                float f = h - (float)i;
                //int i = (int)h / 6;
                switch (i)
                {
                    default:
                    case 0:
                        g *= 1 - s * (1 - f);
                        b *= 1 - s;
                        break;
                    case 1:
                        r *= 1 - s * f;
                        b *= 1 - s;
                        break;
                    case 2:
                        r *= 1 - s;
                        b *= 1 - s * (1 - f);
                        break;
                    case 3:
                        r *= 1 - s;
                        g *= 1 - s * f;
                        break;
                    case 4:
                        r *= 1 - s * (1 - f);
                        g *= 1 - s;
                        break;
                    case 5:
                        g *= 1 - s;
                        b *= 1 - s * f;
                        break;
                }
            }
            byte br = (byte)(255 * r);
            byte bg = (byte)(255 * g);
            byte bb = (byte)(255 * b);
            return Color.FromArgb(br, bg, bb);
        }
    }
}
