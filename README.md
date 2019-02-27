# ImageProcessingTemplate
はじめての画像処理


## ゴール

- ゴール1: フラクタル次元測定器を作る
- ゴール2: アナライザー画面を作る
- ゴール3: カメラでリアルタイム測定


## フラクタル次元測定手法

1. ボックスカウント (バイナリ化)
2. ピクセルカウント (バイナリ化 + スレッシュホールドスイープ)


## ImageProccess

- 画像の読み込み/表示/解析
- カメラ画像の撮影/表示/保存/解析
- グレースケール化
- ２値化


## 解析機能
- フラクタル次元の計算 (ボックスカウンティング)
	
	- Images.BitmapToFloat2D
	- Images.Float2Bitmap(float[x,y][c])


	- float[,] Gray = GrayScale(Image)
	- int[,] Binaly = Binaly(float[,] Gray, int Thresh)

	- int Ct = BoxCountBackend(float[,] Binaly, byte Level)

	- new BoxCount(Bitmap image, byte max_level)
	- BoxCount.Calc(int Thr)
	- BoxCount.D
	- BoxCount.LeastMeanSquare.a
	- BoxCount.LeastMeanSquare.b
	- BoxCount.LeastMeanSquare.loss

	- int D = LeastMeanSquare(float[] X, float[] Y);

- フラクタル次元の計算 (ピクセルカウント)

## 可視化機能
- ヒストグラム(RGB, HSV)


## Gray Fractal Analyser

グレースケール画像のフラクタル解析の実装



# コーディングAPI

リソース

byte[,]

// まずはfloat[,]でやる
// FiFractal.ImageArrayContainer
// FiFractal.ImageArrayContainer.RGBA
// FiFractal.ImageArrayContainer.RGB
// FiFractal.ImageArrayContainer.G
// FiFractal.ImageArrayContainer.HSV


// Convert
byte[,] Bitmap2Byte2D(ref Bitmap)
float[,] Bitmap2Float2D(ref Bitmap)


// Color Trans
BitmapConverter.GrayScale(ref Bitmap)


// Analys
Histgram(ColorArray ColorArray.R)
	float[] .Bins
	float[] .Px

LeastMeanSquare()


// Fractal
BoxCounting()
PixcepCounting()


# GPUからCPU側へメモリを転送する方法
glReadPixels() を使うか Pixel Buffer Object