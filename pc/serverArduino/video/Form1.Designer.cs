namespace video
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            //VP.Dispose();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
     
        #region Code généré par le Concepteur Windows Form

        private void InitializeComponent()
        {
            this.ImageReel = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ListeWebCam = new System.Windows.Forms.ComboBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.Resolution = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ImgContour = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.LblFPS = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Log = new System.Windows.Forms.RichTextBox();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown7 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageReel
            // 
            this.ImageReel.Location = new System.Drawing.Point(6, 21);
            this.ImageReel.Name = "ImageReel";
            this.ImageReel.Size = new System.Drawing.Size(413, 327);
            this.ImageReel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageReel.TabIndex = 0;
            this.ImageReel.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 351);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image reçu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "WebCam";
            // 
            // ListeWebCam
            // 
            this.ListeWebCam.FormattingEnabled = true;
            this.ListeWebCam.Location = new System.Drawing.Point(80, 9);
            this.ListeWebCam.Name = "ListeWebCam";
            this.ListeWebCam.Size = new System.Drawing.Size(261, 21);
            this.ListeWebCam.TabIndex = 3;
            this.ListeWebCam.SelectedIndexChanged += new System.EventHandler(this.ListeWebCam_SelectedIndexChanged);
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(350, 9);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 4;
            this.button_Ok.Text = "Ok";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.ValideCamera_Click);
            // 
            // Resolution
            // 
            this.Resolution.FormattingEnabled = true;
            this.Resolution.Location = new System.Drawing.Point(80, 37);
            this.Resolution.Name = "Resolution";
            this.Resolution.Size = new System.Drawing.Size(261, 21);
            this.Resolution.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(457, 356);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Contours";
            // 
            // ImgContour
            // 
            this.ImgContour.Location = new System.Drawing.Point(460, 21);
            this.ImgContour.Name = "ImgContour";
            this.ImgContour.Size = new System.Drawing.Size(413, 327);
            this.ImgContour.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImgContour.TabIndex = 8;
            this.ImgContour.TabStop = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(492, 12);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(179, 20);
            this.numericUpDown1.TabIndex = 10;
            // 
            // LblFPS
            // 
            this.LblFPS.AutoSize = true;
            this.LblFPS.Location = new System.Drawing.Point(495, 37);
            this.LblFPS.Name = "LblFPS";
            this.LblFPS.Size = new System.Drawing.Size(27, 13);
            this.LblFPS.TabIndex = 11;
            this.LblFPS.Text = "FPS";
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(350, 35);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 12;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 78);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(915, 567);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ImageReel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ImgContour);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(907, 541);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Image";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Log);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(907, 541);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Log
            // 
            this.Log.Location = new System.Drawing.Point(13, 17);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(719, 496);
            this.Log.TabIndex = 0;
            this.Log.Text = "";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(692, 18);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown2.TabIndex = 16;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(692, 44);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown3.TabIndex = 17;
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.Location = new System.Drawing.Point(761, 18);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown4.TabIndex = 18;
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.Location = new System.Drawing.Point(761, 44);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown5.TabIndex = 19;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.Location = new System.Drawing.Point(830, 18);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown6.TabIndex = 20;
            // 
            // numericUpDown7
            // 
            this.numericUpDown7.Location = new System.Drawing.Point(830, 44);
            this.numericUpDown7.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown7.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 657);
            this.Controls.Add(this.numericUpDown7);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.LblFPS);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.Resolution);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.ListeWebCam);
            this.Controls.Add(this.label2);
            this.Name = "Form1";
            this.Text = "Img";
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown7)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ImageReel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ListeWebCam;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.ComboBox Resolution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox ImgContour;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label LblFPS;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox Log;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.NumericUpDown numericUpDown7;
    }
}

