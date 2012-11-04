using System.Windows.Forms;
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
            this.label3 = new System.Windows.Forms.Label();
            this.selecteur_resolution = new System.Windows.Forms.ComboBox();
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
            this.pic_ImageRouge = new System.Windows.Forms.PictureBox();
            this.pic_ImageEdge = new System.Windows.Forms.PictureBox();
            this.pic_ImageNormal = new System.Windows.Forms.PictureBox();
            this.pic_ImageBleu = new System.Windows.Forms.PictureBox();
            this.pic_ImageVert = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbl_lum = new System.Windows.Forms.Label();
            this.lbl_sat = new System.Windows.Forms.Label();
            this.lbl_hue = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageRouge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageEdge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageNormal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageBleu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageVert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Résolution :";
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
            this.numericUpDown1.Location = new System.Drawing.Point(993, 12);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            130,
            0,
            0,
            0});
            // 
            // pic_ImageRouge
            // 
            this.pic_ImageRouge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_ImageRouge.Location = new System.Drawing.Point(72, 410);
            this.pic_ImageRouge.Name = "pic_ImageRouge";
            this.pic_ImageRouge.Size = new System.Drawing.Size(237, 176);
            this.pic_ImageRouge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_ImageRouge.TabIndex = 9;
            this.pic_ImageRouge.TabStop = false;
            // 
            // pic_ImageEdge
            // 
            this.pic_ImageEdge.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_ImageEdge.Location = new System.Drawing.Point(431, 127);
            this.pic_ImageEdge.Name = "pic_ImageEdge";
            this.pic_ImageEdge.Size = new System.Drawing.Size(388, 277);
            this.pic_ImageEdge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_ImageEdge.TabIndex = 7;
            this.pic_ImageEdge.TabStop = false;
            // 
            // pic_ImageNormal
            // 
            this.pic_ImageNormal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_ImageNormal.Location = new System.Drawing.Point(39, 127);
            this.pic_ImageNormal.Name = "pic_ImageNormal";
            this.pic_ImageNormal.Size = new System.Drawing.Size(386, 277);
            this.pic_ImageNormal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_ImageNormal.TabIndex = 5;
            this.pic_ImageNormal.TabStop = false;
            this.pic_ImageNormal.MouseClick += new MouseEventHandler(this.pic_ImageNormal_Click);
            // 
            // pic_ImageBleu
            // 
            this.pic_ImageBleu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_ImageBleu.Location = new System.Drawing.Point(315, 410);
            this.pic_ImageBleu.Name = "pic_ImageBleu";
            this.pic_ImageBleu.Size = new System.Drawing.Size(237, 176);
            this.pic_ImageBleu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_ImageBleu.TabIndex = 10;
            this.pic_ImageBleu.TabStop = false;
            // 
            // pic_ImageVert
            // 
            this.pic_ImageVert.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pic_ImageVert.Location = new System.Drawing.Point(558, 410);
            this.pic_ImageVert.Name = "pic_ImageVert";
            this.pic_ImageVert.Size = new System.Drawing.Size(237, 176);
            this.pic_ImageVert.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_ImageVert.TabIndex = 11;
            this.pic_ImageVert.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(883, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Radius Filtre Couleur";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(870, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Seuil Detection couleur";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(993, 38);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 13;
            this.numericUpDown2.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(870, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Seuil Detection Glyphe";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(993, 63);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown3.TabIndex = 15;
            this.numericUpDown3.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(925, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Hue : ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(932, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Sat :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(928, 192);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Lum :";
            // 
            // lbl_lum
            // 
            this.lbl_lum.AutoSize = true;
            this.lbl_lum.Location = new System.Drawing.Point(967, 192);
            this.lbl_lum.Name = "lbl_lum";
            this.lbl_lum.Size = new System.Drawing.Size(13, 13);
            this.lbl_lum.TabIndex = 22;
            this.lbl_lum.Text = "0";
            // 
            // lbl_sat
            // 
            this.lbl_sat.AutoSize = true;
            this.lbl_sat.Location = new System.Drawing.Point(967, 167);
            this.lbl_sat.Name = "lbl_sat";
            this.lbl_sat.Size = new System.Drawing.Size(13, 13);
            this.lbl_sat.TabIndex = 21;
            this.lbl_sat.Text = "0";
            // 
            // lbl_hue
            // 
            this.lbl_hue.AutoSize = true;
            this.lbl_hue.Location = new System.Drawing.Point(967, 141);
            this.lbl_hue.Name = "lbl_hue";
            this.lbl_hue.Size = new System.Drawing.Size(13, 13);
            this.lbl_hue.TabIndex = 20;
            this.lbl_hue.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 712);
            this.Controls.Add(this.lbl_lum);
            this.Controls.Add(this.lbl_sat);
            this.Controls.Add(this.lbl_hue);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pic_ImageVert);
            this.Controls.Add(this.pic_ImageBleu);
            this.Controls.Add(this.pic_ImageRouge);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.pic_ImageEdge);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pic_ImageNormal);
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
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageRouge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageEdge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageNormal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageBleu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_ImageVert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
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
        private System.Windows.Forms.PictureBox pic_ImageNormal;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel label_etat_programe;
        private System.Windows.Forms.PictureBox pic_ImageEdge;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.PictureBox pic_ImageRouge;
        private System.Windows.Forms.Label txt_resolution;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox selecteur_resolution;
        private System.Windows.Forms.PictureBox pic_ImageBleu;
        private System.Windows.Forms.PictureBox pic_ImageVert;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_lum;
        private System.Windows.Forms.Label lbl_sat;
        private System.Windows.Forms.Label lbl_hue;
    }
}

