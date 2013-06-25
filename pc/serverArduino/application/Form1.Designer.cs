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
            this.Blobs = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.LblFPS = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.Resolution = new System.Windows.Forms.ComboBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.ListeWebCam = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imageDebug = new Emgu.CV.UI.ImageBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ListeZones = new System.Windows.Forms.ListView();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ListeCubes = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.ListeArduino = new System.Windows.Forms.ListView();
            this._id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._connected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatComm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatRobot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._angle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_Xbee = new System.Windows.Forms.CheckBox();
            this.btn_ActualiserListePortSerie = new System.Windows.Forms.Button();
            this.ctlListePorts = new System.Windows.Forms.ComboBox();
            this.btn_connection = new System.Windows.Forms.Button();
            this.lbl_portSerie = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.RTB_LOG = new System.Windows.Forms.RichTextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.debugIA = new System.Windows.Forms.PictureBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageReel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageDebug)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.debugIA)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1193, 717);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.ImageReel);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.Blobs);
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
            this.ImageReel.Size = new System.Drawing.Size(1152, 516);
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
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
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
            this.numericUpDown1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
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
            this.button_Ok.Click += new System.EventHandler(this.ValideCamera_Click);
            // 
            // ListeWebCam
            // 
            this.ListeWebCam.FormattingEnabled = true;
            this.ListeWebCam.Location = new System.Drawing.Point(80, 14);
            this.ListeWebCam.Name = "ListeWebCam";
            this.ListeWebCam.Size = new System.Drawing.Size(261, 21);
            this.ListeWebCam.TabIndex = 17;
            this.ListeWebCam.SelectedIndexChanged += new System.EventHandler(this.ListeWebCam_SelectedIndexChanged);
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
            this.tabPage2.Controls.Add(this.imageDebug);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.ListeZones);
            this.tabPage2.Controls.Add(this.ListeCubes);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.ListeArduino);
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
            // imageDebug
            // 
            this.imageDebug.Location = new System.Drawing.Point(20, 416);
            this.imageDebug.Name = "imageDebug";
            this.imageDebug.Size = new System.Drawing.Size(538, 244);
            this.imageDebug.TabIndex = 28;
            this.imageDebug.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 307);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Zones";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Cubes";
            // 
            // ListeZones
            // 
            this.ListeZones.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.ListeZones.Location = new System.Drawing.Point(8, 323);
            this.ListeZones.Name = "ListeZones";
            this.ListeZones.Size = new System.Drawing.Size(1160, 87);
            this.ListeZones.TabIndex = 25;
            this.ListeZones.UseCompatibleStateImageBehavior = false;
            this.ListeZones.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Tag = "_id";
            this.columnHeader8.Text = "ID";
            this.columnHeader8.Width = 70;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Tag = "_connected";
            this.columnHeader9.Text = "Postions Coins";
            this.columnHeader9.Width = 300;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Tag = "_etatComm";
            this.columnHeader10.Text = "Position Centre";
            this.columnHeader10.Width = 154;
            // 
            // ListeCubes
            // 
            this.ListeCubes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.ListeCubes.Location = new System.Drawing.Point(8, 163);
            this.ListeCubes.Name = "ListeCubes";
            this.ListeCubes.Size = new System.Drawing.Size(1160, 141);
            this.ListeCubes.TabIndex = 24;
            this.ListeCubes.UseCompatibleStateImageBehavior = false;
            this.ListeCubes.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "_id";
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "_connected";
            this.columnHeader2.Text = "ID_Zone";
            this.columnHeader2.Width = 101;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Tag = "_etatComm";
            this.columnHeader3.Text = "FAIT";
            this.columnHeader3.Width = 154;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Tag = "_etatRobot";
            this.columnHeader4.Text = "ID_ROBOT";
            this.columnHeader4.Width = 168;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Tag = "_posX";
            this.columnHeader5.Text = "posX";
            this.columnHeader5.Width = 76;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Tag = "_posY";
            this.columnHeader6.Text = "posY";
            this.columnHeader6.Width = 59;
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
            // ListeArduino
            // 
            this.ListeArduino.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._id,
            this._connected,
            this._etatComm,
            this._etatRobot,
            this._posX,
            this._posY,
            this._angle});
            this.ListeArduino.Location = new System.Drawing.Point(8, 57);
            this.ListeArduino.Name = "ListeArduino";
            this.ListeArduino.Size = new System.Drawing.Size(1160, 87);
            this.ListeArduino.TabIndex = 22;
            this.ListeArduino.UseCompatibleStateImageBehavior = false;
            this.ListeArduino.View = System.Windows.Forms.View.Details;
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
            this._posY.Width = 59;
            // 
            // _angle
            // 
            this._angle.Tag = "Angle";
            this._angle.Text = "Angle";
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
            this.RTB_LOG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTB_LOG.Location = new System.Drawing.Point(3, 3);
            this.RTB_LOG.Name = "RTB_LOG";
            this.RTB_LOG.Size = new System.Drawing.Size(1179, 685);
            this.RTB_LOG.TabIndex = 0;
            this.RTB_LOG.Text = "";
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1185, 691);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "ImageDebug";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.debugIA);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1185, 691);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "IADebug";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // debugIA
            // 
            this.debugIA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugIA.Location = new System.Drawing.Point(3, 3);
            this.debugIA.Name = "debugIA";
            this.debugIA.Size = new System.Drawing.Size(1179, 685);
            this.debugIA.TabIndex = 0;
            this.debugIA.TabStop = false;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(61, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(8, 8);
            this.listView1.TabIndex = 24;
            this.listView1.UseCompatibleStateImageBehavior = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageDebug)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.debugIA)).EndInit();
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
        private System.Windows.Forms.Label Blobs;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Label LblFPS;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.ComboBox Resolution;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.ComboBox ListeWebCam;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListView ListeArduino;
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
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ColumnHeader _angle;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListView ListeZones;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ListView ListeCubes;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.PictureBox debugIA;
        private Emgu.CV.UI.ImageBox imageDebug;
        private System.Windows.Forms.ListView listView1;
    }
}

