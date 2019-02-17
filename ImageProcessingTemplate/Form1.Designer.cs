namespace ImageProcessingTemplate
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sAVEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_debug = new System.Windows.Forms.TextBox();
            this.button_Gradation = new System.Windows.Forms.Button();
            this.comboBox_GradationType = new System.Windows.Forms.ComboBox();
            this.button_HSV = new System.Windows.Forms.Button();
            this.button_Gray = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button_Histgram = new System.Windows.Forms.Button();
            this.chartHistogramHSVControl1 = new ImageProcessingTemplate.ChartHistogramHSVControl();
            this.chartHistogramControl1 = new ImageProcessingTemplate.ChartHistogramRGBControl();
            this.button_Ana_Fractal = new System.Windows.Forms.Button();
            this.chartFractalControl1 = new ImageProcessingTemplate.ChartFractalControl();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(831, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sAVEToolStripMenuItem,
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(40, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // sAVEToolStripMenuItem
            // 
            this.sAVEToolStripMenuItem.Name = "sAVEToolStripMenuItem";
            this.sAVEToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.sAVEToolStripMenuItem.Text = "SAVE";
            this.sAVEToolStripMenuItem.Click += new System.EventHandler(this.sAVEToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 434);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(831, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(134, 18);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 17);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(315, 266);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // textBox_debug
            // 
            this.textBox_debug.Location = new System.Drawing.Point(593, 29);
            this.textBox_debug.Multiline = true;
            this.textBox_debug.Name = "textBox_debug";
            this.textBox_debug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_debug.Size = new System.Drawing.Size(213, 385);
            this.textBox_debug.TabIndex = 8;
            // 
            // button_Gradation
            // 
            this.button_Gradation.Location = new System.Drawing.Point(6, 18);
            this.button_Gradation.Name = "button_Gradation";
            this.button_Gradation.Size = new System.Drawing.Size(75, 23);
            this.button_Gradation.TabIndex = 10;
            this.button_Gradation.Text = "Gradation";
            this.button_Gradation.UseVisualStyleBackColor = true;
            this.button_Gradation.Click += new System.EventHandler(this.button_Gradation_Click);
            // 
            // comboBox_GradationType
            // 
            this.comboBox_GradationType.FormattingEnabled = true;
            this.comboBox_GradationType.Items.AddRange(new object[] {
            "R",
            "G",
            "B"});
            this.comboBox_GradationType.Location = new System.Drawing.Point(87, 18);
            this.comboBox_GradationType.Name = "comboBox_GradationType";
            this.comboBox_GradationType.Size = new System.Drawing.Size(34, 20);
            this.comboBox_GradationType.TabIndex = 15;
            // 
            // button_HSV
            // 
            this.button_HSV.Location = new System.Drawing.Point(6, 47);
            this.button_HSV.Name = "button_HSV";
            this.button_HSV.Size = new System.Drawing.Size(75, 23);
            this.button_HSV.TabIndex = 17;
            this.button_HSV.Text = "HSV";
            this.button_HSV.UseVisualStyleBackColor = true;
            this.button_HSV.Click += new System.EventHandler(this.button_HSV_Click);
            // 
            // button_Gray
            // 
            this.button_Gray.Location = new System.Drawing.Point(6, 18);
            this.button_Gray.Name = "button_Gray";
            this.button_Gray.Size = new System.Drawing.Size(75, 23);
            this.button_Gray.TabIndex = 20;
            this.button_Gray.Text = "Gray";
            this.button_Gray.UseVisualStyleBackColor = true;
            this.button_Gray.Click += new System.EventHandler(this.button_Gray_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_GradationType);
            this.groupBox1.Controls.Add(this.button_Gradation);
            this.groupBox1.Controls.Add(this.button_HSV);
            this.groupBox1.Location = new System.Drawing.Point(334, 158);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(155, 85);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Create";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_Gray);
            this.groupBox2.Location = new System.Drawing.Point(333, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(155, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Effects";
            // 
            // button_Histgram
            // 
            this.button_Histgram.Location = new System.Drawing.Point(12, 389);
            this.button_Histgram.Name = "button_Histgram";
            this.button_Histgram.Size = new System.Drawing.Size(303, 25);
            this.button_Histgram.TabIndex = 23;
            this.button_Histgram.Text = "Histgram";
            this.button_Histgram.UseVisualStyleBackColor = true;
            this.button_Histgram.Click += new System.EventHandler(this.button_Histgram_Click);
            // 
            // chartHistogramHSVControl1
            // 
            this.chartHistogramHSVControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(53)))), ((int)(((byte)(73)))));
            this.chartHistogramHSVControl1.Location = new System.Drawing.Point(12, 301);
            this.chartHistogramHSVControl1.Name = "chartHistogramHSVControl1";
            this.chartHistogramHSVControl1.nbin = 256;
            this.chartHistogramHSVControl1.Size = new System.Drawing.Size(150, 82);
            this.chartHistogramHSVControl1.TabIndex = 19;
            // 
            // chartHistogramControl1
            // 
            this.chartHistogramControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(53)))), ((int)(((byte)(73)))));
            this.chartHistogramControl1.Location = new System.Drawing.Point(165, 301);
            this.chartHistogramControl1.Name = "chartHistogramControl1";
            this.chartHistogramControl1.nbin = 256;
            this.chartHistogramControl1.Size = new System.Drawing.Size(150, 82);
            this.chartHistogramControl1.TabIndex = 18;
            // 
            // button_Ana_Fractal
            // 
            this.button_Ana_Fractal.Location = new System.Drawing.Point(437, 29);
            this.button_Ana_Fractal.Name = "button_Ana_Fractal";
            this.button_Ana_Fractal.Size = new System.Drawing.Size(75, 27);
            this.button_Ana_Fractal.TabIndex = 24;
            this.button_Ana_Fractal.Text = "Fractal";
            this.button_Ana_Fractal.UseVisualStyleBackColor = true;
            this.button_Ana_Fractal.Click += new System.EventHandler(this.button_Ana_Fractal_Click);
            // 
            // chartFractalControl1
            // 
            this.chartFractalControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(53)))), ((int)(((byte)(73)))));
            this.chartFractalControl1.Location = new System.Drawing.Point(340, 301);
            this.chartFractalControl1.Name = "chartFractalControl1";
            this.chartFractalControl1.Size = new System.Drawing.Size(149, 82);
            this.chartFractalControl1.TabIndex = 25;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(386, 34);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(45, 19);
            this.numericUpDown1.TabIndex = 26;
            this.numericUpDown1.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 457);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.chartFractalControl1);
            this.Controls.Add(this.button_Ana_Fractal);
            this.Controls.Add(this.button_Histgram);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chartHistogramHSVControl1);
            this.Controls.Add(this.chartHistogramControl1);
            this.Controls.Add(this.textBox_debug);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_debug;
        private System.Windows.Forms.Button button_Gradation;
        private System.Windows.Forms.ComboBox comboBox_GradationType;
        private System.Windows.Forms.Button button_HSV;
        private ChartHistogramRGBControl chartHistogramControl1;
        private ChartHistogramHSVControl chartHistogramHSVControl1;
        private System.Windows.Forms.Button button_Gray;
        private System.Windows.Forms.ToolStripMenuItem sAVEToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Button button_Histgram;
        private System.Windows.Forms.Button button_Ana_Fractal;
        private ChartFractalControl chartFractalControl1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}

