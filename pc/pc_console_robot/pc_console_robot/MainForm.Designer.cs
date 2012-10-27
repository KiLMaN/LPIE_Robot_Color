namespace pc_console_robot
{
    partial class MainForm
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
            this.selecteur_camera = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_refresh_liste_cam = new System.Windows.Forms.Button();
            this.btn_afficher_flux_cam = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txt_resolution = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_nb_fps = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_stopper_flux_video = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.label_etat_programe = new System.Windows.Forms.ToolStripStatusLabel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.selecteur_resolution = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // selecteur_camera
            // 
            this.selecteur_camera.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selecteur_camera.FormattingEnabled = true;
            this.selecteur_camera.Location = new System.Drawing.Point(14, 19);
            this.selecteur_camera.Name = "selecteur_camera";
            this.selecteur_camera.Size = new System.Drawing.Size(250, 21);
            this.selecteur_camera.TabIndex = 0;
            this.selecteur_camera.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.selecteur_resolution);
            this.groupBox1.Controls.Add(this.selecteur_camera);
            this.groupBox1.Controls.Add(this.btn_refresh_liste_cam);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 94);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Caméra Vidéo";
            // 
            // btn_refresh_liste_cam
            // 
            this.btn_refresh_liste_cam.BackgroundImage = global::pc_console_robot.Properties.Resources.actualiser_discret;
            this.btn_refresh_liste_cam.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_refresh_liste_cam.Location = new System.Drawing.Point(269, 18);
            this.btn_refresh_liste_cam.Name = "btn_refresh_liste_cam";
            this.btn_refresh_liste_cam.Size = new System.Drawing.Size(23, 23);
            this.btn_refresh_liste_cam.TabIndex = 1;
            this.btn_refresh_liste_cam.UseVisualStyleBackColor = true;
            this.btn_refresh_liste_cam.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_afficher_flux_cam
            // 
            this.btn_afficher_flux_cam.Location = new System.Drawing.Point(19, 18);
            this.btn_afficher_flux_cam.Name = "btn_afficher_flux_cam";
            this.btn_afficher_flux_cam.Size = new System.Drawing.Size(74, 23);
            this.btn_afficher_flux_cam.TabIndex = 3;
            this.btn_afficher_flux_cam.Text = "Lancer";
            this.btn_afficher_flux_cam.UseVisualStyleBackColor = true;
            this.btn_afficher_flux_cam.Click += new System.EventHandler(this.btn_afficher_flux_cam_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txt_resolution);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txt_nb_fps);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btn_stopper_flux_video);
            this.groupBox2.Controls.Add(this.btn_afficher_flux_cam);
            this.groupBox2.Location = new System.Drawing.Point(334, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 94);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controles";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // txt_resolution
            // 
            this.txt_resolution.AutoSize = true;
            this.txt_resolution.Location = new System.Drawing.Point(99, 67);
            this.txt_resolution.Name = "txt_resolution";
            this.txt_resolution.Size = new System.Drawing.Size(13, 13);
            this.txt_resolution.TabIndex = 11;
            this.txt_resolution.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Résolution :";
            // 
            // txt_nb_fps
            // 
            this.txt_nb_fps.AutoSize = true;
            this.txt_nb_fps.Location = new System.Drawing.Point(99, 49);
            this.txt_nb_fps.Name = "txt_nb_fps";
            this.txt_nb_fps.Size = new System.Drawing.Size(13, 13);
            this.txt_nb_fps.TabIndex = 6;
            this.txt_nb_fps.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "FPS :";
            // 
            // btn_stopper_flux_video
            // 
            this.btn_stopper_flux_video.Enabled = false;
            this.btn_stopper_flux_video.Location = new System.Drawing.Point(99, 18);
            this.btn_stopper_flux_video.Name = "btn_stopper_flux_video";
            this.btn_stopper_flux_video.Size = new System.Drawing.Size(74, 23);
            this.btn_stopper_flux_video.TabIndex = 4;
            this.btn_stopper_flux_video.Text = "Stopper";
            this.btn_stopper_flux_video.UseVisualStyleBackColor = true;
            this.btn_stopper_flux_video.Click += new System.EventHandler(this.btn_stopper_flux_video_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.label_etat_programe});
            this.statusStrip1.Location = new System.Drawing.Point(0, 690);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1125, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // label_etat_programe
            // 
            this.label_etat_programe.Name = "label_etat_programe";
            this.label_etat_programe.Size = new System.Drawing.Size(114, 17);
            this.label_etat_programe.Text = "label_etat_programe";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(892, 86);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Location = new System.Drawing.Point(433, 410);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(386, 277);
            this.pictureBox4.TabIndex = 9;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(39, 410);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(388, 277);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(431, 127);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(386, 277);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(39, 127);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 277);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // selecteur_resolution
            // 
            this.selecteur_resolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.selecteur_resolution.FormattingEnabled = true;
            this.selecteur_resolution.Location = new System.Drawing.Point(80, 49);
            this.selecteur_resolution.Name = "selecteur_resolution";
            this.selecteur_resolution.Size = new System.Drawing.Size(184, 21);
            this.selecteur_resolution.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Résolution :";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 712);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox selecteur_camera;
        private System.Windows.Forms.Button btn_refresh_liste_cam;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_afficher_flux_cam;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_stopper_flux_video;
        private System.Windows.Forms.Label txt_nb_fps;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel label_etat_programe;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label txt_resolution;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox selecteur_resolution;
    }
}

