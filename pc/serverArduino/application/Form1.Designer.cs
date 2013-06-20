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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ImageReel = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ImgContour = new System.Windows.Forms.PictureBox();
            this.Blobs = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblFPS = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Resolution = new System.Windows.Forms.ComboBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.ListeWebCam = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BTN_STOP_IA = new System.Windows.Forms.Button();
            this.BTN_START_IA = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ListArduino = new System.Windows.Forms.ListView();
            this._id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._connected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatComm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatRobot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_Xbee = new System.Windows.Forms.CheckBox();
            this.btn_ActualiserListePortSerie = new System.Windows.Forms.Button();
            this.ctlListePorts = new System.Windows.Forms.ComboBox();
            this.btn_connection = new System.Windows.Forms.Button();
            this.lbl_portSerie = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.RTB_LOG = new System.Windows.Forms.RichTextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            // Blobs
            // 
            this.Blobs.AutoSize = true;
            this.Blobs.Location = new System.Drawing.Point(691, 43);
            this.Blobs.Name = "Blobs";
            this.Blobs.Size = new System.Drawing.Size(33, 13);
            this.Blobs.TabIndex = 23;
            this.Blobs.Text = "Blobs";
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.BTN_STOP_IA);
            this.tabPage2.Controls.Add(this.BTN_START_IA);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.ListArduino);
            this.tabPage2.Controls.Add(this.CB_Xbee);
            this.tabPage2.Controls.Add(this.btn_ActualiserListePortSerie);
            this.tabPage2.Controls.Add(this.ctlListePorts);
            this.tabPage2.Controls.Add(this.btn_connection);
            this.tabPage2.Controls.Add(this.lbl_portSerie);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1185, 691);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "IA";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BTN_STOP_IA
            // 
            this.BTN_STOP_IA.Enabled = false;
            this.BTN_STOP_IA.Location = new System.Drawing.Point(764, 37);
            this.BTN_STOP_IA.Name = "BTN_STOP_IA";
            this.BTN_STOP_IA.Size = new System.Drawing.Size(75, 23);
            this.BTN_STOP_IA.TabIndex = 25;
            this.BTN_STOP_IA.Text = "Stop IA";
            this.BTN_STOP_IA.UseVisualStyleBackColor = true;
            this.BTN_STOP_IA.Click += new System.EventHandler(this.BTN_STOP_IA_Click);
            // 
            // BTN_START_IA
            // 
            this.BTN_START_IA.Location = new System.Drawing.Point(764, 8);
            this.BTN_START_IA.Name = "BTN_START_IA";
            this.BTN_START_IA.Size = new System.Drawing.Size(75, 23);
            this.BTN_START_IA.TabIndex = 24;
            this.BTN_START_IA.Text = "Start IA";
            this.BTN_START_IA.UseVisualStyleBackColor = true;
            this.BTN_START_IA.Click += new System.EventHandler(this.BTN_START_IA_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Robots :";
            // 
            // ListArduino
            // 
            this.ListArduino.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._id,
            this._connected,
            this._etatComm,
            this._etatRobot,
            this._posX,
            this._posY});
            this.ListArduino.Location = new System.Drawing.Point(8, 57);
            this.ListArduino.Name = "ListArduino";
            this.ListArduino.Size = new System.Drawing.Size(650, 87);
            this.ListArduino.TabIndex = 22;
            this.ListArduino.UseCompatibleStateImageBehavior = false;
            this.ListArduino.View = System.Windows.Forms.View.Details;
            // 
            // _id
            // 
            this._id.Tag = "_id";
            this._id.Text = "ID";
            // 
            // _connected
            // 
            this._connected.Tag = "_connected";
            this._connected.Text = "Connecté";
            this._connected.Width = 101;
            // 
            // _etatComm
            // 
            this._etatComm.Tag = "_etatComm";
            this._etatComm.Text = "EtatComm";
            this._etatComm.Width = 154;
            // 
            // _etatRobot
            // 
            this._etatRobot.Tag = "_etatRobot";
            this._etatRobot.Text = "EtatRobot";
            this._etatRobot.Width = 168;
            // 
            // _posX
            // 
            this._posX.Tag = "_posX";
            this._posX.Text = "posX";
            this._posX.Width = 76;
            // 
            // _posY
            // 
            this._posY.Tag = "_posY";
            this._posY.Text = "posY";
            this._posY.Width = 81;
            // 
            // CB_Xbee
            // 
            this.CB_Xbee.AutoSize = true;
            this.CB_Xbee.Checked = true;
            this.CB_Xbee.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_Xbee.Location = new System.Drawing.Point(246, 12);
            this.CB_Xbee.Name = "CB_Xbee";
            this.CB_Xbee.Size = new System.Drawing.Size(68, 17);
            this.CB_Xbee.TabIndex = 21;
            this.CB_Xbee.Text = "XbeeAPI";
            this.CB_Xbee.UseVisualStyleBackColor = true;
            this.CB_Xbee.CheckedChanged += new System.EventHandler(this.CB_Xbee_CheckedChanged);
            // 
            // btn_ActualiserListePortSerie
            // 
            this.btn_ActualiserListePortSerie.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_ActualiserListePortSerie.Image = ((System.Drawing.Image)(resources.GetObject("btn_ActualiserListePortSerie.Image")));
            this.btn_ActualiserListePortSerie.Location = new System.Drawing.Point(71, 9);
            this.btn_ActualiserListePortSerie.Name = "btn_ActualiserListePortSerie";
            this.btn_ActualiserListePortSerie.Size = new System.Drawing.Size(29, 23);
            this.btn_ActualiserListePortSerie.TabIndex = 20;
            this.btn_ActualiserListePortSerie.UseVisualStyleBackColor = true;
            this.btn_ActualiserListePortSerie.Click += new System.EventHandler(this.btn_ActualiserListePortSerie_Click);
            // 
            // ctlListePorts
            // 
            this.ctlListePorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ctlListePorts.FormattingEnabled = true;
            this.ctlListePorts.Location = new System.Drawing.Point(106, 10);
            this.ctlListePorts.Name = "ctlListePorts";
            this.ctlListePorts.Size = new System.Drawing.Size(134, 21);
            this.ctlListePorts.TabIndex = 18;
            // 
            // btn_connection
            // 
            this.btn_connection.Location = new System.Drawing.Point(320, 10);
            this.btn_connection.Name = "btn_connection";
            this.btn_connection.Size = new System.Drawing.Size(75, 23);
            this.btn_connection.TabIndex = 17;
            this.btn_connection.Text = "Connection";
            this.btn_connection.UseVisualStyleBackColor = true;
            this.btn_connection.Click += new System.EventHandler(this.btn_connection_Click);
            // 
            // lbl_portSerie
            // 
            this.lbl_portSerie.AutoSize = true;
            this.lbl_portSerie.Location = new System.Drawing.Point(6, 14);
            this.lbl_portSerie.Name = "lbl_portSerie";
            this.lbl_portSerie.Size = new System.Drawing.Size(59, 13);
            this.lbl_portSerie.TabIndex = 19;
            this.lbl_portSerie.Text = "Port Série :";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.RTB_LOG);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1185, 691);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "logs";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // RTB_LOG
            // 
            this.RTB_LOG.Location = new System.Drawing.Point(6, 6);
            this.RTB_LOG.Name = "RTB_LOG";
            this.RTB_LOG.Size = new System.Drawing.Size(643, 380);
            this.RTB_LOG.TabIndex = 0;
            this.RTB_LOG.Text = "";
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
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgContour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox RTB_LOG;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView ListArduino;
        private System.Windows.Forms.ColumnHeader _id;
        private System.Windows.Forms.ColumnHeader _connected;
        private System.Windows.Forms.ColumnHeader _etatComm;
        private System.Windows.Forms.ColumnHeader _etatRobot;
        private System.Windows.Forms.ColumnHeader _posX;
        private System.Windows.Forms.ColumnHeader _posY;
        private System.Windows.Forms.CheckBox CB_Xbee;
        private System.Windows.Forms.Button btn_ActualiserListePortSerie;
        private System.Windows.Forms.ComboBox ctlListePorts;
        private System.Windows.Forms.Button btn_connection;
        private System.Windows.Forms.Label lbl_portSerie;
        private System.Windows.Forms.Button BTN_STOP_IA;
        private System.Windows.Forms.Button BTN_START_IA;
        private System.IO.Ports.SerialPort serialPort1;
    }
}

