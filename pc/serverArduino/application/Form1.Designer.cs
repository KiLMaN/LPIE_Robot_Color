namespace application
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.ImageReel = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImgContour = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Blobs = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblFPS = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Resolution = new System.Windows.Forms.ComboBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.ListeWebCam = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1193, 717);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ImageReel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.ImgContour);
            this.tabPage1.Controls.Add(this.Blobs);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.BtnStop);
            this.tabPage1.Controls.Add(this.LblFPS);
            this.tabPage1.Controls.Add(this.numericUpDown1);
            this.tabPage1.Controls.Add(this.Resolution);
            this.tabPage1.Controls.Add(this.button_Ok);
            this.tabPage1.Controls.Add(this.ListeWebCam);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1185, 691);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Video";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1185, 691);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IA";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(995, 585);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "logs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(643, 380);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // ImageReel
            // 
            this.ImageReel.Location = new System.Drawing.Point(8, 82);
            this.ImageReel.Name = "ImageReel";
            this.ImageReel.Size = new System.Drawing.Size(413, 327);
            this.ImageReel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageReel.TabIndex = 0;
            this.ImageReel.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 412);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Image reçu";
            // 
            // ImgContour
            // 
            this.ImgContour.Location = new System.Drawing.Point(462, 82);
            this.ImgContour.Name = "ImgContour";
            this.ImgContour.Size = new System.Drawing.Size(413, 327);
            this.ImgContour.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImgContour.TabIndex = 8;
            this.ImgContour.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(459, 417);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Contours";
            // 
            // Blobs
            // 
            this.Blobs.AutoSize = true;
            this.Blobs.Location = new System.Drawing.Point(691, 43);
            this.Blobs.Name = "Blobs";
            this.Blobs.Size = new System.Drawing.Size(33, 13);
            this.Blobs.TabIndex = 23;
            this.Blobs.Text = "Blobs";
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(350, 40);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(75, 23);
            this.BtnStop.TabIndex = 22;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            // 
            // LblFPS
            // 
            this.LblFPS.AutoSize = true;
            this.LblFPS.Location = new System.Drawing.Point(495, 42);
            this.LblFPS.Name = "LblFPS";
            this.LblFPS.Size = new System.Drawing.Size(27, 13);
            this.LblFPS.TabIndex = 21;
            this.LblFPS.Text = "FPS";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(492, 17);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(179, 20);
            this.numericUpDown1.TabIndex = 20;
            // 
            // Resolution
            // 
            this.Resolution.FormattingEnabled = true;
            this.Resolution.Location = new System.Drawing.Point(80, 42);
            this.Resolution.Name = "Resolution";
            this.Resolution.Size = new System.Drawing.Size(261, 21);
            this.Resolution.TabIndex = 19;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(350, 14);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(75, 23);
            this.button_Ok.TabIndex = 18;
            this.button_Ok.Text = "Ok";
            this.button_Ok.UseVisualStyleBackColor = true;
            // 
            // ListeWebCam
            // 
            this.ListeWebCam.FormattingEnabled = true;
            this.ListeWebCam.Location = new System.Drawing.Point(80, 14);
            this.ListeWebCam.Name = "ListeWebCam";
            this.ListeWebCam.Size = new System.Drawing.Size(261, 21);
            this.ListeWebCam.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "WebCam";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1193, 717);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.PictureBox ImageReel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox ImgContour;
        private System.Windows.Forms.Label Blobs;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label LblFPS;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox Resolution;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.ComboBox ListeWebCam;
        private System.Windows.Forms.Label label2;
    }
}

