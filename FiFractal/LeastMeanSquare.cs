using System;
using System.Linq;

namespace FiFractal
{
    public class LeastMeanSquare
    {
        public double a;
        public double b;
        public double loss;

        public LeastMeanSquare(double[] X, double[] Y)
        {
            // 例外処理
            if (X.Length != Y.Length) throw new ArgumentException($"Defferent Lenght X[{X.Length}] & Y[{Y.Length}]");
            if (X.Length <= 0) throw new ArgumentException("Must Length > 0");

            int N = X.Length;

            // 平均
            double X_ave = X.Average();
            double Y_ave = Y.Average();

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
            a = Sxy / Sxx;
            b = Y_ave - a * X_ave;

            // 2乗誤差
            loss = 0;
            for (int i = 0; i < N; i++)
            {
                loss += (Y[i] - (a * X[i] + b)) * (Y[i] - (a * X[i] + b));

            }
        }

    }
}
