namespace video
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if(FinalVideo != null && FinalVideo.IsRunning == true)
                FinalVideo.SignalToStop();
            
            for (int i = 0; i < ListeThread.Length; i++)
            {
                try
                {
                    if (ListeThread[i].IsAlive)
                        ListeThread[i].Abort();
                }
                catch { };
            }
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
            this.ImgNb = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ImgContour = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.LblFPS = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.Blobs = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgNb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageReel
            // 
            this.ImageReel.Location = new System.Drawing.Point(9, 90);
            this.ImageReel.Name = "ImageReel";
            this.ImageReel.Size = new System.Drawing.Size(413, 327);
            this.ImageReel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageReel.TabIndex = 0;
            this.ImageReel.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 425);
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
            // ImgNb
            // 
            this.ImgNb.Location = new System.Drawing.Point(471, 90);
            this.ImgNb.Name = "ImgNb";
            this.ImgNb.Size = new System.Drawing.Size(413, 327);
            this.ImgNb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImgNb.TabIndex = 6;
            this.ImgNb.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(468, 425);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Image noire et blanc";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(904, 425);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Contours";
            // 
            // ImgContour
            // 
            this.ImgContour.Location = new System.Drawing.Point(907, 90);
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
            // Blobs
            // 
            this.Blobs.AutoSize = true;
            this.Blobs.Location = new System.Drawing.Point(843, 30);
            this.Blobs.Name = "Blobs";
            this.Blobs.Size = new System.Drawing.Size(33, 13);
            this.Blobs.TabIndex = 13;
            this.Blobs.Text = "Blobs";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(846, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Chargement image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1359, 657);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Blobs);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.LblFPS);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ImgContour);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ImgNb);
            this.Controls.Add(this.Resolution);
            this.Controls.Add(this.button_Ok);
            this.Controls.Add(this.ListeWebCam);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImageReel);
            this.Name = "Form1";
            this.Text = "Img";
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgNb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
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
        private System.Windows.Forms.PictureBox ImgNb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox ImgContour;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label LblFPS;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label Blobs;
        private System.Windows.Forms.Button button1;
    }
}

